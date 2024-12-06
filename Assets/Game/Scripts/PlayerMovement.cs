using System;
using System.Collections;
using System.Collections.Generic;
using Game.Scripts.Enemy;
using Game.Scripts.Obstacle;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Scripts
{
    public class PlayerMovement : MonoBehaviour
    {
        public GameObject playerUnit;  // The player unit (Cube or prefab)
        public float moveSpeed = 2f;   // Movement speed of the player
        public ObstacleData obstacleData;  // Reference to the obstacle data (from Assignment 2)
        public Animator animator;
        
        public bool isMoving = false;
        private Queue<Vector3> movementQueue = new Queue<Vector3>();

        private void Update()
        {
            
            if (isMoving)
            {
                animator.SetBool("IsMoving", true);
            }
            else
            {
                animator.SetBool("IsMoving", false);
            }
            if (!isMoving && movementQueue.Count > 0)
            {
                Vector3 nextPosition = movementQueue.Dequeue();
                StartCoroutine(MoveToTarget(nextPosition));
            }

            if (Input.GetKeyDown(KeyCode.Mouse0) && !isMoving)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    Tile tile = hit.collider.GetComponent<Tile>();
                    if (tile != null)
                    {
                        if (!tile.isObstacle)
                        {
                            BfsToTile(tile);
                        }
                        else
                        {
                            Debug.Log("Tile is an obstacle");
                        }
                    }
                }
            }
        }

        void BfsToTile(Tile targetTile)
        {
            // BFS to find the shortest path
            Vector2Int originalPosition = new Vector2Int(
                Mathf.RoundToInt(playerUnit.transform.position.x),
                Mathf.RoundToInt(playerUnit.transform.position.z)
            );

            Vector2Int target = targetTile.gridPosition;


            Queue<Vector2Int> queue = new Queue<Vector2Int>();
            Dictionary<Vector2Int, Vector2Int> cameFrom = new Dictionary<Vector2Int, Vector2Int>();
            HashSet<Vector2Int> visited = new HashSet<Vector2Int>();

            queue.Enqueue(originalPosition);
            visited.Add(originalPosition);

            while (queue.Count > 0)
            {
                Vector2Int current = queue.Dequeue();

                if (current == target)
                {
                    BackTrackPath(originalPosition, target, cameFrom);
                    return;
                }
                foreach (Vector2Int neighbor in GetNeighborsPos(current))
                {

                    if (!visited.Contains(neighbor) && !IsObstacle(neighbor))
                    {
                        queue.Enqueue(neighbor);
                        visited.Add(neighbor);
                        cameFrom[neighbor] = current;
                    }
                }
            }
        }

        void BackTrackPath(Vector2Int start, Vector2Int target, Dictionary<Vector2Int, Vector2Int> cameFrom)
        {
            List<Vector3> path = new List<Vector3>();
            Vector2Int current = target;

            while (current != start)
            {
                Vector3 worldPosition = new Vector3(current.x, playerUnit.transform.position.y, current.y);
                path.Add(worldPosition);
                current = cameFrom[current];
            }

            path.Reverse();  // Reverse to get the path from start to target
            AddToPath(path);
        }

        void AddToPath(List<Vector3> path)
        {
            foreach (Vector3 position in path)
            {
                movementQueue.Enqueue(position);
            }
        }

        System.Collections.IEnumerator MoveToTarget(Vector3 targetPosition)
        {
            isMoving = true;
            if (gameObject.transform.position.x < targetPosition.x)
            {
                playerUnit.transform.transform.rotation = Quaternion.Euler(0, 90, 0);
            }
            else if (gameObject.transform.position.x > targetPosition.x)
            {
                playerUnit.transform.transform.rotation = Quaternion.Euler(0, -90, 0);
            }

            else if (gameObject.transform.position.z < targetPosition.z)
            {
                playerUnit.transform.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else if (gameObject.transform.position.z > targetPosition.z)
            {
                playerUnit.transform.transform.rotation = Quaternion.Euler(0, -180, 0);
            }

            while (Vector3.Distance(playerUnit.transform.position, targetPosition) > 0.1f)
            {
                playerUnit.transform.position = Vector3.MoveTowards(playerUnit.transform.position, targetPosition, moveSpeed * Time.deltaTime);
                yield return null;
            }

            playerUnit.transform.position = targetPosition;  // Snap to target position
            isMoving = false;  // Movement complete
        }

        List<Vector2Int> GetNeighborsPos(Vector2Int position)
        {
            List<Vector2Int> neighbors = new List<Vector2Int>
            {
                new Vector2Int(position.x, position.y + 1), // Up
                new Vector2Int(position.x, position.y - 1), // Down
                new Vector2Int(position.x + 1, position.y), // Right
                new Vector2Int(position.x - 1, position.y)  // Left
            };

            // remove outside grid values
            neighbors.RemoveAll(neighbor => !IsValidPosition(neighbor));
            return neighbors;
        }

        bool IsObstacle(Vector2Int position)
        {
            Tile tile = GetTileAtPosition(position);
            return !tile || tile.isObstacle;
        }

        bool IsValidPosition(Vector2Int position)
        {
            Tile tile = GetTileAtPosition(position);
            return tile ;
        }

        static Tile GetTileAtPosition(Vector2Int position)
        {
            Ray ray = new Ray(new Vector3(position.x, 2, position.y), Vector3.down);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                return hit.collider.GetComponent<Tile>();
            }
            return null;
        }
    }
}

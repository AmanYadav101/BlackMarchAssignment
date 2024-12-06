using System.Collections;
using System.Collections.Generic;
using Game.Scripts.Enemy;
using Game.Scripts.Obstacle;
using UnityEngine;

namespace Game.Scripts
{
    public enum Turn
    {
        Player,
        Enemy,
    }

    public class CharacterMovement : MonoBehaviour
    {
        public bool isMoving = false;
        public ObstacleData obstacleData;
        public EnemyObstacleData enemyObstacleData;

        // public GameObject characterUnit; // The player unit (Cube or prefab)
        public float moveSpeed = 2f; // Movement speed of the player
        protected Queue<Vector3> movementQueue = new Queue<Vector3>();
        protected static Turn turnToMove = Turn.Player;
        protected  Vector2 finalTarget = Vector2.negativeInfinity;
        protected  Vector2Int originalPosition = new Vector2Int { x = -1, y = -1 };


        protected IEnumerator MoveToTarget(Vector3 targetPosition, Turn changeTurnToMoveTo)
        {
            isMoving = true;
            if (gameObject.transform.position.x < targetPosition.x)
            {
                gameObject.transform.transform.rotation = Quaternion.Euler(0, 90, 0);
            }
            else if (gameObject.transform.position.x > targetPosition.x)
            {
                gameObject.transform.transform.rotation = Quaternion.Euler(0, -90, 0);
            }
            else if (gameObject.transform.position.z < targetPosition.z)
            {
                gameObject.transform.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else if (gameObject.transform.position.z > targetPosition.z)
            {
                gameObject.transform.transform.rotation = Quaternion.Euler(0, -180, 0);
            }

            while (Vector3.Distance(gameObject.transform.position, targetPosition) > 0.1f)
            {
                gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, targetPosition,
                    moveSpeed * Time.deltaTime);
                yield return null;
            }

            gameObject.transform.position = targetPosition; // Snap to target position
            isMoving = false; // Movement complete
            if (finalTarget != Vector2.negativeInfinity && Mathf.Approximately(finalTarget.x, targetPosition.x) &&
                Mathf.Approximately(finalTarget.y, targetPosition.z))
            {
                turnToMove = changeTurnToMoveTo;
            }
        }

        protected void BfsToTile(Tile targetTile)
        {
            finalTarget.x = targetTile.gridPosition.x;
            finalTarget.y = targetTile.gridPosition.y;

            // BFS to find the shortest path
            originalPosition = new Vector2Int(
                Mathf.RoundToInt(gameObject.transform.position.x),
                Mathf.RoundToInt(gameObject.transform.position.z)
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
                    if (!visited.Contains(neighbor) && !IsObstacle(neighbor) && !IsEnemy(neighbor))
                    {
                        queue.Enqueue(neighbor);
                        visited.Add(neighbor);
                        cameFrom[neighbor] = current;
                    }
                }
            }
        }

        private void BackTrackPath(Vector2Int start, Vector2Int target, Dictionary<Vector2Int, Vector2Int> cameFrom)
        {
            List<Vector3> path = new List<Vector3>();
            Vector2Int current = target;

            while (current != start)
            {
                Vector3 worldPosition = new Vector3(current.x, gameObject.transform.position.y, current.y);
                path.Add(worldPosition);
                current = cameFrom[current];
            }

            path.Reverse(); // Reverse to get the path from start to target
            AddToPath(path);
        }

        protected void AddToPath(List<Vector3> path)
        {
            foreach (Vector3 position in path)
            {
                movementQueue.Enqueue(position);
            }
        }

        protected List<Vector2Int> GetNeighborsPos(Vector2Int position)
        {
            List<Vector2Int> neighbors = new List<Vector2Int>
            {
                new Vector2Int(position.x, position.y + 1), // Up
                new Vector2Int(position.x, position.y - 1), // Down
                new Vector2Int(position.x + 1, position.y), // Right
                new Vector2Int(position.x - 1, position.y) // Left
            };


            // remove outside grid values
            neighbors.RemoveAll(neighbor => !IsValidPosition(neighbor));
            return neighbors;
        }

        private bool IsObstacle(Vector2Int position)
        {
            return obstacleData.GetObstacleAt(position.x, position.y) ;
        }
        
        private bool IsEnemy(Vector2Int position)
        {
            return enemyObstacleData.GetEnemiesAt(position.x, position.y) ;
        }

        private bool IsValidPosition(Vector2Int position)
        {
            return position is { x: >= 0 and <= 9, y: >= 0 and <= 9 };
        }

        protected static Tile GetTileAtPosition(Vector2Int position)
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
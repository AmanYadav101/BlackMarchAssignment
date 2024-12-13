using System.Collections;
using System.Collections.Generic;
using Game.Scripts.Enemy;
using Game.Scripts.StateManagers;
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
        public Animator animator;

        // public GameObject characterUnit; // The player unit (Cube or prefab)
        public float moveSpeed = 2f; // Movement speed of the player
        protected Queue<Vector3> movementQueue = new Queue<Vector3>();
        protected static Turn turnToMove = Turn.Player;
        protected Vector2 finalTarget = Vector2.negativeInfinity;
        protected Vector2Int originalPosition = new Vector2Int { x = -1, y = -1 };

        /// <summary>
        /// Move character gameObject to the targetPosition.
        /// </summary>
        /// <param name="targetPosition">Target position to move to.</param>
        /// <param name="changeTurnToMoveTo">Change the turn of moving to.</param>
        protected IEnumerator MoveToTarget(Vector3 targetPosition, Turn changeTurnToMoveTo)
        {
            isMoving = true;

            animator.SetBool("IsMoving", isMoving);

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

            isMoving = false; // Movement complete
            animator.SetBool("IsMoving", isMoving);

            gameObject.transform.position = targetPosition; // Snap to target position
            if (finalTarget != Vector2.negativeInfinity && Mathf.Approximately(finalTarget.x, targetPosition.x) &&
                Mathf.Approximately(finalTarget.y, targetPosition.z))
            {
                turnToMove = changeTurnToMoveTo;
            }
        }

        /// <summary>
        /// BFS to find the shortest path between the character and the targetTile.
        /// </summary>
        /// <param name="targetTileStateManager">Target tile</param>
        protected bool BfsToTile(TileStateManager targetTileStateManager)
        {
            // BFS to find the shortest path

            finalTarget.x = targetTileStateManager.gridPosition.x;
            finalTarget.y = targetTileStateManager.gridPosition.y;

            originalPosition = new Vector2Int(
                Mathf.RoundToInt(gameObject.transform.position.x),
                Mathf.RoundToInt(gameObject.transform.position.z)
            );
            Vector2Int target = targetTileStateManager.gridPosition;

            Queue<Vector2Int> queue = new Queue<Vector2Int>();
            Dictionary<Vector2Int, Vector2Int> cameFrom = new Dictionary<Vector2Int, Vector2Int>();
            HashSet<Vector2Int> visited = new HashSet<Vector2Int>();

            queue.Enqueue(originalPosition);
            visited.Add(originalPosition);


            while (queue.Count > 0)
            {
                // get the current element from the start of the queue
                Vector2Int current = queue.Dequeue();

                if (current == target)
                {
                    // get path and it to the queue movement
                    BackTrackPath(originalPosition, target, cameFrom);
                    return true;
                }

                foreach (Vector2Int neighbor in GetNeighborsPos(current))
                {
                    // check if the neighbor is an obstacle or an enemy or is already visited
                    if (visited.Contains(neighbor) || IsObstacle(neighbor) || IsEnemy(neighbor)) continue;
                    // add neighbor to the queue and mark it as visited
                    queue.Enqueue(neighbor);
                    visited.Add(neighbor);

                    // add neighbor's parent 
                    cameFrom[neighbor] = current;
                }
            }

            // when path is not found.
            if (turnToMove == Turn.Player)
            {
                Debug.LogError("No possible path found");
            }
            else
            {
                turnToMove = Turn.Player;
            }

            return false;
        }

        /// <summary>
        /// Backtrack the path taken from start to target using cameFrom dictionary.
        /// </summary>
        /// <param name="start">Starting position.</param>
        /// <param name="target">Target position.</param>
        /// <param name="cameFrom">dictionary of path taken.</param>
        private void BackTrackPath(Vector2Int start, Vector2Int target, Dictionary<Vector2Int, Vector2Int> cameFrom)
        {
            List<Vector3> path = new List<Vector3>();
            Vector2Int current = target;

            // traverse the path using cameFrom dictionary from target to the start.
            while (current != start)
            {
                Vector3 worldPosition = new Vector3(current.x, gameObject.transform.position.y, current.y);
                path.Add(worldPosition);
                current = cameFrom[current];
            }

            // Reverse to get the path from start to target
            path.Reverse();

            AddToPath(path);
        }

        /// <summary>
        /// Add path List to the movementQueue
        /// </summary>
        /// <param name="path">path to add to the movementQueue</param>
        private void AddToPath(List<Vector3> path)
        {
            foreach (Vector3 position in path)
            {
                movementQueue.Enqueue(position);
            }
        }

        /// <summary>
        /// Get all neighboring grid values.
        /// </summary>
        /// <param name="position">find neighbors of position.</param>
        /// <returns>A list of neighbors grid position.</returns>
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

        /// <summary>
        /// Check the obstacleData if the position is an obstacle or not.
        /// </summary>
        /// <param name="position">grid position to check for obstacle.</param>
        /// <returns>boolean value if the given position is an obstacle or not.</returns>
        private bool IsObstacle(Vector2Int position)
        {
            return obstacleData.GetObstacleAt(position.x, position.y);
        }

        /// <summary>
        /// Check if there is an enemy at the position or not.
        /// </summary>
        /// <param name="position">grid position to check for enemy.</param>
        /// <returns>boolean value if the given position is an enemy or not.</returns>
        private bool IsEnemy(Vector2Int position)
        {
            return enemyObstacleData.GetEnemiesAt(position.x, position.y);
        }

        /// <summary>
        /// Check if the position is on the grid or not.
        /// </summary>
        /// <param name="position">grid position to check if it is valid.</param>
        /// <returns>boolean value if the given position is valid or not.</returns>
        private bool IsValidPosition(Vector2Int position)
        {
            return position is { x: >= 0 and <= 9, y: >= 0 and <= 9 };
        }

        /// <summary>
        /// Get the tile under the given position using ray casting in downward direction.
        /// </summary>
        /// <param name="position">position to get tile from.</param>
        /// <returns>null if Tile not found else returns Tile object</returns>
        protected static TileStateManager GetTileAtPosition(Vector2Int position)
        {
            // ray casting from the position in downward direction
            Ray ray = new Ray(new Vector3(position.x, 2, position.y), Vector3.down);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                return hit.collider.GetComponent<TileStateManager>();
            }

            return null;
        }
    }
}
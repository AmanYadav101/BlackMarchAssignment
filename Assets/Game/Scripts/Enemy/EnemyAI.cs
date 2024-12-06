using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Game.Scripts.Enemy
{
    public class EnemyAI : MonoBehaviour, IAI
    {
        public Transform player; // Assign the player's Transform in the Inspector
        public float moveSpeed = 3f; // Speed of the enemy
        private bool _isMoving = false;

        private Vector2Int _enemyGridPosition;
        private Vector2Int _playerGridPosition;

        public PlayerMovement playerMovement;
        private float playerNotMovingTimer = 0f; // Timer to track how long the player has been stationary
        private float delayBeforeMoving = 1f;   // The amount of time (in seconds) the player needs to stop moving before the enemy moves

        private void Awake()
        {
            playerMovement = GameObject.FindObjectOfType<PlayerMovement>();
        }

        private void Start()
        {
            _enemyGridPosition = GetGridPosition(transform.position);
        }

       
        private void Update()
        {
            if (player == null)
            {
                Debug.LogWarning("Player Transform not assigned!");
                return;
            }

            if (!playerMovement.isMoving)
            {
                playerNotMovingTimer += Time.deltaTime; 
            }
            else
            {
                playerNotMovingTimer = 0f; 
            }

            if (playerNotMovingTimer >= delayBeforeMoving)
            {
                if (!_isMoving)
                {
                    // Get the player's current grid position
                    _playerGridPosition = GetGridPosition(player.position);

                    // Stop moving if adjacent to the player
                    if (IsAdjacentToPlayer(_enemyGridPosition, _playerGridPosition))
                    {
                        Debug.Log("Enemy is adjacent to the player. Stopping movement.");
                        return;
                    }

                    // Move towards the player's grid position
                    Vector2Int nextPosition = GetNextGridPosition(_enemyGridPosition, _playerGridPosition);
                    if (nextPosition != _enemyGridPosition)
                    {
                        StartCoroutine(MoveToGridPosition(nextPosition));
                    }
                }
            }
        }

        /// <summary>
        /// Checks if the enemy is adjacent to the player.
        /// </summary>
        private bool IsAdjacentToPlayer(Vector2Int enemyPos, Vector2Int playerPos)
        {
            List<Vector2Int> neighbors = GetNeighbors(playerPos);
            return neighbors.Contains(enemyPos);
        }

        /// <summary>
        /// Moves the enemy to the next grid position over time.
        /// </summary>
        IEnumerator MoveToGridPosition(Vector2Int targetPosition)
        {
            _isMoving = true;
            Vector3 targetWorldPosition = GridToWorldPosition(targetPosition);

            while (Vector3.Distance(transform.position, targetWorldPosition) > 0.1f)
            {
                transform.position =
                    Vector3.MoveTowards(transform.position, targetWorldPosition, moveSpeed * Time.deltaTime);
                yield return null;
            }

            transform.position = targetWorldPosition; // Snap to target position
            _enemyGridPosition = targetPosition; // Update the grid position
            _isMoving = false;
        }

        /// <summary>
        /// Calculates the next grid position toward the player.
        /// </summary>
        private Vector2Int GetNextGridPosition(Vector2Int current, Vector2Int target)
        {
            List<Vector2Int> neighbors = GetNeighbors(current);
            Vector2Int closest = current;
            float closestDistance = float.MaxValue;

            foreach (Vector2Int neighbor in neighbors)
            {
                if (!IsObstacle(neighbor))
                {
                    float distance = Vector2Int.Distance(neighbor, target);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closest = neighbor;
                    }
                }
            }

            return closest;
        }

        /// <summary>
        /// Gets the valid neighbors for the current grid position.
        /// </summary>
        private List<Vector2Int> GetNeighbors(Vector2Int position)
        {
            List<Vector2Int> neighbors = new List<Vector2Int>
            {
                new Vector2Int(position.x, position.y + 1), // Up
                new Vector2Int(position.x, position.y - 1), // Down
                new Vector2Int(position.x + 1, position.y), // Right
                new Vector2Int(position.x - 1, position.y) // Left
            };

            neighbors.RemoveAll(neighbor => !IsValidPosition(neighbor));
            return neighbors;
        }

        /// <summary>
        /// Converts a world position to grid coordinates.
        /// </summary>
        private Vector2Int GetGridPosition(Vector3 worldPosition)
        {
            return new Vector2Int(
                Mathf.RoundToInt(worldPosition.x),
                Mathf.RoundToInt(worldPosition.z)
            );
        }

        /// <summary>
        /// Converts grid coordinates to a world position.
        /// </summary>
        private Vector3 GridToWorldPosition(Vector2Int gridPosition)
        {
            return new Vector3(gridPosition.x, transform.position.y, gridPosition.y);
        }

        /// <summary>
        /// Checks if the position is valid (not an obstacle and within bounds).
        /// </summary>
        private bool IsValidPosition(Vector2Int position)
        {
            Tile tile = GetTileAtPosition(position);
            return tile != null && !tile.isObstacle;
        }

        /// <summary>
        /// Checks if the given grid position is an obstacle.
        /// </summary>
        private bool IsObstacle(Vector2Int position)
        {
            Tile tile = GetTileAtPosition(position);
            return tile == null || tile.isObstacle;
        }

        /// <summary>
        /// Retrieves the tile at the given grid position.
        /// </summary>
        private static Tile GetTileAtPosition(Vector2Int position)
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
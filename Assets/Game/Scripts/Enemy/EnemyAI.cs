using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;

namespace Game.Scripts.Enemy
{
    public class EnemyAI : CharacterMovement, IAI
    {
        private Transform player; // Assign the player's Transform in the Inspector

        private Vector2Int _enemyGridPosition;
        private Vector2Int _playerGridPosition;

        private float playerNotMovingTimer = 0f; // Timer to track how long the player has been stationary

        private float
            delayBeforeMoving =
                1f; // The amount of time (in seconds) the player needs to stop moving before the enemy moves

        private void Awake()
        {
            player = GameObject.FindAnyObjectByType<PlayerMovement>().transform;
        }

        private void Start()
        {
            _enemyGridPosition = GetGridPosition(transform.position);
        }

        private void Update()
        {
            // if (player == null)
            // {
            //     Debug.LogWarning("Player Transform not assigned!");
            //     return;
            // }

            // Track how long the player has been stationary
            // if (!playerMovement.isMoving)
            // {
            //     playerNotMovingTimer += Time.deltaTime;
            // }
            // else
            // {
            //     playerNotMovingTimer = 0f;
            // }
            // Debug.Log("turn to move is : " + turnToMove);
            if (turnToMove == Turn.Player)
            {
                return;
            }

            _playerGridPosition = GetGridPosition(player.position);

            Debug.Log("PlayerPos: " + _playerGridPosition);
            // If the enemy is adjacent to the player, don't move


            Tile tile = GetTileAtPosition(_playerGridPosition);
            Debug.Log("Tile: " + tile);
            if (!isMoving && movementQueue.Count > 0)
            {
                Vector3 nextPosition = movementQueue.Dequeue();
                if (Mathf.Approximately(nextPosition.x, _playerGridPosition.x) && Mathf.Approximately(nextPosition.z, _playerGridPosition.y))
                {
                    enemyObstacleData.SetObstacleAt((int)gameObject.transform.position.x,
                        (int)gameObject.transform.position.z, true);
                    enemyObstacleData.SetObstacleAt(originalPosition.x,
                        originalPosition.y, false);
                    turnToMove = Turn.Player;
                    return;
                }

                StartCoroutine(MoveToTarget(nextPosition, Turn.Player));
            }

            // Perform BFS to find the shortest path to the player
            if (!isMoving)
            {
                BfsToTile(tile);
            }
        }

        private Vector2Int GetGridPosition(Vector3 playerPosition)
        {
            return new Vector2Int((int)playerPosition.x, (int)playerPosition.z);
        }
    }
}
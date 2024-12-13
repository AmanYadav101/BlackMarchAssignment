using Game.Scripts.StateManagers;
using UnityEngine;


namespace Game.Scripts.Enemy
{
    public class EnemyAI : CharacterMovement
    {
        private Transform _player; 

        private Vector2Int _playerGridPosition;
        
        private void Awake()
        {
            _player = GameObject.FindAnyObjectByType<PlayerMovement>().transform;
        }
        

        private void Update()
        {
           
            if (turnToMove == Turn.Player)
            {
                return;
            }

            _playerGridPosition = GetGridPosition(_player.position);


            TileStateManager tileStateManager = GetTileAtPosition(_playerGridPosition);
            Debug.Log("Tile: " + tileStateManager);
            if (!isMoving && movementQueue.Count > 0)
            {
                Vector3 nextPosition = movementQueue.Dequeue();
                if (Mathf.Approximately(nextPosition.x, _playerGridPosition.x) && Mathf.Approximately(nextPosition.z, _playerGridPosition.y))
                {
                    
                    enemyObstacleData.SetSelectedPosition((int)gameObject.transform.position.x,
                        (int)gameObject.transform.position.z);
                    
                    turnToMove = Turn.Player;
                    return;
                }

                StartCoroutine(MoveToTarget(nextPosition, Turn.Player));
            }

            // Perform BFS to find the shortest path to the player
            if (!isMoving)
            {
                BfsToTile(tileStateManager);
            }
        }

        private Vector2Int GetGridPosition(Vector3 playerPosition)
        {
            return new Vector2Int((int)playerPosition.x, (int)playerPosition.z);
        }
    }
}
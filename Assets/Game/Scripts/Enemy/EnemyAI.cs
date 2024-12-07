using UnityEngine;


namespace Game.Scripts.Enemy
{
    public class EnemyAI : CharacterMovement
    {
        private Transform _player; 

        private Vector2Int _enemyGridPosition;
        private Vector2Int _playerGridPosition;
        
        private void Awake()
        {
            _player = GameObject.FindAnyObjectByType<PlayerMovement>().transform;
        }

        private void Start()
        {
            _enemyGridPosition = GetGridPosition(transform.position);
        }

        private void Update()
        {
           
            if (turnToMove == Turn.Player)
            {
                return;
            }

            _playerGridPosition = GetGridPosition(_player.position);


            Tile tile = GetTileAtPosition(_playerGridPosition);
            Debug.Log("Tile: " + tile);
            if (!isMoving && movementQueue.Count > 0)
            {
                Vector3 nextPosition = movementQueue.Dequeue();
                if (Mathf.Approximately(nextPosition.x, _playerGridPosition.x) && Mathf.Approximately(nextPosition.z, _playerGridPosition.y))
                {
                    
                    enemyObstacleData.SetObstacleAt(originalPosition.x,
                        originalPosition.y, false);
                    
                    enemyObstacleData.SetObstacleAt((int)gameObject.transform.position.x,
                        (int)gameObject.transform.position.z, true);
                    
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
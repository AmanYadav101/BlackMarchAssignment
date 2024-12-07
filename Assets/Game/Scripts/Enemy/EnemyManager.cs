using Game.Scripts.Obstacle;
using UnityEngine;

namespace Game.Scripts.Enemy
{
    /// <summary>
    /// Used to Spawn Enemies at the locations mentioned in the "EnemyObstacleData" scriptable object.
    /// The maxNumberOfEnemies to be spawned can be adjusted by the variable "_maxNumberOfEnemies"
    /// Doesn't spawn an Enemy if there is an obstacle present on the corresponding tile.
    /// </summary>
    public class EnemyManager : MonoBehaviour
    {
        public ObstacleData obstacleData;
        public EnemyObstacleData enemyObstacleData;
        public GameObject enemyPrefab;
        public float tileSize = 1.0f;

        private GameObject[,] _enemies = new GameObject[10, 10];

        // Used to keep record of number of enemies that has been spawned and that the enemies to spawn doesn't exist _maxNumberOfEnemies.
        private const int MaxNumberOfEnemies = 1;

        private void Start()
        {
            GenerateEnemies();
        }

        // Used for spawning enemies at the particular position in the grid.
        // Won't Spawn an enemy if there is already an obstacle present on the tile.
        public void GenerateEnemies()
        {
            Vector2Int enemyGridPosition = enemyObstacleData.GetSelectedPosition();

            if (obstacleData.GetObstacleAt(enemyGridPosition.x, enemyGridPosition.y) ||
                enemyGridPosition == Vector2Int.zero)
            {
                Vector3 position = new Vector3((9) * tileSize, tileSize, (9) * tileSize);
                _enemies[9, 9] = Instantiate(enemyPrefab, position, Quaternion.identity, transform);
            }
            else
            {
                Vector3 position = new Vector3((enemyGridPosition.x) * tileSize, tileSize,
                    (enemyGridPosition.y) * tileSize);
                _enemies[enemyGridPosition.x, enemyGridPosition.y] =
                    Instantiate(enemyPrefab, position, Quaternion.identity, transform);
            }
        }
    }
}
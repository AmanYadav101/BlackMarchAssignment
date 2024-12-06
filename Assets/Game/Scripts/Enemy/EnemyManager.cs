using Game.Scripts.Obstacle;
using UnityEngine;

namespace Game.Scripts.Enemy
{
    public class EnemyManager : MonoBehaviour
    {
        public ObstacleData obstacleData;

        public EnemyObstacleData enemyObstacleData; // Reference to the ScriptableObject
        public GameObject enemyPrefab; // Prefab for the obstacle (e.g., a red sphere)
        public float tileSize = 1.0f; // Tile size (same as the grid)

        private GameObject[,] _enemies = new GameObject[10, 10]; // Store obstacle instances

        private int _maxNumberOfEnemies = 1;
        int numberOfEnemies = 0;

        void Start()
        {
            GenerateEnemies();
        }

        public void GenerateEnemies()
        {

            for (int x = 0; x < 10; x++)
            {
                for (int y = 0; y < 10; y++)
                {
                    
                    if (enemyObstacleData.GetEnemiesAt(x, y) &&numberOfEnemies < _maxNumberOfEnemies)
                    {
                        if (obstacleData.GetObstacleAt(x, y))
                        {
                            Debug.LogError("Obstacle already exists");
                            return;
                        }
                        Vector3 position = new Vector3((x) * tileSize, tileSize, (y) * tileSize);

                        _enemies[x, y] = Instantiate(enemyPrefab, position, Quaternion.identity, transform);
                        numberOfEnemies++;
                    }
                }
            }
        }

      
    }
}
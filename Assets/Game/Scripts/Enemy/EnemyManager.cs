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

        private int _maxNumberOfEnemies = 1;
        
        // Used to keep record of number of enemies that has been spawned and that the enemies to spawn doesn't exist _maxNumberOfEnemies.
        int numberOfEnemies = 0; 

        void Start()
        {
            GenerateEnemies();
        }

        // Used for spawning enemies at the particular position in the grid.
        // Won't Spawn an enemy if there is already an obstacle present on the tile.
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
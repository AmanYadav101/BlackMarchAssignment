using Game.Scripts.Obstacle;
using UnityEngine;

namespace Game.Scripts.Managers
{
    public class ObstacleManager : MonoBehaviour
    {
        public ObstacleData obstacleData; // Reference to the ScriptableObject
        public GameObject obstaclePrefab; // Prefab for the obstacle (e.g., a red sphere)
        public float tileSize = 1.0f; // Tile size (same as the grid)

        private GameObject[,] obstacles = new GameObject[10, 10]; // Store obstacle instances

        void Start()
        {
            GenerateObstacles();
        }

        public void GenerateObstacles()
        {

            for (int x = 0; x < 10; x++)
            {
                for (int y = 0; y < 10; y++)
                {
                    if (obstacleData.GetObstacleAt(x, y))
                    {
                        Vector3 position = new Vector3((x) * tileSize, tileSize * 1.5f, (y) * tileSize);

                        obstacles[x, y] = Instantiate(obstaclePrefab, position, Quaternion.identity, transform);
                    }
                }
            }
        }

      
    }
}
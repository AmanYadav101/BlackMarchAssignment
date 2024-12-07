using Game.Scripts.Obstacle;
using UnityEngine;

namespace Game.Scripts.Managers
{
    /// <summary>
    /// Obstacle Manager is used for Generating Obstacles at Positions mentioned in the "Obstacle Data".
    /// </summary>
    public class ObstacleManager : MonoBehaviour
    {
        public ObstacleData obstacleData;
        public GameObject obstaclePrefab;
        public float tileSize = 1.0f;
        private GameObject[,] obstacles = new GameObject[10, 10];

        void Start()
        {
            GenerateObstacles();
        }

        /// <summary>
        /// GenerateObstacles function used for generating obstacles, by looping through the obstacle data and
        /// generating the obstacle at that location in the grid.
        /// The positions of obstacles are determined based on the grid index.
        /// </summary>
        public void GenerateObstacles()
        {
            for (int x = 0; x < 10; x++)
            {
                for (int y = 0; y < 10; y++)
                {
                    if (x == 0 && y == 0 || x == 9 && y == 9)
                    {
                        continue;
                    }
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
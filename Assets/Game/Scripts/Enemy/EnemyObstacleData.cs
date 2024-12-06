using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Scripts.Enemy
{
    [CreateAssetMenu(fileName = "Enemy Obstacle Data", menuName = "Enemy Obstacles/Enemy Obstacle Data")]
    public class EnemyObstacleData : ScriptableObject
    {
        public List<bool> enemyGrid = new List<bool>(100); 
         
        private void OnValidate()
        {
            if (enemyGrid == null || enemyGrid.Count != 100)
            {
                enemyGrid = new List<bool>(new bool[100]);
            }
        }
        public bool GetEnemiesAt(int x, int y)
        {
            return enemyGrid[y * 10 + x]; 
        }

        // Mutator method to set an obstacle at a specific position
        public void SetObstacleAt(int x, int y, bool value)
        {
            enemyGrid[y * 10 + x] = value;
        }
    }
}
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Scripts.Enemy
{
    
    /// <summary>
    /// Implementation is same as Obstacle data Scriptable Object.
    /// Used in EnemyObstacle Editor so that spawn points of the enemies can be selected in the editor.
    /// </summary>
    [CreateAssetMenu(fileName = "Enemy Obstacle Data", menuName = "Enemy Obstacles/Enemy Obstacle Data")]
    public class EnemyObstacleData : ScriptableObject
    {
        public List<bool> enemyGrid = new List<bool>(100); 
        
        public bool GetEnemiesAt(int x, int y)
        {
            return enemyGrid[y * 10 + x]; 
        }

        public void SetObstacleAt(int x, int y, bool value)
        {
            enemyGrid[y * 10 + x] = value;
        }
    }
}
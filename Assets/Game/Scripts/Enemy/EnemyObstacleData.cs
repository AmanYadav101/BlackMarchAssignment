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
        public Vector2Int selectedGridPosition = new Vector2Int(-1, -1);

        public bool GetEnemiesAt(int x, int y)
        {
            return enemyGrid[y * 10 + x];
        }

        public Vector2Int GetSelectedPosition()
        {
            // Return the currently selected grid position, or (-1, -1) if none is selected
            return selectedGridPosition;
        }

        public void SetSelectedPosition(int x, int y)
        {
            ClearAll();
            selectedGridPosition = new Vector2Int(x, y);
            enemyGrid[y * 10 + x] = true;
        }

        public void ClearSelection()
        {
            // Clear the currently selected position
            if (selectedGridPosition.x == -1 || selectedGridPosition.y == -1) return;
            enemyGrid[selectedGridPosition.y * 10 + selectedGridPosition.x] = false;
            selectedGridPosition = new Vector2Int(-1, -1);
        }

        public void ClearAll()
        {
            for (int i = 0; i < 100; i++)
            {
                enemyGrid[i] = false;
            }

            selectedGridPosition = new Vector2Int(-1, -1);
        }
    }
}
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts.Obstacle
 {
     /// <summary>
     /// 
     /// </summary>
     [CreateAssetMenu(fileName = "New Obstacle Data", menuName = "Obstacle System/Obstacle Data")]
     public class ObstacleData : ScriptableObject
     {
         // Used lists instead of a 2D array as the arrays in unity are not serialized. 
         // Which makes any data stored in the scriptable object to not persist.
         public List<bool> obstacleGrid = new List<bool>(100);
         

         // Used for checking if there is any obstacle present on the particular position of the grid.
         // returns true if there is an obstacle present otherwise it returns false.
         public bool GetObstacleAt(int x, int y)
         {
             return obstacleGrid[y * 10 + x]; 
         }

         // This method is used to set an obstacle at a specific position
         public void SetObstacleAt(int x, int y, bool value)
         {
             obstacleGrid[y * 10 + x] = value;
         }
     }
}
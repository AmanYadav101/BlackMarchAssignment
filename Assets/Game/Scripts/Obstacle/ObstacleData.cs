using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts.Obstacle
 {
     [CreateAssetMenu(fileName = "New Obstacle Data", menuName = "Obstacle System/Obstacle Data")]
     public class ObstacleData : ScriptableObject
     {
         public List<bool> obstacleGrid = new List<bool>(100); 
         
         public bool GetObstacleAt(int x, int y)
         {
             return obstacleGrid[y * 10 + x]; 
         }

         // Mutator method to set an obstacle at a specific position
         public void SetObstacleAt(int x, int y, bool value)
         {
             obstacleGrid[y * 10 + x] = value;
         }
     }
}
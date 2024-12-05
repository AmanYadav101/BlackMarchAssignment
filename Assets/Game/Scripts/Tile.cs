using UnityEngine;

namespace Game.Scripts
{
    public class Tile : MonoBehaviour
    {
        public Vector2Int gridPosition;  // The tile's position in the grid
        public bool isObstacle;

        public int visited = -1;
        public int x = 0;
        public int y = 0;
    }
}
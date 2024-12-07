using UnityEngine;

namespace Game.Scripts
{
    /// <summary>
    /// Used to Store data of a tile.
    /// Added on every tile for checking if its position in the grid and if it has an obstacle on it or not.
    /// </summary>
    public class Tile : MonoBehaviour
    {
        public Vector2Int gridPosition;  // The tile's position in the grid
        public bool isObstacle; // If the tile has an obstacle on it or not
    }
}
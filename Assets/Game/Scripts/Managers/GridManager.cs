using Game.Scripts.StateManagers;
using Game.Scripts.Obstacle;
using TMPro;
using UnityEngine;

namespace Game.Scripts.Managers
{
    /// <summary>
    /// GridManager class, used for generating grid of size 10x10 as mentioned in the assignment.
    /// It uses gridRows and gridColumns variables to generate the grid based on the number of rows and columns specified.
    /// </summary>
    public class GridManager : MonoBehaviour
    {
        public GameObject tilePrefab;
        public int gridRows = 10;
        public int gridColumns = 10;
        public float tileSize = 1.0f;
        public Camera mainCamera;
        public TextMeshProUGUI tileInfoText;
        public ObstacleData obstacleData;
        public TileStateManager tileStateManager;


        private TileStateManager _lastHoveredTileState;

        private void Start()
        {
            GenerateGrid();
        }


        /// <summary>
        /// Update function is being used to Hover over the tiles using "Raycast".
        /// If the hovered tile has no obstacles on it then the color of the hovered tile will turn blue,
        /// and If the hovered tile has obstacle present on it, then color of the tile will turn to red on hover.
        /// Hovering over tile also shows the location of the tile and if the Tile has an obstacle on it or not in grid using the "TextMeshPro".
        /// </summary>
        private void Update()
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                tileStateManager = hit.collider.GetComponent<TileStateManager>();
                if (!tileStateManager) return;
                tileStateManager.isMouseOnTile = true;

                if (Input.GetMouseButtonDown(0))
                {
                    Debug.Log("mv 1pressed");
                }
                if (_lastHoveredTileState != tileStateManager)
                {
                    ResetLastHoveredTile();
                    _lastHoveredTileState = tileStateManager;
                }

                // state - obstacle
                // tile.GetComponent<MeshRenderer>().materials[0].color = tile.isObstacle ? Color.red : Color.blue;

                tileInfoText.text = tileStateManager.hasObstacle
                    ? $"Obstacle Tile:\n {tileStateManager.gridPosition}"
                    : $"Tile Position:\n {tileStateManager.gridPosition}";
            }
            else
            {
                tileInfoText.text = "Hover over a tile";
            }
        }
        
        
        /// <summary>
        /// GenerateGrid function used for generating the grid of size "gridRows x gridColumns".
        /// All it does is that it loops through gridRows and gridColumns and Instantiates the "tilePrefab", which is the tile we want to spawn in the grid
        /// Each Tile has a Tile Script attached to it, which is used for determining if the tile is an obstacle Tile or not.
        /// </summary>
        private void GenerateGrid()
        {
            for (var i = 0; i < gridRows; i++)
            {
                for (var j = 0; j < gridColumns; j++)
                {
                    var position = new Vector3(i * tileSize, 0, j * tileSize);
                    var rotation = Quaternion.Euler(-90, 0, 0);
                    var tile = Instantiate(tilePrefab, position, rotation, transform);
                    tile.GetComponent<MeshRenderer>().materials[0].color = Color.gray;
                    tile.name = $"Tile_{i}_{j}";

                    var tileStateManagerScript = tile.GetComponent<TileStateManager>();
                    tileStateManagerScript.gridPosition = new Vector2Int(i, j);


                    tileStateManagerScript.hasObstacle = obstacleData.GetObstacleAt(i, j);
                }
            }
        }


        // Just a function for resetting the color of the tiles back to its original color.
        private void ResetLastHoveredTile()
        {
            if (_lastHoveredTileState)
            {
                _lastHoveredTileState.isMouseOnTile = false;
            }
        }
    }
}
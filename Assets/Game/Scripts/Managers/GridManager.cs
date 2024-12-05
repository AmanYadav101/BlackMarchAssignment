using System;
using Game.Scripts.Obstacle;
using TMPro;
using UnityEngine;

namespace Game.Scripts.Managers
{
    public class GridManager : MonoBehaviour
    {
        public GameObject tilePrefab; 
        public int gridSizeX = 10; 
        public int gridSizeZ = 10;
        public float tileSize = 1.0f; 
        public Camera mainCamera; 
        public TextMeshProUGUI tileInfoText; 

        public ObstacleData obstacleData;

        private Tile _lastHoveredTile; 


        void Start()
        {
            GenerateGrid();
        }

        void Update()
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Tile tile = hit.collider.GetComponent<Tile>();
                if (tile != null)
                {
                    if (_lastHoveredTile != tile)
                    {
                        ResetLastHoveredTile();
                        _lastHoveredTile = tile;
                    }

                    tile.GetComponent<MeshRenderer>().materials[0].color = Color.gray;
                    tileInfoText.text = $"Tile Position: {tile.gridPosition}";
                    
                }
            }
            else
            {
                tileInfoText.text = "Hover over a tile";
            }
        }

        void GenerateGrid()
        {

            for (int x = 0; x <gridSizeX ; x++)
            {
                for (int z = 0; z < gridSizeX; z++)
                {
                    
                    Vector3 position = new Vector3(x * tileSize, 0, z * tileSize);
                    GameObject tile = Instantiate(tilePrefab, position, Quaternion.identity, transform);
                    tile.GetComponent<MeshRenderer>().materials[0].color = Color.white;
                    tile.name = $"Tile_{x}_{z}";

                    Tile tileScript = tile.AddComponent<Tile>();
                    tileScript.gridPosition = new Vector2Int(x, z);

                    if (obstacleData.GetObstacleAt(x , z ))
                    {
                        tileScript.isObstacle = true;
                        tile.GetComponent<MeshRenderer>().materials[0].color = Color.red; 
                    }
                    else
                    {
                        tileScript.isObstacle = false;
                    }
                }
            }
        }


        void ResetLastHoveredTile()
        {
            if (_lastHoveredTile != null)
            {
                _lastHoveredTile.GetComponent<MeshRenderer>().materials[0].color = Color.white;
            }
        }
    }
}
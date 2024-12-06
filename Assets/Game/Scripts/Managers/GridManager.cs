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
        
        public GameObject[,] gridArray;
        public int startX = 0;
        public int startZ = 0;
        public int endX = 2;
        public int endZ = 2;
        
        private Tile _lastHoveredTile; 


        void Start()
        {
            gridArray = new GameObject[gridSizeX, gridSizeZ]; 
            if(tileInfoText != null)
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

                    tile.GetComponent<MeshRenderer>().materials[0].color = tile.isObstacle ? Color.red : Color.gray;

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
                        // tile.GetComponent<MeshRenderer>().materials[0].color = Color.red; 
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

//
//
//
// using System;
// using System.Collections;
// using System.Collections.Generic;
// using Game.Scripts.Obstacle;
// using TMPro;
// using Unity.Mathematics;
// using UnityEngine;
// using UnityEngine.Rendering.Universal;
//
// namespace Game.Scripts.Managers
// {
//     public class GridManager : MonoBehaviour
//     {
//          public bool findDistance = false;
//         public GameObject tilePrefab;
//         public GameObject player; // Reference to the player GameObject
//         public float moveSpeed = 5f; // Speed of player movement
//         public int gridSizeX = 10;
//         public int gridSizeZ = 10;
//         public float tileSize = 1.0f;
//         public Camera mainCamera;
//         public TextMeshProUGUI tileInfoText;
//         public ObstacleData obstacleData;
//
//         public GameObject[,] gridArray;
//         public int startX = 0;
//         public int startZ = 0;
//         public int endX = 2;
//         public int endZ = 2;
//         public List<GameObject> path = new List<GameObject>();
//         private Tile _lastHoveredTile;
//
//         private bool isMoving = false; // Prevents input while moving
//
//         void Start()
//         {
//             gridArray = new GameObject[gridSizeX, gridSizeZ];
//             if (tileInfoText != null)
//                 GenerateGrid();
//
//             // Place the player at the starting position
//             if (player != null)
//             {
//                 player.transform.position = gridArray[startX, startZ].transform.position + Vector3.up * 0.5f;
//             }
//         }
//
//         void Update()
//         {
//             if (findDistance && !isMoving)
//             {
//                 SetDistance();
//                 SetPath();
//                 StartCoroutine(MovePlayerAlongPath());
//                 findDistance = false;
//             }
//
//             Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
//
//             if (Physics.Raycast(ray, out RaycastHit hit))
//             {
//                 Tile tile = hit.collider.GetComponent<Tile>();
//                 if (tile != null)
//                 {
//                     if (_lastHoveredTile != tile)
//                     {
//                         ResetLastHoveredTile();
//                         _lastHoveredTile = tile;
//                     }
//
//                     tile.GetComponent<MeshRenderer>().materials[0].color = Color.gray;
//                     tileInfoText.text = $"Tile Position: {tile.gridPosition}";
//                 }
//             }
//             else
//             {
//                 tileInfoText.text = "Hover over a tile";
//             }
//         }
//
//         IEnumerator MovePlayerAlongPath()
//         {
//             isMoving = true;
//
//             foreach (GameObject tile in path)
//             {
//                 Vector3 targetPosition = tile.transform.position + Vector3.up * 0.5f;
//                 while (Vector3.Distance(player.transform.position, targetPosition) > 0.01f)
//                 {
//                     player.transform.position = Vector3.MoveTowards(
//                         player.transform.position,
//                         targetPosition,
//                         moveSpeed * Time.deltaTime
//                     );
//                     yield return null;
//                 }
//                 player.transform.position = targetPosition;
//                 yield return new WaitForSeconds(0.1f); // Optional delay between steps
//             }
//
//             isMoving = false;
//         }
//
//
//         void GenerateGrid()
//         {
//             for (int x = 0; x < gridSizeX; x++)
//             {
//                 for (int z = 0; z < gridSizeX; z++)
//                 {
//                     Vector3 position = new Vector3(x * tileSize, 0, z * tileSize);
//                     GameObject tile = Instantiate(tilePrefab, position, Quaternion.identity, transform);
//                     tile.GetComponent<MeshRenderer>().materials[0].color = Color.white;
//                     tile.name = $"Tile_{x}_{z}";
//                     gridArray[x, z] = tile;
//                     Tile tileScript = tile.AddComponent<Tile>();
//                     tileScript.gridPosition = new Vector2Int(x, z);
//
//                     if (obstacleData.GetObstacleAt(x, z))
//                     {
//                         tileScript.isObstacle = true;
//                         tile.GetComponent<MeshRenderer>().materials[0].color = Color.red;
//                     }
//                     else
//                     {
//                         tileScript.isObstacle = false;
//                     }
//                 }
//             }
//         }
//
//
//         void SetDistance()
//         {
//             InitialSetup();
//             int x = startX;
//             int z = startZ;
//             int[] testArray = new int[gridSizeX * gridSizeZ];
//             for (int step = 0; step < gridSizeX * gridSizeZ; step++)
//             {
//                 foreach (GameObject obj in gridArray)
//                 {
//                     if (obj && obj.GetComponent<Tile>().visited == step - 1)
//                     {
//                         TestFourDirections(obj.GetComponent<Tile>().x, obj.GetComponent<Tile>().z, step);
//                     }
//                 }
//             }
//         }
//
//         void SetPath()
//         {
//             int step;
//             int x = endX;
//             int z = endZ;
//             List<GameObject> tempList = new List<GameObject>();
//             path.Clear();
//             if (gridArray[endX, endZ] && gridArray[endX, endZ].GetComponent<Tile>().visited > 0)
//             {
//                 path.Add(gridArray[x, z]);
//                 step = gridArray[x, z].GetComponent<Tile>().visited - 1;
//             }
//             else
//             {
//                 Debug.Log("No path found");
//                 return;
//             }
//
//             for (int i = step; step > -1; step--)
//             {
//                 if (TestDirection(x, z, step, 1))
//                 {
//                     tempList.Add(gridArray[x, z + 1]);
//                 }
//
//                 if (TestDirection(x, z, step, 2))
//                 {
//                     tempList.Add(gridArray[x + 1, z]);
//                 }
//
//                 if (TestDirection(x, z, step, 3))
//                 {
//                     tempList.Add(gridArray[x, z - 1]);
//                 }
//
//                 if (TestDirection(x, z, step, 4))
//                 {
//                     tempList.Add(gridArray[x - 1, z]);
//                 }
//
//                 GameObject tempObj = FindClosest(gridArray[endX, endZ].transform, tempList);
//                 path.Add(tempObj);
//                 x = tempObj.GetComponent<Tile>().x;
//                 z = tempObj.GetComponent<Tile>().z;
//                 tempList.Clear();
//             }
//         }
//
//         void InitialSetup()
//         {
//             foreach (GameObject obj in gridArray)
//             {
//                 obj.GetComponent<Tile>().visited = -1;
//             }
//
//             gridArray[startX, startZ].GetComponent<Tile>().visited = 0;
//         }
//
//         bool TestDirection(int x, int z, int step, int direction)
//         {
//             switch (direction)
//             {
//                 case 1:
//                     if (z + 1 < gridSizeZ && gridArray[x, z + 1] &&
//                         gridArray[x, z + 1].GetComponent<Tile>().visited == step)
//                         return true;
//                     else
//                         return false;
//                 case 2:
//                     if (x + 1 < gridSizeX && gridArray[x + 1, z] &&
//                         gridArray[x + 1, z].GetComponent<Tile>().visited == step)
//                         return true;
//                     else
//                         return false;
//                 case 3:
//                     if (z - 1 > -1 && gridArray[x, z - 1] && gridArray[x, z - 1].GetComponent<Tile>().visited == step)
//                         return true;
//                     else
//                         return false;
//                 case 4:
//                     if (x - 1 > -1 && gridArray[x + 1, z] && gridArray[x + 1, z].GetComponent<Tile>().visited == step)
//                         return true;
//                     else
//                         return false;
//             }
//
//             return false;
//         }
//
//         void SetVisited(int x, int z, int step)
//         {
//             if (gridArray[x, z])
//             {
//                 gridArray[x, z].GetComponent<Tile>().visited = step;
//             }
//         }
//
//         void TestFourDirections(int x, int z, int step)
//         {
//             if (TestDirection(x, z, -1, 1))
//             {
//                 SetVisited(x, z + 1, step);
//             }
//             if (TestDirection(x, z, -1, 2))
//             {
//                 SetVisited(x + 1, z, step);
//             }
//
//             if (TestDirection(x, z, -1, 3))
//             {
//                 SetVisited(x, z - 1, step);
//             }
//
//             if (TestDirection(x, z, -1, 4))
//             {
//                 SetVisited(x - 1, z, step);
//             }
//         }
//
//
//         void ResetLastHoveredTile()
//         {
//             if (_lastHoveredTile != null)
//             {
//                 _lastHoveredTile.GetComponent<MeshRenderer>().materials[0].color = Color.white;
//             }
//         }
//
//         GameObject FindClosest(Transform targetLocation, List<GameObject> list)
//         {
//             float currentDistance = tileSize * gridSizeX * gridSizeZ;
//
//             int indexNumber = 0;
//             for (int i = 0; i < list.Count; i++)
//             {
//                 if (Vector3.Distance(targetLocation.position, list[i].transform.position) < currentDistance)
//                 {
//                     currentDistance = Vector3.Distance(targetLocation.position, list[i].transform.position);
//                     indexNumber = i;
//                 }
//             }
//
//             return list[indexNumber];
//         }
//     }
// }
//

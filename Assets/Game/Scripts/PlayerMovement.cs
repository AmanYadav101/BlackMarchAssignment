using System;
using Game.Scripts.Obstacle;
using Unity.VisualScripting;
using UnityEngine;

namespace Game.Scripts
{
    public class PlayerMovement : MonoBehaviour
    {
        public GameObject playerUnit;  // The player unit (Cube or prefab)
        public float moveSpeed = 2f;   // Movement speed of the player
        public ObstacleData obstacleData;  // Reference to the obstacle data (from Assignment 2)

        private bool isMoving = false;  
     private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) && !isMoving)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    Tile tile = hit.collider.GetComponent<Tile>();
                    if (tile != null)
                    {
                        if (!tile.isObstacle)
                        {
                            MovePlayerToTile(tile);
                        }
                        else
                        {
                            Debug.Log("Tile is obstacle");
                        }
                    }
                }
            }
        }

        void MovePlayerToTile(Tile tile)
        {
            isMoving = true;
            Vector3 targetPosition = new Vector3(tile.gridPosition.x, playerUnit.transform.position.y, tile.gridPosition.y);
            StartCoroutine(MoveToTarget(targetPosition));
        }

        System.Collections.IEnumerator MoveToTarget(Vector3 targetPosition)
        {
            while (Vector3.Distance(playerUnit.transform.position, targetPosition) > 0.1f)
            {
                playerUnit.transform.position = Vector3.MoveTowards(playerUnit.transform.position, targetPosition, moveSpeed * Time.deltaTime);
                yield return null;
            }

            playerUnit.transform.position = targetPosition;  // Snap to target position
            isMoving = false;  // Movement complete
        }
    }
}

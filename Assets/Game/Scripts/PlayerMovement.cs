using System;
using Game.Scripts.Managers;
using UnityEngine;

namespace Game.Scripts
{
    public class PlayerMovement : CharacterMovement
    {
        private Vector2 targetPosition = new Vector2(-1, -1);

        public bool playerMovedHisTurn = false;

        UiManager uiManager;

        private void Start()
        {
            uiManager = GameObject.FindObjectOfType<UiManager>();
        }

        private void Update()
        {
            if (turnToMove == Turn.Enemy)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    StartCoroutine(uiManager.WaitForTurn());
                }

                return;
            }


            if (!isMoving && movementQueue.Count > 0)
            {
                Vector3 nextPosition = movementQueue.Dequeue();
                StartCoroutine(MoveToTarget(nextPosition, Turn.Enemy));
            }

            if (Input.GetKeyDown(KeyCode.Mouse0) && !isMoving && turnToMove == Turn.Player)
            {
                StartCoroutine(uiManager.Moving());
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    Tile tile = hit.collider.GetComponent<Tile>();
                    targetPosition.x = tile.transform.position.x;
                    targetPosition.y = tile.transform.position.z;

                    if (tile != null)
                    {
                        if (!tile.isObstacle)
                        {
                            BfsToTile(tile);
                        }
                        else
                        {
                            Debug.Log("Tile is an obstacle");
                        }
                    }
                }

                playerMovedHisTurn = true;
            }

            if (Input.GetKeyDown(KeyCode.Mouse0) && isMoving && turnToMove == Turn.Player)
            {
                StartCoroutine(uiManager.Moving());
            }
        }
    }
}
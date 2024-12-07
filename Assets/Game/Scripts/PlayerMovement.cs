using System;
using Game.Scripts.Managers;
using UnityEngine;

namespace Game.Scripts
{
    public class PlayerMovement : CharacterMovement
    {
        private UiManager uiManager;

        private void Start()
        {
            uiManager = FindObjectOfType<UiManager>();
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
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    Tile tile = hit.collider.GetComponent<Tile>();

                    if (tile != null)
                    {
                        if (!tile.isObstacle)
                        {
                            StartCoroutine(uiManager.Moving());
                            if (!BfsToTile(tile))
                            {
                                StartCoroutine(uiManager.NoPathFound());
                            }
                        }
                        else
                        {
                            StartCoroutine(uiManager.ObstacleTile());

                            Debug.Log("Tile is an obstacle");
                        }
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.Mouse0) && isMoving && turnToMove == Turn.Player)
            {
                StartCoroutine(uiManager.Moving());
            }
        }
    }
}
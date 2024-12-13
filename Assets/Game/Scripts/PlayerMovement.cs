using Game.Scripts.Managers;
using Game.Scripts.StateManagers;
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
                uiManager.HideUIOnEndTurn();

                if (Input.GetMouseButtonDown(0))
                {
                    StartCoroutine(uiManager.WaitForTurn());
                }

                return;
            }

            if (turnToMove == Turn.Player)
            {
                uiManager.ShowEndTurnUI();
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
                    TileStateManager tileStateManager = hit.collider.GetComponent<TileStateManager>();

                    if (tileStateManager != null)
                    {
                        if (!tileStateManager.hasObstacle)
                        {
                            StartCoroutine(uiManager.Moving());
                            if (!BfsToTile(tileStateManager))
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
                uiManager.HideUIOnEndTurn();
                StartCoroutine(uiManager.Moving());
            }
        }

        public void TestMethod()
        {
            turnToMove = Turn.Enemy;
        }
    }
}
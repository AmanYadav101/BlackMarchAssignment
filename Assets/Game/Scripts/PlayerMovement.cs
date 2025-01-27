using System;
using System.Collections;
using Game.Scripts.FSM;
using Game.Scripts.FSM.PlayerState;
using Game.Scripts.Managers;
using Game.Scripts.StateManagers;
using Unity.VisualScripting;
using UnityEngine;

namespace Game.Scripts
{
    public class PlayerMovement : CharacterMovement
    {
        private UiManager uiManager;

        public static event Action OnMouseClicked;
        PlayerBaseState _currentPlayerState;
        [HideInInspector] public PlayerIdleState _playerIdleState;
        [HideInInspector] public PlayerWalkingState _playerWalkingState;

        [HideInInspector] public Vector3 nPosition;

        private void OnEnable()
        {
            OnMouseClicked += MouseClicked;
        }

        private void OnDisable()
        {
            OnMouseClicked -= MouseClicked;
        }

        private void Awake()
        {
            _playerIdleState = new PlayerIdleState(this);
            _playerWalkingState = new PlayerWalkingState(this);
        }

        private void Start()
        {
            ChangeState(_playerIdleState);
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
                if (!isMoving && movementQueue.Count > 0)
                {
                    nPosition = movementQueue.Dequeue();
                    ChangeState(_playerWalkingState);
                }
                else if (movementQueue.Count == 0)
                {
                    if (_currentPlayerState != _playerIdleState)
                    {
                        ChangeState(_playerIdleState);
                    }
                }
            }
            _currentPlayerState?.OnUpdateState();


            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                OnMouseClicked?.Invoke();
            }
        }

        public void TestMethod()
        {
            turnToMove = Turn.Enemy;
        }

        public void StartMovementToPosition(Vector3 targetPosition, float speed)
        {
            if (!isMoving)
            {
                StartCoroutine(MoveToTarget(targetPosition));
            }
        }

        private void MouseClicked()
        {
            switch (isMoving)
            {
                case false when turnToMove == Turn.Player:
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit))
                    {
                        TileStateManager tileStateManager = hit.collider.GetComponent<TileStateManager>();

                        if (tileStateManager)
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

                    break;
                }
                case true when turnToMove == Turn.Player:
                    uiManager.HideUIOnEndTurn();
                    StartCoroutine(uiManager.Moving());
                    break;
            }
        }


        public void ChangeState(PlayerBaseState newState)
        {
            _currentPlayerState?.OnExitState();
            _currentPlayerState = newState;
            _currentPlayerState?.OnEnterState();
        }
    }
}
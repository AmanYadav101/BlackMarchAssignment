using System;
using System.Collections;
using System.Collections.Generic;
using Game.Scripts.Enemy;
using Game.Scripts.Obstacle;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Scripts
{
    public class PlayerMovement : CharacterMovement
    {
        // public GameObject playerUnit; // The player unit (Cube or prefab)
        // public float moveSpeed = 2f; // Movement speed of the player
        public Animator animator;
        private Vector2 targetPosition = new Vector2(-1,-1);

        public bool playerMovedHisTurn = false;
        // public bool isMoving = false;
        // private Queue<Vector3> movementQueue = new Queue<Vector3>();

        private void Update()
        {
            if (turnToMove == Turn.Enemy)
            {
                return;
            }
            if (isMoving)
            {
                animator.SetBool("IsMoving", true);
            }
            else
            {
                animator.SetBool("IsMoving", false);
            }

            if (!isMoving && movementQueue.Count > 0)
            {
                Vector3 nextPosition = movementQueue.Dequeue();
                Debug.Log(transform.position.x + " ---------------------- z-" + transform.position.z);
                StartCoroutine(MoveToTarget(nextPosition,Turn.Enemy));
                
            }

            if (Input.GetKeyDown(KeyCode.Mouse0) && !isMoving && turnToMove == Turn.Player)
            {
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
        }
    }
}
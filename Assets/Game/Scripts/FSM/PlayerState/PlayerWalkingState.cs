using System;
using System.Collections;
using Game.Scripts.StateManagers;
using UnityEngine;

namespace Game.Scripts.FSM.PlayerState
{
    public class PlayerWalkingState : PlayerBaseState
    {
        const float walkingSpeed = 3f;

        public PlayerWalkingState(PlayerMovement playerMovement)
        {
            this.playerMovement = playerMovement;
        }

        public override void OnEnterState()
        {
            Debug.Log("OnEnter() walking ");
        }
public override void OnUpdateState()
        {
            Debug.Log("OnUpdate walking");
            
            playerMovement.StartMovementToPosition(playerMovement.nPosition,walkingSpeed);
        }
        public override void OnExitState()
        {
            Debug.Log("OnExit walking");
        }

        

        public override void OnPlayerInput()
        {
        }

        protected void MoveToPosition(Vector3 targetPosition)
        {
            playerMovement.isMoving = true;
            if (playerMovement.transform.position.x < targetPosition.x)
            {
                playerMovement.transform.rotation = Quaternion.Euler(0, 90, 0);
            }
            else if (playerMovement.transform.position.x > targetPosition.x)
            {
                playerMovement.transform.rotation = Quaternion.Euler(0, -90, 0);
            }
            else if (playerMovement.transform.position.z < targetPosition.z)
            {
                playerMovement.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else if (playerMovement.transform.position.z > targetPosition.z)
            {
                playerMovement.transform.rotation = Quaternion.Euler(0, -180, 0);
            }

            while (Vector3.Distance(playerMovement.transform.position, targetPosition) > 0.1f)
            {
                playerMovement.transform.position = Vector3.MoveTowards(playerMovement.transform.position, targetPosition,
                    walkingSpeed * Time.deltaTime);
            }
            playerMovement.isMoving = false;

            playerMovement.transform.position = targetPosition; // Snap to target position
           
        }
    }
}
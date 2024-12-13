using System.Collections;
using Game.Scripts.StateManagers;
using UnityEngine;

namespace Game.Scripts.FSM.PlayerState
{
    public class PlayerWalkingState: PlayerBaseState
    {
        const int walkingSpeed = 3;
        public override void OnEnterState(PlayerStateManager playerState)
        {
            Debug.Log("OnEnter() walking ");
            throw new System.NotImplementedException();
        }

        public override void OnExitState(PlayerStateManager playerState)
        {
            Debug.Log("OnExit walking");
            throw new System.NotImplementedException();
        }

        public override void OnUpdateState(PlayerStateManager playerState)
        {
            Debug.Log("OnUpdate walking");
            throw new System.NotImplementedException();
        }

        public override void OnPlayerInput(PlayerStateManager playerState)
        {
            throw new System.NotImplementedException();
        }
        
        protected IEnumerator MoveToPosition(Vector3 targetPosition, PlayerStateManager playerState)
        {
            if (playerState.transform.position.x < targetPosition.x)
            {
                playerState.transform.transform.rotation = Quaternion.Euler(0, 90, 0);
            }
            else if (playerState.transform.position.x > targetPosition.x)
            {
                playerState.transform.transform.rotation = Quaternion.Euler(0, -90, 0);
            }
            else if (playerState.transform.position.z < targetPosition.z)
            {
                playerState.transform.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else if (playerState.transform.position.z > targetPosition.z)
            {
                playerState.transform.transform.rotation = Quaternion.Euler(0, -180, 0);
            }

            while (Vector3.Distance(playerState.transform.position, targetPosition) > 0.1f)
            {
                playerState.transform.position = Vector3.MoveTowards(playerState.transform.position, targetPosition,
                    walkingSpeed * Time.deltaTime);
                yield return null;
            }
            playerState.transform.position = targetPosition; // Snap to target position

        }
    }
}
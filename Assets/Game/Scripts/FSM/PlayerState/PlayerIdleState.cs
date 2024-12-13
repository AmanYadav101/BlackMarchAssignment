using Game.Scripts.StateManagers;
using UnityEngine;

namespace Game.Scripts.FSM.PlayerState
{
    public class PlayerIdleState: PlayerBaseState
    {
        public override void OnEnterState(PlayerStateManager playerState)
        {
            Debug.Log("idle enter");
            playerState.animator.Play("Idle");
            throw new System.NotImplementedException();
        }

        public override void OnExitState(PlayerStateManager playerState)
        {
            Debug.Log("idle exit enter");

            throw new System.NotImplementedException();
        }

        public override void OnUpdateState(PlayerStateManager playerState)
        {
            throw new System.NotImplementedException();
        }

        public override void OnPlayerInput(PlayerStateManager playerState)
        {
            throw new System.NotImplementedException();
        }
    }
}
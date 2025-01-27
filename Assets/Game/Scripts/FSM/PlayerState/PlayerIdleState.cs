using Game.Scripts.StateManagers;
using UnityEngine;

namespace Game.Scripts.FSM.PlayerState
{
    public class PlayerIdleState: PlayerBaseState
    {
        public PlayerIdleState(PlayerMovement playerMovement)
        {
            this.playerMovement = playerMovement;
        }
        public override void OnEnterState()
        {
            Debug.Log("idle enter");
            //playerMovement.animator.Play("Idle");
        }

        public override void OnExitState()
        {
            Debug.Log("idle exit");

        }

        public override void OnUpdateState()
        {
            Debug.Log("idle update");
        }

        public override void OnPlayerInput()
        {
        }
    }
}
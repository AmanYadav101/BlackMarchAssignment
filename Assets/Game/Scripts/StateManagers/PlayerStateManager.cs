using Game.Scripts.FSM;
using Game.Scripts.FSM.PlayerState;
using UnityEngine;

namespace Game.Scripts.StateManagers
{
    public class PlayerStateManager: MonoBehaviour
    {
        PlayerBaseState currentPlayerState;
        private PlayerIdleState playerIdleState = new PlayerIdleState();
        private PlayerWalkingState playerWalkingState = new PlayerWalkingState();
        
        public Vector2Int currentPlayerPosition;
        public Vector2Int targetPosition;
        
        public Animator animator;
        
        private void Start()
        {
            currentPlayerState = playerIdleState;
            currentPlayerState.OnEnterState(this);
        }
        
        
        
        
        private void Update()
        {
            currentPlayerState.OnUpdateState(this);
        }

        private void ChangeState(PlayerBaseState newState)
        {
            currentPlayerState?.OnExitState(this);
            currentPlayerState = newState;
            currentPlayerState?.OnEnterState(this);
        }
        
         
    }
}
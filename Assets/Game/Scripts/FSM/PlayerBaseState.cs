using Game.Scripts.StateManagers;

namespace Game.Scripts.FSM
{
    public abstract class PlayerBaseState
    {
        public abstract void OnEnterState(PlayerStateManager playerState);
        public abstract void OnExitState(PlayerStateManager playerState);
        public abstract void OnUpdateState(PlayerStateManager playerState);
        public abstract void OnPlayerInput(PlayerStateManager playerState);
        
    }
}
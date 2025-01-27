using Game.Scripts.StateManagers;

namespace Game.Scripts.FSM
{
    public abstract class PlayerBaseState
    {
        protected PlayerMovement playerMovement;
        
        public abstract void OnEnterState();
        public abstract void OnExitState();
        public abstract void OnUpdateState();
        public abstract void OnPlayerInput();
        
    }
}
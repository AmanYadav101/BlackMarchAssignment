using Game.Scripts.StateManagers;

namespace Game.Scripts.FSM
{
    public abstract class TileBaseState
    {
        public abstract void OnEnterState(TileStateManager tileState);
        public abstract void OnExitState(TileStateManager tileState);
        public abstract void OnUpdateState(TileStateManager tileState);
    }
}
using Game.Scripts.StateManagers;
using UnityEngine;

namespace Game.Scripts.FSM.TileState
{
    public class TileIdleState: TileBaseState
    {
        public override void OnEnterState(TileStateManager tileState)
        {
            tileState.meshRenderer.material.color = Color.gray;
        }

        public override void OnExitState(TileStateManager tileState)
        {
        }

        public override void OnUpdateState(TileStateManager tileState)
        {
            if (tileState.isMouseOnTile)
            {
                tileState.ChangeTileState(tileState.tileHoverState);
                return;
            }
            tileState.meshRenderer.material.color = Color.gray;
        }
    }
}
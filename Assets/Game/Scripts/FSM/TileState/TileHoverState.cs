using Game.Scripts.StateManagers;
using UnityEngine;

namespace Game.Scripts.FSM.TileState
{
    public class TileHoverState: TileBaseState
    {
        public override void OnEnterState(TileStateManager tileState)
        {
        }

        public override void OnExitState(TileStateManager tileState)
        {
        }

        public override void OnUpdateState(TileStateManager tileState)
        {
            if (!tileState.isMouseOnTile)
            {
                tileState.ChangeTileState(tileState.tileIdleState);
                return;
            }
            if (tileState.hasObstacle)
            {
                tileState.ChangeTileState(tileState.tileObstacleState);
                return;
            }
            tileState.meshRenderer.material.color = Color.blue;
        }
    }
}
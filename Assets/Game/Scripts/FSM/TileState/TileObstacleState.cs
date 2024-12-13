using Game.Scripts.StateManagers;
using UnityEngine;

namespace Game.Scripts.FSM.TileState
{
    public class TileObstacleState : TileBaseState
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
            tileState.meshRenderer.material.color = Color.red;
        }
    }
}
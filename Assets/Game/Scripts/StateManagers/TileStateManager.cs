using System;
using Game.Scripts.FSM;
using Game.Scripts.FSM.TileState;
using UnityEngine;

namespace Game.Scripts.StateManagers
{
    public class TileStateManager: MonoBehaviour
    {
        private TileBaseState _tileState;
        
        public TileIdleState tileIdleState = new TileIdleState();
        public TileHoverState tileHoverState = new TileHoverState();
        public TileObstacleState tileObstacleState = new TileObstacleState();

        public Vector2Int gridPosition;  // The tile's position in the grid
        public bool hasObstacle; // If the tile has an obstacle on it or not
        public bool isMouseOnTile;
        public MeshRenderer meshRenderer;
        private void Start()
        {
            _tileState = tileIdleState;
            meshRenderer = gameObject.GetComponent<MeshRenderer>();
            _tileState.OnEnterState(this);
        }

        private void Update()
        {
            _tileState.OnUpdateState(this);
        }

        public void ChangeTileState(TileBaseState newState)
        {
            _tileState?.OnExitState(this);
            _tileState = newState;
            _tileState?.OnEnterState(this);
        }
    }
}
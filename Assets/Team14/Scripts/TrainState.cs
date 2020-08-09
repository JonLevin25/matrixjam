﻿using System;
using UnityEngine;

namespace MatrixJam.Team14
{
    [Serializable]
    public abstract class TrainState
    {
        // For debug
        public abstract string Name { get; }
        public abstract string AnimTrigger { get; }
        public abstract TrainMove? Move { get; }

        public virtual void OnEnter()
        {
            if (Move != null)
                TrainController.Instance.PlaySFX(Move.Value);
        }

        public virtual void OnExit()
        {
        }

        public virtual void OnUpdate()
        {
        }

        public override string ToString() => $"[TrainState] {Name}";

        protected bool HandleJump() => HandleMoveTransition(TrainMove.Jump, TrainController.JumpState);
        protected bool HandleDuck() => HandleMoveTransition(TrainMove.Duck, TrainController.DuckState);
        protected bool HandleDuckHold() => HandleMoveHold(TrainMove.Duck, TrainController.DriveState);
        protected bool HandleHonk() => HandleMoveTransition(TrainMove.Honk, TrainController.HonkState);
        
        // protected bool HandleHonk()
        // {
        //     var honk = TrainMoves.GetKeyDown(TrainMove.Honk);
        //     if (!honk) return false;
        //     
        //     TrainController.Instance.HonkAnim();
        //     var obstacle = Obstacle.HandleMovePressed(TrainMove.Honk);
        //     return obstacle != null;
        // }


        private bool HandleMoveTransition(TrainMove move, TrainState state)
        {
            var playerPressed = TrainMoves.GetKeyDown(move);
            if (playerPressed)
                TransitionWithMove(move, state);

            return playerPressed;
        }

        private bool HandleMoveHold(TrainMove move, TrainState stateOnRelease)
        {
            var playerReleased = TrainMoves.GetKeyRelease(move);
                if (playerReleased)
                TrainController.TransitionState(stateOnRelease, null);
            
            return playerReleased;
        }

        private void TransitionWithMove(TrainMove move, TrainState state)
        {
            var obstacle = Obstacle.HandleMovePressed(move);
            TrainController.TransitionState(state, obstacle ? obstacle.MoveCue : null);
        }
    }

    public abstract class AutoExitTrainState : TrainState
    {
        private float _timeToExit;
        private float _timeSinceEnter;
        
        public TrainState AutoExitState { private get; set; }
        
        public AutoExitTrainState(float timeToExit, TrainState autoExitState)
        {
            AutoExitState = autoExitState;
            _timeToExit = timeToExit;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _timeSinceEnter = 0f;
        }

        public override void OnUpdate()
        {
            _timeSinceEnter += Time.deltaTime;
            
            if (_timeSinceEnter >= _timeToExit)
                TrainController.TransitionState(TrainController.DriveState, null);
        }
    }

    public class TrainDriveState : TrainState
    {
        public override string Name => "Drive";
        public override string AnimTrigger => "Idle";
        public override TrainMove? Move => null;

        public override void OnUpdate()
        {
            base.OnUpdate();
            
            HandleHonk();
            if (HandleDuck()) return;
            if (HandleJump()) return;
        }
    }
    
    public class TrainHonkState : AutoExitTrainState
    {
        public override string Name => "Honk";
        public override string AnimTrigger => "Honk";
        public override TrainMove? Move => TrainMove.Honk;

        public TrainHonkState(float timeToExit, TrainState autoExitState) : base(timeToExit, autoExitState)
        {
        }
        
        public override void OnUpdate()
        {
            base.OnUpdate();
            // Don't allow transition to jump/honk during    
            // if (HandleJump()) return;
            // if (HandleDuck()) return;
        }
    }

    public class TrainJumpState : AutoExitTrainState
    {
        public override string Name => "Jump";
        public override string AnimTrigger => "Jump";
        public override TrainMove? Move => TrainMove.Jump;
        
        public TrainJumpState(float timeToExit, TrainState autoExitState) : base(timeToExit, autoExitState)
        {
        }
        public override void OnUpdate()
        {
            base.OnUpdate();
            
            // if (HandleHonk()) return;
            if (HandleDuck()) return;
        }
    }

    public class TrainDuckState : TrainState
    {
        public override string Name => "Duck";
        public override string AnimTrigger => "Duck";
        public override TrainMove? Move => TrainMove.Duck;

        public override void OnUpdate()
        {
            Debug.Log("DuckState 1");
            base.OnUpdate();
            Debug.Log("DuckState 2");
            if (HandleJump()) return;
            Debug.Log("DuckState 3");
            if (HandleHonk()) return;
            Debug.Log("DuckState 4");
            HandleDuckHold();
        }
    }

    public class TrainNullState : TrainState
    {
        public override string Name => "NONE";
        public override string AnimTrigger => null;
        public override TrainMove? Move => null;
    }
}

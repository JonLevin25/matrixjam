using System;
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
        public abstract bool AllowSelfTransition { get; }
        public abstract bool PlaySFXOnEnter { get; }

        public virtual void OnEnter()
        {
            if (PlaySFXOnEnter && Move != null)
                TrainController.Instance.PlaySFX(Move.Value);
        }

        public virtual void OnExit()
        {
        }

        public virtual void OnUpdate()
        {
        }

        public override string ToString() => $"[TrainState] {Name}";

        protected bool HandleJump() => HandleMoveTransition(TrainMove.Jump, TrainController.JumpState, true);
        protected bool HandleDuck() => HandleMoveTransition(TrainMove.Duck, TrainController.DuckState, true);
        protected bool HandleDuckHold() => HandleMoveHold(TrainMove.Duck, TrainController.DriveState);
        protected bool HandleHonk() => HandleMoveTransition(TrainMove.Honk, TrainController.HonkState, true);
        
        // protected bool HandleHonk()
        // {
        //     var honk = TrainMoves.GetKeyDown(TrainMove.Honk);
        //     if (!honk) return false;
        //     
        //     TrainController.Instance.HonkAnim();
        //     var obstacle = Obstacle.HandleMovePressed(TrainMove.Honk);
        //     return obstacle != null;
        // }


        private bool HandleMoveTransition(TrainMove move, TrainState state, bool immediate = false)
        {
            var playerPressed = TrainMoves.GetKeyDown(move);
            if (playerPressed)
                TransitionWithMove(move, state, immediate);

            return playerPressed;
        }

        private bool HandleMoveHold(TrainMove move, TrainState stateOnRelease)
        {
            var playerReleased = TrainMoves.GetKeyRelease(move);
            if (playerReleased)
                TrainController.TransitionState(stateOnRelease, null);
            
            return playerReleased;
        }

        private void TransitionWithMove(TrainMove move, TrainState state, bool immediate = false)
        {
            var obstacle = Obstacle.HandleMovePressed(move);

            var moveCue = GetMoveCue(obstacle, immediate); 
            TrainController.TransitionState(state, moveCue);
        }

        private static Transform GetMoveCue(Obstacle obstacle, bool transitionImmediate)
        {
            if (transitionImmediate) return null;
            if (!obstacle) return null;
            return obstacle.MoveCue;
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
        public override bool PlaySFXOnEnter => true;
        public override string Name => "Drive";
        public override string AnimTrigger => "Idle";
        public override TrainMove? Move => null;
        public override bool AllowSelfTransition => false;

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
        public override bool PlaySFXOnEnter => true;
        public override string Name => "Honk";
        public override string AnimTrigger => "Honk";
        public override TrainMove? Move => TrainMove.Honk;
        public override bool AllowSelfTransition => true;

        public TrainHonkState(float timeToExit, TrainState autoExitState) : base(timeToExit, autoExitState)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
            TrainController.Instance.Thomas.WeirdAnim(3f);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            if (HandleHonk()) return;
            // Don't allow transition to jump/honk during    
            // if (HandleJump()) return;
            // if (HandleDuck()) return;
        }
    }

    public class TrainJumpState : AutoExitTrainState
    {
        public override bool PlaySFXOnEnter => true;
        public override string Name => "Jump";
        public override string AnimTrigger => "Jump";
        public override TrainMove? Move => TrainMove.Jump;
        public override bool AllowSelfTransition => false;
        
        public TrainJumpState(float timeToExit, TrainState autoExitState) : base(timeToExit, autoExitState)
        {
        }
        
        public override void OnEnter()
        {
            base.OnEnter();
            TrainController.Instance.Thomas.HappyAnim(1.8f);
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
        public override bool PlaySFXOnEnter => true;
        public override string Name => "Duck";
        public override string AnimTrigger => "Duck";
        public override TrainMove? Move => TrainMove.Duck;
        public override bool AllowSelfTransition => false;

        public override void OnUpdate()
        {
            base.OnUpdate();
            if (HandleJump()) return;
            if (HandleHonk()) return;
            HandleDuckHold();
        }
    }

    public class TrainNullState : TrainState
    {
        public override bool PlaySFXOnEnter => false;
        public override string Name => "NONE";
        public override string AnimTrigger => null;
        public override TrainMove? Move => null;
        public override bool AllowSelfTransition => false;
    }
}

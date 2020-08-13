using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team11
{
    [RequireComponent(typeof(Exit))]
    public class MatrixExitScene : GameScene
    {
        private Exit _exit;

        private void Awake()
        {
            this._exit = this.GetComponent<Exit>();
        }

        public override void Enter()
        {
            base.Enter();
            this._exit.EndLevel();
        }
    }
}

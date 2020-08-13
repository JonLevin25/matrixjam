using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MatrixJam.Team11
{
    public class GameScene : MonoBehaviour
    {
        [SerializeField] protected float _enterDelayTime = 1f;
        public float EnterDelayTime { get { return this._enterDelayTime; } }

        public virtual void Enter()
        {
            this.gameObject.SetActive(true);
        }

        public virtual void Exit()
        {
            this.gameObject.SetActive(false);
        }
    }
}

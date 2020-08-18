using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MatrixJam.Team11
{
    public class GameScene : MonoBehaviour
    {
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

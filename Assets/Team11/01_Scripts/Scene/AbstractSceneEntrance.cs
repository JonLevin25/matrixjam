using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MatrixJam.Team11
{
    public abstract class AbstractSceneEntrance : MonoBehaviour
    {
        protected SceneManager _manager;

        protected virtual void Awake()
        {
            this._manager = Object.FindObjectOfType<SceneManager>();

            if (this._manager == null)
            {
                Debug.LogError($"{this.name}: SceneManager not found!");
            }
        }

        public abstract void Enter();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team11
{
    public class SceneEntrance : MonoBehaviour
    {
        [SerializeField] bool isRedPill;
        [SerializeField] bool isBluePill;

        [SerializeReference] public GameScene TargetScene;

        private SceneManager _manager;

        private void Awake()
        {
            this._manager = Object.FindObjectOfType<SceneManager>();

            if (this._manager == null)
            {
                Debug.LogError($"{this.name}: SceneManager not found!");
            }

            if (this.TargetScene == null)
            {
                Debug.LogWarning($"{this.name}: has no TargetScene!");
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(isRedPill)
            {
                FindObjectOfType<MusicPlayer>().PlayRedPill();
                FindObjectOfType<SunsetPlayerController>().canMove = false;
            }
            else if(isBluePill)
            {
                FindObjectOfType<MusicPlayer>().PlayBluePill();
                FindObjectOfType<SunsetPlayerController>().canMove = false;
            }
            this.Enter();
            this.gameObject.SetActive(false);
        }

        public void Enter()
        {
            if (this.TargetScene != null)
            {
                this._manager.GoToScene(this.TargetScene);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MatrixJam.Team11
{
    public class SceneEntrance : AbstractSceneEntrance
    {
        [SerializeField] bool isRedPill;
        [SerializeField] bool isBluePill;

        [SerializeReference] public GameScene TargetScene;
        [SerializeField] public SceneTransition Transition = SceneTransition.Default;

        [SerializeField] protected UnityEvent OnEnter;

        protected override void Awake()
        {
            base.Awake();

            if (this.TargetScene == null)
            {
                Debug.LogWarning($"{this.name}: has no TargetScene!");
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            this.Enter();
            this.gameObject.SetActive(false);
        }

        public override void Enter()
        {
            if (this.OnEnter != null)
            {
                this.OnEnter.Invoke();
            }

            if (isRedPill)
            {
                FindObjectOfType<MusicPlayer>().PlayRedPill();
            }
            else if (isBluePill)
            {
                FindObjectOfType<MusicPlayer>().PlayBluePill();
            }

            if ((isRedPill || isBluePill)
                && TryGetComponent<SunsetPlayerController>(out SunsetPlayerController player))
            {
                player.canMove = false;
            }

            if (this.TargetScene != null)
            {
                this._manager.GoToScene(this.TargetScene, this.Transition);
            }
        }
    }
}

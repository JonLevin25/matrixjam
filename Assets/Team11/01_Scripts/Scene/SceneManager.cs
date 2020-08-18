using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace MatrixJam.Team11
{
    public class SceneManager : MonoBehaviour
    {
        struct TransitionPack
        {
            public GameScene Target;
            public SceneTransition Transition;

            public TransitionPack(GameScene target, SceneTransition? transition)
            {
                this.Target = target;
                this.Transition = (transition == null) ? (SceneTransition.Default) : ((SceneTransition)transition);
            }
        }

        [SerializeReference] protected GameScene _initialScenePrefab;
        [SerializeReference] protected BlackoutFader _fader;
        [SerializeReference] protected GameObject _temporaryPresence;

        public SceneTransition LastTransition { get; protected set; }
        public GameScene CurrentScene { get; protected set; }
        public GameScene CurrentScenePrefab { get; protected set; }

        private Queue<TransitionPack> _nextTransition;
        private Coroutine _business;
        public bool IsBusy { get { return this._business != null; } }

        private void Awake()
        {
            this._nextTransition = new Queue<TransitionPack>(2);
            this._business = null;
        }

        private void Start()
        {
            this.GoToScene(this._initialScenePrefab, new SceneTransition()
            {
                FadeOutTime = 0f,
                FadeInTime = 0f,
                FadedDuration = 0f,
                FadeColor = new Color()
            });
        }

        private void OnEnable()
        {
            if (Object.FindObjectsOfType<SceneManager>().Length > 1)
            {
                Debug.LogError($"{this.name}: more than one SceneManager present in the scene!!!");
            }
        }

        public void GoToScene(GameScene scenePrefab, SceneTransition? transitionInfo = null)
        {
            this._nextTransition.Enqueue(new TransitionPack(scenePrefab, transitionInfo));
            this.CurrentScenePrefab = scenePrefab;

            if (this.IsBusy == false)
            {
                this._business = this.StartCoroutine(this.ProcessScenesQueueCoroutine());
            }
        }

        public void RestartCurrentScene()
        {
            this.RestartCurrentScene(this.LastTransition);
        }

        public void RestartCurrentScene(SceneTransition? transitionInfo)
        {
            this.GoToScene(this.CurrentScenePrefab, transitionInfo);
        }

        protected virtual IEnumerator ProcessScenesQueueCoroutine()
        {
            while (this._nextTransition.Count > 0)
            {
                TransitionPack next = this._nextTransition.Peek();

                /* black out */
                if (this.CurrentScene != null)
                {
                    this._fader.FadeColor = next.Transition.FadeColor;
                    yield return this.StartCoroutine(this._fader.FadeIn(next.Transition.FadeInTime));
                }

                while (this._nextTransition.Count > 0)
                {
                    if (this.CurrentScene != null)
                    {
                        this.CurrentScene.Exit();
                        this._temporaryPresence.SetActive(true);

                        SceneManager.Destroy(this.CurrentScene);
                    }

                    next = this._nextTransition.Peek();
                    this.LastTransition = next.Transition;

                    yield return this.StartCoroutine(this._fader.LerpOverTime(
                        next.Transition.FadedDuration,
                        this._fader.FadeColor,
                        next.Transition.FadeColor
                        ));
                    this._fader.FadeColor = next.Transition.FadeColor;

                    if (next.Target != null)
                    {
                        this.CurrentScene = SceneManager.Instantiate<GameScene>(next.Target);

                        this._temporaryPresence.SetActive(false);
                        this.CurrentScene.Enter();
                    }
                    else
                    {
                        this.CurrentScene = null;
                    }

                    /* scene processed - dequeue it */
                    this._nextTransition.Dequeue();
                }

                /* black in */
                if (this.CurrentScene != null)
                {
                    yield return this.StartCoroutine(this._fader.FadeOut(next.Transition.FadeOutTime));
                }
            }

            this._business = null;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace MatrixJam.Team11
{
    public class SceneManager : MonoBehaviour
    {
        // TODO: RestartCurrentScene();


        [SerializeReference] protected GameScene InitialScenePrefab;
        [SerializeReference] protected BlackoutFader Fader;
        
        

        public GameScene CurrentScene { get; protected set; }

        private GameScene _restartScenePrefab;
        private Queue<GameScene> _nextScenePrefab;
        private Coroutine _business;
        public bool IsBusy { get { return this._business != null; } }

        private void Awake()
        {
            this._nextScenePrefab = new Queue<GameScene>(2);
            this._business = null;
        }

        private void Start()
        {
            this.GoToScene(this.InitialScenePrefab);
        }

        private void OnEnable()
        {
            if (Object.FindObjectsOfType<SceneManager>().Length > 1)
            {
                Debug.LogError($"{this.name}: more than one SceneManager present in the scene!!!");
            }
        }

        public void GoToScene(GameScene scenePrefab)
        {
            this._nextScenePrefab.Enqueue(scenePrefab);
            this._restartScenePrefab = scenePrefab;

            if (this.IsBusy == false)
            {
                this._business = this.StartCoroutine(this.ProcessScenesQueueCoroutine());
            }
        }

        public void RestartCurrentScene()
        {
            this.GoToScene(this._restartScenePrefab);
        }

        protected virtual IEnumerator ProcessScenesQueueCoroutine()
        {
            while (this._nextScenePrefab.Count > 0)
            {
                /* black out */
                if (this.CurrentScene != null)
                {
                    yield return this.StartCoroutine(this.Fader.FadeIn(1f, 0f, false));
                }

                while (this._nextScenePrefab.Count > 0)
                {
                    if (this.CurrentScene != null)
                    {
                        this.CurrentScene.Exit();
                        Object.Destroy(this.CurrentScene);
                    }

                    GameScene nextScenePrefab = this._nextScenePrefab.Peek();
                    if (nextScenePrefab != null)
                    {
                        this.CurrentScene = Object.Instantiate<GameScene>(nextScenePrefab);
                        this.CurrentScene.Enter();

                        yield return new WaitForSeconds(this.CurrentScene.EnterDelayTime);
                        yield return new WaitUntil(() => this.CurrentScene.gameObject.activeInHierarchy);
                    }
                    else
                    {
                        this.CurrentScene = null;
                    }
                    /* scene processed - dequeue it */
                    this._nextScenePrefab.Dequeue();
                }

                /* black in */
                if (this.CurrentScene != null)
                {
                    yield return this.StartCoroutine(this.Fader.FadeOut(1f));
                }
            }

            this._business = null;
        }
    }
}

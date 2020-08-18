using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace MatrixJam.Team14
{
#if UNITY_EDITOR
    using UnityEditor;

    [CustomEditor(typeof(GameManager))]
    public class GameManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var script = target as GameManager;
            
            GUILayout.Space(20);
            if (GUILayout.Button("Update"))
            {
                script.OnValidate();
            }
        }
    }
#endif

    public class GameManager : MonoBehaviour
    {
        public static event Action ResetEvent;
        public static event Action<bool> GameFinishedEvent;
        
        [SerializeField] private KeyCode[] secretRestartCombo;
        [SerializeField] private int debugStartBeatsOffset;
        [Space]
        [SerializeField] private int startLives;
        [SerializeField] private AudioManager audioManager;
        [SerializeField] public SFXmanager sfxManager;

        [SerializeField] private Transform startAndDirection;

        [SerializeField] private Transform character;
        [SerializeField] private ThomasMoon thomasMoon;

        // [Header("Infra")]
        // [SerializeField] private Exit winExit;
        // [SerializeField] private Exit loseExit;

        private bool reachedEnd;
        [SerializeField] private GameObject gameOver;
        [SerializeField] private GameObject youWin;
        
        public static GameManager Instance { get; private set; }

        public Vector3[] BeatPositions { get; private set; }
        public Vector3[] TrackStartPositions { get; set; }
        public Vector3[] TrackEndPositions { get; private set; }


        private void Awake()
        {
            sfxManager = FindObjectOfType<SFXmanager>();
            if (Instance != null)
            {
                Debug.LogError("There shouldnt be 2 trains!");
                Destroy(gameObject);
                return;
            }

            Instance = this;

            TrainController.Instance.Lives = startLives;
            audioManager.OnFinishTrack += OnTrackFinished;
            audioManager.OnFinishTracklist += OnTrackListFinished;
            
            // Can maybe get rid of this if it causes problems
            UpdateBeatPositions();    
        }

        private void Start()
        {
            audioManager.Restart(debugStartBeatsOffset);
        }

        public void OnValidate()
        {
            UpdateBeatPositions();
        }

        private void Update()
        {
            if (secretRestartCombo.Length > 0 && secretRestartCombo.All(Input.GetKey)) RestartLevel();
            if (reachedEnd) return;
            
            var pos = audioManager.GetCurrPosition(startAndDirection);
            character.position = pos;
        }

        private void RestartLevel()
        {
            var sceneName = UnityEngine.SceneManagement.SceneManager.GetSceneAt(0).name;
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        }

        private void OnDestroy()
        {
            audioManager.OnFinishTrack -= OnTrackFinished;
            audioManager.OnFinishTracklist -= OnTrackListFinished;
            
            if (Instance != this) return;
            Instance = null;
        }

        public static float GetTimeinTracklist() => Instance.audioManager.GetCurrGlobalSecs();

        private void OnTrackFinished(int i)
        {
            Debug.Log($"Track {i} finished!");
        }

        private void OnTrackListFinished()
        {
            reachedEnd = true;
            youWin.SetActive(true);
            GameFinishedEvent?.Invoke(true);
            
            MatrixExit(true, 8);
            Debug.Log("Success! Last Track Finished!");
        }

        private void UpdateBeatPositions()
        {
            BeatPositions = audioManager.GetAllBeatPositions(startAndDirection);
            TrackEndPositions = audioManager.GetTrackEndPositions(startAndDirection);
            TrackStartPositions = audioManager.GetTrackStartPositions(startAndDirection);
        }

        private void MatrixExit(bool win, float delay)
        {
            // StartCoroutine(Routine());
            // IEnumerator Routine()
            // {
            //     yield return new WaitForSeconds(delay);
            //     var exit = win ? winExit : loseExit;
            //     exit.EndLevel();
            // }
        }

        public void OnDeath()
        {
            GameOverExplosive.Explode();
            var livesRemaining = --TrainController.Instance.Lives;
            if (livesRemaining == 0)
            {
                Invoke(nameof(DoDeath), 5f);
            }
            else
            {
                Invoke(nameof(DoDeath), 2f);
            }
            sfxManager.TunnelBump.PlayRandomPitch();

        }

        private void DoDeath()
        {
            Debug.Log("StopExplosion");
            GameOverExplosive.StopExplosion();
            if (TrainController.Instance.Lives == 0) OnGameOver();
            else Restart();
        }

        private void OnGameOver()
        {
            Debug.Log("GAME OVERRR");
            sfxManager.Lose.PlayRandom();
            gameOver.SetActive(true);
            GameFinishedEvent?.Invoke(false);
            MatrixExit(false, 8f);
        }

        private void Restart()
        {
            Debug.Log("RESTART!");
            audioManager.RestartLastCheckpoint();

            ResetEvent?.Invoke();
        }
    }
}

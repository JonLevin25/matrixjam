using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Assertions;

namespace MatrixJam.Team14
{
    public struct FutureAnimation
    {
        public readonly Animator Anim;
        public readonly float Value;
        public readonly string Trigger;

        public FutureAnimation(Animator anim, float value, string trigger)
        {
            Anim = anim;
            Value = value;
            Trigger = trigger;
        }
    }
    
    public class TrainController : MonoBehaviour
    {
        public static TrainController Instance { get; private set; }
        
        [SerializeField] private bool debugDontHonkOnStart;
        [Space]
        
        [SerializeField] private ThomasMoon thomas;
        [SerializeField] private SFXmanager sfxManager;
        [SerializeField] private float startHonkDelay;
        [SerializeField] private new Collider collider;

        [Header("States config")]
        [SerializeField] private float jumpTime = 0.4f;
        [SerializeField] private float honkTime = 2.28f;


        [Header("Cars config")]
        [SerializeField] private Animator masterCarAnim;
        [SerializeField] private Animator[] slaveCarAnims;

        [Header("Debug")]
        [SerializeField] private bool debugTrackTime = true;
        [SerializeField] private bool debugStates;
        [SerializeField] private bool debugObstacles;
        [SerializeField] private Color  debugTrackColor = new Color(1f, 0.5f, 0f, 1f);
        [SerializeField] private Color debugStatesColor = Color.red;
        [SerializeField] private Color debugObstaclesColor = Color.green;
        [SerializeField] private Vector2 debugSize = new Vector2(2, 2);

        private int _lives;
        private TrainState _currstate;
        private TrainState _prevState;

        public ThomasMoon Thomas => thomas;

        public int Lives
        {
            get => _lives;
            set
            {
                _lives = value;
                SetCarsNum(_lives);
            }
        }

        private static IEnumerable<string> AllTriggers => AllStates.Select(state => state.AnimTrigger);

        private static IEnumerable<TrainState> AllStates
        {
            get
            {
                yield return DriveState;
                yield return HonkState;
                yield return JumpState;
                yield return DuckState;
            }
        }

        public bool Honking { get; private set; }
        public static TrainState DriveState { get; private set; }
        public static TrainState HonkState { get; private set; }
        public static TrainState JumpState { get; private set; }
        public static TrainState DuckState { get; private set; }
        public static TrainState NullState { get; private set; }

        private HashSet<FutureAnimation> _futureAnimations = new HashSet<FutureAnimation>();

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError("There shouldnt be 2 trains!");
                Destroy(gameObject);
                return;
            }

            Instance = this;
            CreateStates();
            _currstate = NullState;
            
            Obstacle.OnObstacleEvent += OnObstacleEvent;
            GameManager.ResetEvent += OnGameReset;
        }

        private IEnumerator Start()
        {
            if (debugDontHonkOnStart)
                TransitionState(DriveState, null);
            else
            {
                yield return new WaitForSeconds(startHonkDelay);
                TransitionState(HonkState, null);
            }
        }

        private void OnValidate()
        {
            CreateStates();
        }

        private void OnDestroy()
        {
            if (Instance != this) return;
            Obstacle.OnObstacleEvent -= OnObstacleEvent;
            GameManager.ResetEvent -= OnGameReset;
            Instance = null;
        }

        private void Update()
        {
            _currstate?.OnUpdate();
            HandlePendingAnimations();
        }

        private void OnGUI()
        {
            GUI.matrix = Matrix4x4.TRS(new Vector3(20f, 0f ,0f), Quaternion.identity, new Vector3(debugSize.x, debugSize.y, 1f));
            GUILayout.Space(10);
            if (debugTrackTime)
            {
                GUI.color = debugTrackColor;
                
                var trackTimeStr = FormatSecs(GameManager.GetTimeinTracklist());
                GUILayout.Label($"Total Time: {trackTimeStr}");
            }
            if (debugStates)
            {
                GUI.color = debugStatesColor;
                GUILayout.Label($"TrainState: {_currstate?.Name}");
            }

            var obstacleDict = Obstacle.CurrObstacles;
            if (debugObstacles && obstacleDict != null)
            {
                GUI.color = debugObstaclesColor;
                GUILayout.Label("Obstacles");
                foreach (var trainMove in obstacleDict.Keys)
                {
                    var obstacles = obstacleDict[trainMove];
                    GUILayout.Label($"[{trainMove}]: {obstacles.Count}");
                }
            }
        }

        /// <summary>
        /// Add future animation cues for master + slave chars, using the transform given or master car
        /// </summary>
        /// <param name="trigger">The trigger to cue</param>
        /// <param name="moveCue">The transform to use for position, if null will use masterCar postion</param>
        public void CueFutureAnimations(string trigger, Transform moveCue)
        {
            var value = moveCue
                ? moveCue.position.z
                : masterCarAnim.transform.position.z;
            
            Debug.Log($"CueAnim: {trigger} ({moveCue?.position.z:F1})");
            
            // Master car
            var masterFutureAnim = new FutureAnimation(masterCarAnim, value, trigger);
            _futureAnimations.Add(masterFutureAnim);
            
            foreach (var slaveCarAnim in slaveCarAnims)
            {
                var futureAnim = new FutureAnimation(slaveCarAnim, value, trigger);
                _futureAnimations.Add(futureAnim);
            }
        }

        private void OnGameReset()
        {
            collider.enabled =true;
            TransitionState(DriveState, null);
        }

        public static void TransitionState(TrainState newState, Transform moveCue) => Instance.TransitionStateInternal(newState, moveCue);

        private void TransitionStateInternal(TrainState newState, Transform moveCue)
        {
            if (newState == _currstate && !_currstate.AllowSelfTransition)
            {
                Debug.LogWarning($"Same state transition not allowed for state ({newState}). Ignoring.");
                return;
            }
            
            Assert.IsNotNull(newState);

            Debug.Log($"State Transition: {_prevState?.Name} -> {newState.Name}");
            _prevState?.OnExit();
            
            _currstate = newState;
            newState.OnEnter();

            if (newState.AnimTrigger != null)
            {
                CueFutureAnimations(newState.AnimTrigger, moveCue);
                HandlePendingAnimations();
            }
            
            _prevState = newState;
        }


        public void PlaySFX(TrainMove move)
        {
            Debug.Log("PlaySFX " + move);
            
            if (sfxManager == null) return;
            sfxManager.PlaySFX(move);
        }

        private void CreateStates()
        {
            DriveState = new TrainDriveState();
            
            HonkState = new TrainHonkState(honkTime, DriveState);
            JumpState = new TrainJumpState(jumpTime, DriveState);
            DuckState = new TrainDuckState();    
            NullState = new TrainNullState();
        }

        private void OnObstacleEvent(ObstaclePayload payload)
        {
            Debug.Log("Obstacle failed!");
            // Handle failed only currently (success comes from TrainState)
            if (!payload.Successful)
            {
                OnObstacleFailed(payload);
                return;
            }
            
            // // TODO: ???
            // var nextState = GetState(payload.Move);
            // TransitionState(nextState, payload.MoveCue);
        }

        private void OnObstacleFailed(ObstaclePayload payload)
        {
            var obstacleHolder = payload.ObstacleHolder; 
            if (obstacleHolder)
            {
                // Add script to train that will kill it when it reaches the obstacle
                const float approxObstacleWidth = 0.5f;
                var targetZ = obstacleHolder.position.z;
                var trainKiller = gameObject.AddComponent<TrainKiller>();
                trainKiller.Init(this, trans => trans.position.z + approxObstacleWidth >= targetZ);
            }
            else
                KillTrain();
        }

        public void KillTrain()
        {
            collider.enabled = false;
            GameManager.Instance.OnDeath();
        }

        private void HandlePendingAnimations()
        {
            foreach (var futureAnim in _futureAnimations.Reverse())
            {
                var currValue = futureAnim.Anim.transform.position.z;
                if (currValue >= futureAnim.Value)
                {
                    _futureAnimations.Remove(futureAnim);
                    SetOnlyTrigger(futureAnim.Anim, futureAnim.Trigger);
                }
            }
        }

        private void SetCarsNum(int lives)
        {
            var activeCars = slaveCarAnims.Take(lives);
            var inactiveCars = slaveCarAnims.Skip(lives);

            foreach (var car in activeCars)
                car.gameObject.SetActive(true);
            
            foreach (var car in inactiveCars)
                car.gameObject.SetActive(false);
        }


        public TrainState GetState(TrainMove move)
        {
            switch (move)
            {
                case TrainMove.Jump:
                    return JumpState;
                case TrainMove.Duck:
                    return DuckState;
                case TrainMove.Honk:
                    return HonkState;
                default:
                    throw new ArgumentOutOfRangeException(nameof(move), move, null);
            }
        }

        private static void SetOnlyTrigger(Animator anim, string trigger)
        {
            // Debug.Log($"({anim.name}) Setting Trigger + Resetting rest - {trigger}");
            foreach (var trig in AllTriggers)
            {
                anim.ResetTrigger(trig);
            }
            
            anim.SetTrigger(trigger);
        }

        private static string FormatSecs(float seconds)
        {
            var time = TimeSpan.FromSeconds(seconds);
            return time.ToString(@"mm\:ss\.fff");
        }
    }
}

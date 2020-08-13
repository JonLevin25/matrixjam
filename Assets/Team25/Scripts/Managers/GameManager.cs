using System;
using Cinemachine;
using MatrixJam.Team25.Scripts.Jar;
using MatrixJam.Team25.Scripts.States.States;
using MatrixJam.Team25.Scripts.UI;
using UnityEngine;
using UnityEngine.UI;
using StateMachine = MatrixJam.Team25.Scripts.States.StateMachine;

namespace MatrixJam.Team25.Scripts.Managers
{
    public class GameManager : MonoBehaviour
    {
        [Header("General")] 
        public int maxRounds;
        [Header("Start State")]
        public GameObject startScreen;
        public StartFartButton startFartButton;
        [Header("Fart State")] 
        public ParticleSystem fartParticleSystem;
        public GameObject jar, lid;
        public Animator farterAnimator;
        [Header("Poop State")] 
        public PoopScreen poopScreen;
        public Image poopImage;
        public float poopChance;
        [Header("Game Over")] 
        public UIManager uiManager;
        [HideInInspector] public bool pooped;
        private DataManager dataManager;
        private StateMachine stateMachine;
        private CinemachineVirtualCamera virtualCamera;
        private StartGameState startGame;

        private void Awake()
        {
            virtualCamera = GameObject.FindWithTag("Tag1").GetComponent<CinemachineVirtualCamera>();
            virtualCamera.Follow = jar.transform;
            DefineStateMachine();
            dataManager = FindObjectOfType<DataManager>();
            pooped = false;
        }

        private void Start()
        {
            if (dataManager.round != 0)
            {
                uiManager.HideIntro();
            }

            if (dataManager.round == 1)
            {
                dataManager.totalScore = 0f;
                uiManager.SetTotalScore(0);
            }
        }

        private void DefineStateMachine()
        {
            stateMachine = new StateMachine();
            //Define states
            startGame = new StartGameState(startScreen, startFartButton);
            var fart = new FartState(fartParticleSystem, farterAnimator, poopChance);
            var lidOn = new LidOnState(jar, lid);
            var lidClosed = new LidClosedState();
            var poop = new PoopState(poopScreen);

            //Define transitions
            At(fart, startGame, FartStarted());
            At(lidOn, fart, LidIsOn());
            At(lidClosed, lidOn, LidClosed());
            At(poop, fart, Pooped());


            void At(IState to, IState from, Func<bool> condition) => stateMachine.AddTransition(@from, to, condition);

            //Define predicates as Func<bool>
            Func<bool> FartStarted() => () => startFartButton.fartPressed && Time.timeScale != 0;
            Func<bool> LidIsOn() => () => jar.GetComponentInChildren<LidTrigger>().lidOn;
            Func<bool> LidClosed() => () => lid.GetComponent<LidTwist>().jarClosed;
            Func<bool> Pooped() => () => pooped;
        }

        public void StartGame()
        {
            stateMachine.SetState(startGame);
        }
        
        private void Update()
        {
            stateMachine.Tick();
        }

        public void ResetGame()
        {
            dataManager.ResetGame();
        }
        
        public void IncreaseRound(bool pooped)
        {
            if (pooped)
            {
                dataManager.pooped++;
            }
            if (dataManager.round <= maxRounds)
            {
                dataManager.round++;
            }
            else
            {
                GameOver();
            }
        }

        public void GameOver()
        {
            startScreen.SetActive(false);
            uiManager.DisplayGameOver();
        }

        public void ExitGame()
        {
            dataManager.ExitGame();
        }
        public void PauseGame()
        {
            Time.timeScale = 0;
        }

        public void UnPauseGame()
        {
            Time.timeScale = 1;
        }
    }
}

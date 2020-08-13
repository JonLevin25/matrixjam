using System;
using System.Collections.Generic;
using MatrixJam.Team;
using UnityEngine;

namespace MatrixJam.Team4
{
    public class GameManager : StartHelper
    {
        public GameState gameState = GameState.init;
        private IntroManager _introManager;
        private TurnManager _turnManager;
        private BoardManager _boardManager;
        private IChoiceManager _choiceManager;
        public AiBrain[] AllBrains;
        public static AiBrain Brain;
        

        public List<Player> Players;
        public List<MessageScript> IntroMessages;
        public MessageScript LoseMessage;
        public MessageScript WinMessage;

        public int InitialUnitsCount = 35;
        public bool ShowTutorial;
        private static bool _alreadyPlayedTheTutorial;//in case they end up in our game twice

        void Awake()
        {
            _introManager = new IntroManager(IntroMessages);
            _boardManager = new BoardManager(9);
            _choiceManager = new ChoiceManager(_boardManager);
            _turnManager = new TurnManager(Players);
            Brain = AllBrains[0];
            UIManager.ChoiceManager = _choiceManager;

        }

    
        // Start is called before the first frame update
        void Start()
        {
            _boardManager.AddRandomUnits(InitialUnitsCount);
            if (ShowTutorial && !_alreadyPlayedTheTutorial)
            {
                SetState(GameState.intro);
            }
            else
            {
                SetState(GameState.mainPlay);
            }
        }

        public override void StartHelp(int num_ent)
        {
            if (num_ent <= AllBrains.Length)
            {
                Brain = AllBrains[num_ent];
            }
        }

        private void HandleIntro()
        {
            EventManager.Singleton.IntroDone += OnIntroDone;     
            EventManager.Singleton.NextMessage += _introManager.NextMessage;   
            _introManager.Start();
        }
        
        public void NextMessage()
        {
            _introManager.NextMessage();
        }
        
        private void HandleMainPlay()
        {
            if (ShowTutorial)
            {
                EventManager.Singleton.IntroDone -= OnIntroDone;     
                EventManager.Singleton.NextMessage -= _introManager.NextMessage;
            }
            
            EventManager.Singleton.TurnOver += OnTurnOver;
            EventManager.Singleton.GameOver += OnGameOver;

            OnTurnOver();
        }
        
        private void HandleGameover()
        {
            var winner = FindWinner();
            HandleWinner(winner);
        }

        private Player FindWinner()
        {
            Player winner = Players[0];
            foreach (var player in Players)
            {
                if (player.Score > winner.Score)
                {
                    winner = player;
                }
            }

            return winner;
        }

        private void HandleWinner(Player winner)
        {
            if (winner.playerSide == PlayerSide.Human)
            {
                SoundManager.Instance.PlayVictory();
                WinMessage.gameObject.SetActive(true);
            }
            else
            {
                SoundManager.Instance.PlayDefeat();
                LoseMessage.gameObject.SetActive(true);
            }
        }

        private void OnTurnOver()
        {
            
            var nextPlayer = _turnManager.GetNextPlayer();
            var nextTurnData = _boardManager.GetPlayerTurnData(nextPlayer);
            if (IsGameOver(nextTurnData))
            {
                EventManager.Singleton.OnGameOver();
                return;
            }
            nextPlayer.YourTurn(nextTurnData);
        }

        private bool IsGameOver(TurnData nextTurnData)
        {
            return nextTurnData._positionOptions.Count == 0;
        }

        private void OnIntroDone()
        {
            ShowTutorial = false;
            SetState(GameState.mainPlay);
        }
        
        private void OnGameOver()
        {
            SetState(GameState.gameover);
        }

        public void SetState(GameState gameState)
        {
            switch (gameState)
            {
                case GameState.intro:
                    HandleIntro();
                    break;
                case GameState.mainPlay:
                    HandleMainPlay();
                    break;
                case GameState.gameover:
                    HandleGameover();
                    break;
            }
        }
        public static bool PlayRandom { get; set; }
    }

    public enum GameState
    {
        init,
        intro,
        mainPlay,
        gameover
    }

}

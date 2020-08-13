using MatrixJam.Team25.Scripts.Fart;
using MatrixJam.Team25.Scripts.Managers;
using UnityEngine;

namespace MatrixJam.Team25.Scripts.States.States
{
    public class LidClosedState : IState
    {
        private DataManager dataManager;
        private GameManager gameManager;
        private UIManager uiManager;
        private SoundManager soundManager;
        public LidClosedState()
        {
            gameManager = GameObject.FindObjectOfType<GameManager>();
            dataManager = GameObject.FindObjectOfType<DataManager>();
            uiManager = GameObject.FindObjectOfType<UIManager>();
            soundManager = GameObject.FindObjectOfType<SoundManager>();
        }

        public void Tick()
        {
        }

        public void OnEnter()
        {
            var fartCollision = GameObject.FindObjectOfType<FartColiisionHandler>();
            var scoreManager = new ScoreManager();
            float score = scoreManager.CalculateScore(fartCollision.GetInsideParticles(), dataManager.stink);
            dataManager.totalScore += score; 
            gameManager.IncreaseRound(false);
            uiManager.ShowRoundScore(score);
            uiManager.SetTotalScore(dataManager.totalScore);
            soundManager.KaChing();
            if (dataManager.round <= gameManager.maxRounds)
            {
                uiManager.ShowResetButton();
            }
            else
            {
                gameManager.GameOver();
            }
        }

        public void OnExit()
        {
        }
    }
}
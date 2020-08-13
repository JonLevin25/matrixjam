using System;
using UnityEngine;
using UnityEngine.UI;

namespace MatrixJam.Team25.Scripts.Managers
{
    public class UIManager : MonoBehaviour
    {
        public Button resetButton;
        public Text scoreText;
        public Text roundText;
        public Text roundScoreText;
        public GameObject roundScoreDisplay;
        public GameObject tutorialEnd;
        public GameObject gameOverScreen;
        [Header("Intro Screen")] 
        public GameObject introScreen;
        private DataManager dataManager;
        private GameManager gameManager;

        private void Awake()
        {
            dataManager = FindObjectOfType<DataManager>();
            gameManager = FindObjectOfType<GameManager>();
        }

        private void Start()
        {
            SetTotalScore(dataManager.totalScore);
            SetRound();
        }

        public void ShowResetButton()
        {
            if (dataManager.round == 1)
            {
                tutorialEnd.SetActive(true);
            }
            resetButton.gameObject.SetActive(true);
        }

        public void SetTotalScore(float score)
        {
            scoreText.text = score.ToString("F2") + "$";
        }

        private void SetRound()
        {
            var round = dataManager.round == 0 ? "Tutorial" : dataManager.round + "/" + gameManager.maxRounds;  
            roundText.text = round;
        }

        public void ShowRoundScore(float score)
        {
            roundScoreDisplay.SetActive(true);
            roundScoreText.text = score.ToString("F2") + "$";
        }

        public void HideElements()
        {
            scoreText.transform.parent.gameObject.SetActive(false);
            roundText.transform.parent.gameObject.SetActive(false);
        }

        public void DisplayGameOver()
        {
            gameOverScreen.SetActive(true);
            ShowRoundScore(dataManager.totalScore);
        }

        public void HideIntro()
        {
            scoreText.transform.parent.gameObject.SetActive(true);
            roundText.transform.parent.gameObject.SetActive(true);
            introScreen.SetActive(false);
            gameManager.StartGame();
        }
    }
}

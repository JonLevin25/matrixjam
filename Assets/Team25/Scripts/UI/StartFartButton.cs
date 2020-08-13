using MatrixJam.Team25.Scripts.Managers;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace MatrixJam.Team25.Scripts.UI
{
    public class StartFartButton : MonoBehaviour
    {
        public Button startButton;
        public Bar[] bars;
        public GameObject startButtonTutorial, fartButtonTutorial;
        [HideInInspector] public bool fartPressed = false;
        private DataManager dataManager;
        private GameManager gameManager;

        private void Awake()
        {
            dataManager = FindObjectOfType<DataManager>();
            gameManager = FindObjectOfType<GameManager>();
            if (dataManager.round != 0)
            {
                startButtonTutorial.SetActive(false);
            }
        }

        public void OnStart()
        {
            fartPressed = false;
            startButton.gameObject.SetActive(false);
            foreach (var bar in bars)
            {
                bar.gameObject.SetActive(true);
                bar.OscillateBarFill();
            }
        }

        public void OnFart()
        {
            if (dataManager.round == 0)
            {
                gameManager.PauseGame();
                startButtonTutorial.SetActive(false);
                fartButtonTutorial.SetActive(true);
            }
            foreach (var bar in bars)
            {
                bar.KillBar();
                fartPressed = true;
                dataManager.stink = bars[0].barImage.fillAmount;
                dataManager.fartForce = bars[1].barImage.fillAmount;
            }
        }

        public void ResetBars()
        {
            startButton.gameObject.SetActive(true);
            foreach (var bar in bars)
            {
                bar.ResetBar();
            }
        }
    }
}

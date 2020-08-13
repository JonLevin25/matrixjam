using System;
using DG.Tweening;
using MatrixJam.Team25.Scripts.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace MatrixJam.Team25.Scripts.UI
{
    public class PoopScreen : MonoBehaviour
    {
        public Image poopImage;
        public float startDelay, duration, endValue;
        private UIManager uiManager;
        private SoundManager soundManager;
        private GameManager gameManager;
        private DataManager dataManager;

        private void Awake()
        {
            soundManager = FindObjectOfType<SoundManager>();
            uiManager = FindObjectOfType<UIManager>();
            gameManager = FindObjectOfType<GameManager>();
            dataManager = FindObjectOfType<DataManager>();
        }

        public void TriggerPoop()
        {
            Invoke(nameof(Poop), startDelay);
        }
        
        private void Poop()
        {
            soundManager.Shit();
            uiManager.HideElements();
            poopImage.transform.DOLocalMoveY(endValue, duration).SetEase(Ease.OutQuad).OnComplete(DisplayReset);
        }

        private void DisplayReset()
        {
            if (dataManager.round <= gameManager.maxRounds)
            {
                uiManager.ShowResetButton();
            }
            else
            {
                gameManager.GameOver();
            }
        }
    }
}

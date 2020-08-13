using MatrixJam.Team25.Scripts.Managers;
using MatrixJam.Team25.Scripts.UI;
using UnityEngine;

namespace MatrixJam.Team25.Scripts.States.States
{
    public class StartGameState : IState
    {
        private readonly GameObject start;
        private readonly StartFartButton button;
        public StartGameState(GameObject startScreen, StartFartButton startFartButton)
        {
            start = startScreen;
            button = startFartButton;
        }
        public void Tick()
        {
     
        }

        public void OnEnter()
        {
            start.gameObject.SetActive(true);
        }

        public void OnExit()
        {
            button.fartPressed = false;
            button.ResetBars();
            start.gameObject.SetActive(false);
        }
    }
}

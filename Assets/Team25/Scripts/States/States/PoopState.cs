using System.Collections;
using MatrixJam.Team25.Scripts.Managers;
using UnityEngine;
using MatrixJam.Team25.Scripts.UI;

namespace MatrixJam.Team25.Scripts.States.States
{
    public class PoopState : IState
    {
        private readonly PoopScreen pooped;
        private GameManager gameManager;
        public PoopState(PoopScreen poopScreen)
        {
            gameManager = GameObject.FindObjectOfType<GameManager>();
            pooped = poopScreen;
        }
        public void Tick()
        {
            
        }

        public void OnEnter()
        {
            pooped.TriggerPoop();
            gameManager.IncreaseRound(true);
        }

        
        public void OnExit()
        {
            
        }
    }
}

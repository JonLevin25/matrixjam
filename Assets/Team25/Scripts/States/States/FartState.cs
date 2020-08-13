using MatrixJam.Team25.Scripts.Jar;
using MatrixJam.Team25.Scripts.Managers;
using UnityEngine;

namespace MatrixJam.Team25.Scripts.States.States
{
    public class FartState : IState
    {
        private readonly ParticleSystem fart;
        private readonly Animator farter;
        private readonly float poop;
        private DataManager data;
        private static readonly int Fart = Animator.StringToHash("fart");
        private static readonly int Speed = Animator.StringToHash("speed");

        public FartState(ParticleSystem fartParticleSystem, Animator farterAnimator, float poopChance)
        {
            fart = fartParticleSystem;
            farter = farterAnimator;
            poop = poopChance;
        }
        public void Tick()
        {
        
        }

        public void OnEnter()
        {
            data = GameObject.FindObjectOfType<DataManager>();
            GameObject.FindObjectOfType<LidTwist>().GetComponent<DragMove>().enabled = true;
            var main = fart.main;
            main.startSpeed = new ParticleSystem.MinMaxCurve(data.fartForce, data.fartForce * 10);
            float poopChance = data.round == 0 ? 0 : data.fartForce * poop;
            if (Random.Range(0f, 1f) < poopChance)
            {
                farter.SetBool(Fart, false);
                GameObject.FindObjectOfType<GameManager>().pooped = true;
            }
            else
            {
                farter.SetBool(Fart, true);
                fart.gameObject.SetActive(true);
                GameObject.FindObjectOfType<SoundManager>().Fart();
            }
            farter.SetFloat(Speed, 1f);
        }

        public void OnExit()
        {
         
        }
    }
}

using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team25.Scripts.Fart
{
    public class FartColiisionHandler : MonoBehaviour
    {
        private ParticleSystem ps;
        
        public List<ParticleSystem.Particle> enter = new List<ParticleSystem.Particle>();
        public List<ParticleSystem.Particle> exit = new List<ParticleSystem.Particle>();
        public List<ParticleSystem.Particle> inside = new List<ParticleSystem.Particle>();
        // public List<ParticleSystem.Particle> outside = new List<ParticleSystem.Particle>();
        
        [HideInInspector] public int counter;
        
        private int insideParticleNumber;
        private void OnEnable()
        {
            counter = 0;
            ps = GetComponent<ParticleSystem>();
        }

        private void OnParticleTrigger()
        {
            // int enterParticleNumber = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter);
            // int exitParticleNumber = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Exit, exit);
            insideParticleNumber = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Inside, inside);
            // int outsideParticleNumber = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Outside, outside);


            // for (int i = 0; i < exitParticleNumber; i++)
            // {
            //     ParticleSystem.Particle p = exit[i];
            //     // p.startColor = Color.red;
            //     exit[i] = p;
            //     counter--;
            // }
            // for (int i = 0; i < enterParticleNumber; i++)
            // {
            //     ParticleSystem.Particle p = enter[i];
            //     p.startColor = Color.green;
            //     enter[i] = p;
            //     counter++;
            // }
            // ps.SetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter);
            // ps.SetTriggerParticles(ParticleSystemTriggerEventType.Exit, exit);
            ps.SetTriggerParticles(ParticleSystemTriggerEventType.Inside, inside);
            // ps.SetTriggerParticles(ParticleSystemTriggerEventType.Outside, outside);
            // print(counter);
        }

        public void ChangeLifeTime(float lifeTime)
        {
            Debug.Log("inside particles: " + insideParticleNumber);
        }

        public int GetInsideParticles()
        {
            return insideParticleNumber;
        }
    }
}

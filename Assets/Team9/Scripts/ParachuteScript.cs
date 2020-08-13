using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team9
{
    public class ParachuteScript : MonoBehaviour
    {
        public GameObject windParticle;

        private GameObject wind;

        public List<Transform> HitPoint;

        public HealthBar healthBarRef;

        public bool takeDamageHit = false;
        
        public float _health = 100;

        public bool _flagEnded = false;

        private ParticleSystem particleSys;

        public bool _SetPitch = false;

        [SerializeField] private Sprite Parachute1;
        [SerializeField] private Sprite Parachute2;
        [SerializeField] private Sprite Parachute3;
        [SerializeField] private Sprite Parachute4;

        [SerializeField] private GameObject stringsAttached;

        [SerializeField] private AudioSource AudioControlPitch;

        private AudioSource AudioSourceParachute;

        public AudioClip tear1;
        public AudioClip tear2;
        public AudioClip tear3;
        public AudioClip tear4;



        // Start is called before the first frame update

        private void Start()
        {
            AudioSourceParachute = GetComponent<AudioSource>();
            windParticle.gameObject.SetActive(false);
        }

        private void FixedUpdate()
        {
            if (_health == 0)
            {
                _flagEnded = true;
            }

            if(_flagEnded)
            {
                windParticle.SetActive(false);
            }


            for (int i = 0; i < HitPoint.Count; i++)
            {
                
            }
        }

        public void TakeDamage(int damageReduce)
        {
                takeDamageHit = true;
                _health -= damageReduce;
                changeParachute(_health);
                
        }

        public void startWind()
        {
            windParticle.gameObject.SetActive(true);
            Vector3 templocation = new Vector3(0f, 1.47f, 0f);
            Vector3 rotation = new Vector3(-90f, 0f, 0f);
            wind = windParticle;
            particleSys = wind.GetComponent<ParticleSystem>();
        }

        private void changeParachute(float changeto)
        {
            if (changeto <= 75 && changeto > 50)
            {
                _SetPitch = true;
                this.gameObject.GetComponent<SpriteRenderer>().sprite = Parachute2;
                healthBarRef.ChangeHealth(2);
                AudioSourceParachute.PlayOneShot(tear1);
                AudioControlPitch.pitch += 0.5f;
            }
            else if (changeto <= 50 && changeto > 25)
            {
                this.gameObject.GetComponent<SpriteRenderer>().sprite = Parachute3;
                healthBarRef.ChangeHealth(3);
                AudioSourceParachute.PlayOneShot(tear2);
                AudioControlPitch.pitch += 0.02f;

            }

            else if (changeto <= 25 && changeto > 0)
            {
                this.gameObject.GetComponent<SpriteRenderer>().sprite = Parachute4;
                healthBarRef.ChangeHealth(4);
                AudioSourceParachute.PlayOneShot(tear3);
                AudioControlPitch.pitch += 0.02f;

            }

            if (changeto == 0)
            {
                this.gameObject.GetComponent<SpriteRenderer>().sprite = null;
                stringsAttached.GetComponent<SpriteRenderer>().sprite = null;

                healthBarRef.ChangeHealth(0);
                AudioSourceParachute.PlayOneShot(tear4);
                AudioControlPitch.Stop();

            }


        }

        public void StopMusic()
        {
            AudioControlPitch.Stop();
        }

        public void modifyWind(float speed)
        {
            var main = particleSys.main;
            main.simulationSpeed += 1f;
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team9
{

    public class BirdScript : MonoBehaviour
    {
        public GameObject _deathParticale;

        public float _birdSpeed;

        private Transform _parachutePos;

        private List<Transform> hitPoints;

        private int _randSpot;

        private int _hitCounter = 0;

        public int birdsStamina;

        private bool flyingToTarget = false;

        private bool flyAway = false;

        private Vector3 randFly;

        private ParachuteScript parachutScriptRef;

        public int damageBird = 10;

        private bool _isPicking = false;

        private bool isDead = false;

        public bool abort = false;

        [SerializeField] private AudioClip[] Birds;
        private AudioSource audioSoruce;

        // Start is called before the first frame update
        void Start()
        {
            audioSoruce = this.gameObject.GetComponent<AudioSource>();
            int randomPlaytrueFalse = UnityEngine.Random.Range(0, 2);
            if(audioSoruce!=null)
            {
                for (int i = 0; i < Birds.Length; i++)
                {
                    if (randomPlaytrueFalse == 0)
                    {
                        int randomPlay = UnityEngine.Random.Range(0, Birds.Length);
                        audioSoruce.clip = Birds[randomPlay];
                        audioSoruce.Play();
                    }

                }
            }
            
            //decide the exit path already
            var randomPointUp = 15;
            var randomPointSide =UnityEngine.Random.Range(-20, 22);            
            randFly = new Vector3(randomPointSide, randomPointUp, -1f);

            //getting the partachut object to know where to go to in the update
            GameObject _parachute = GameObject.Find("EggParachute");
            parachutScriptRef = _parachute.GetComponent<ParachuteScript>();
            if(_parachute != null)
            {
                parachutScriptRef = _parachute.GetComponent<ParachuteScript>();
                hitPoints = parachutScriptRef.HitPoint;
            }

            //getting the parachute transform obejct
            _parachutePos = _parachute.transform;

            //assigning a random spot on Parachute
            for (int i = 0; i < this.hitPoints.Count; i++)
            {
                _randSpot = UnityEngine.Random.Range(i, this.hitPoints.Count);
                break;
            }

            //assuming we started we say were flying to object parachute
            flyingToTarget = true;

            abort = false;
        }

        IEnumerator playDeath()
        {
                Instantiate(_deathParticale, transform.position, transform.rotation);
                yield return new WaitForSeconds(0.1f);
                isDead = false;
                Destroy(this.gameObject);
        }
        IEnumerator playPickAnimation()
        {
            
            //here we should play the animation for X seconds of animation of picking then fly away
            yield return new WaitForSeconds(0.6f);
            FlyAway();
        }
        // Update is called once per frame
        void Update()
        {
            if (abort)
            {
                transform.position = Vector3.MoveTowards(transform.position, randFly, _birdSpeed * Time.deltaTime);
            }
            if (flyingToTarget && !abort)
            {
                transform.position = Vector3.MoveTowards(transform.position, this.hitPoints[_randSpot].position, _birdSpeed * Time.deltaTime);
                if (transform.position == this.hitPoints[_randSpot].position)
                {
                    //play picking animation
                    //wait 2 seconds
                    //fly away
                    flyingToTarget = false;
                    parachutScriptRef.TakeDamage(damageBird);
                }
            }
            else if(!flyingToTarget && !abort)
            {
                StartCoroutine(playPickAnimation());
            }

            //fly away to random spot on screen and killing itslef
            if(flyAway && !abort)
            {
                transform.position = Vector3.MoveTowards(transform.position, randFly, _birdSpeed * Time.deltaTime);
            }

            //kill bird when out of screen
            if(randFly == transform.position)
            {
                Destroy(this.gameObject);
            }

        }


        private void OnTriggerEnter2D(Collider2D collision)
        {

        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if(transform.position == this.hitPoints[_randSpot].position)
            {
                /*                Debug.Log("birdHit" + collision.name);
                */                //take health down if stayed
                                  //play animation
            }

        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            
        }        

        public void GetHit()
        {

                if(this._hitCounter < birdsStamina)
                {
                    this._hitCounter++;
                    Instantiate(_deathParticale, transform.position, transform.rotation);
            }
            else if (this._hitCounter >= birdsStamina)
                {
                isDead = true;
                StartCoroutine(playDeath());
            }
        }

        private void FlyAway()
        {
            if (!flyingToTarget)
            {
                flyAway = true;
            }

        }

    }
}

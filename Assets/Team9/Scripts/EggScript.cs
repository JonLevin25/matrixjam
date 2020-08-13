using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MatrixJam;

namespace MatrixJam.Team9
{
    public class EggScript : MonoBehaviour
    {
        public ParalaxController Paralax;

        public float MaxSpeed;
        public float Acceleration;

        public float _verticalSpeed;
        private float _currentSpeed = -10f;

        public bool _gamesStarted = false;

        public ParachuteScript _parachute;

        public bool _flagEnded = false;

        public bool _win = false;


        public Transform movewhenSqash;

        public GameObject SplashSprite;

        public GameObject EggSprite;

        public GameObject PillowSprite;

        public bool playEndScreen = false;

        private Animator anim;

        public bool startPopanim;

        public bool stopMoveanim;

        public AudioClip Squeesh;

        public AudioClip freeFalling;

        public AudioClip winSong;

        private AudioSource AudioSourceEgg;


        // Start is called before the first frame update

        private void Awake()
        {
        }

        void Start()
        {
            playEndScreen = false;
            _currentSpeed = 0;
            anim = GetComponent<Animator>();
            AudioSourceEgg = GetComponent<AudioSource>();

        }

        // Update is called once per frame
        void Update()
        {
            if(startPopanim)
            {
                anim.SetBool(("PopIn"),true);
            }
            if(stopMoveanim)
            {
                anim.SetBool(("StopMoving"), true);
            }
            if (_gamesStarted && !_flagEnded)
            {
                anim.SetBool(("StartMoving"), true);
                _currentSpeed += Acceleration * Time.deltaTime;
                _verticalSpeed += _currentSpeed * Time.deltaTime;
                Paralax.Position -= _currentSpeed * Time.deltaTime;
            }

            _currentSpeed = Mathf.Clamp(_currentSpeed, -MaxSpeed, MaxSpeed);

            if(_parachute._health <= 0 && !_flagEnded)
            {
                AudioSourceEgg.PlayOneShot(freeFalling, 0.05f);
            }

            StartCoroutine(ChangeSprite());



        }



        IEnumerator ChangeSprite()
        {
            while (!_flagEnded)
            {
                yield return null;
            }
            yield return new WaitForSeconds(0.1f);
            if (_flagEnded && !_win)
            {
                EggSprite.SetActive(false);                
                SplashSprite.SetActive(true);
            }


        }

        public void PlayendGame()
        {
            //need to change the speed of crash to variable
            _currentSpeed += Acceleration * 2 * Time.deltaTime;
            _verticalSpeed += _currentSpeed * 2 * Time.deltaTime;
            Paralax.Position -= _currentSpeed * 2 * Time.deltaTime;
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.tag == "Finish")
            {
                AudioSourceEgg.Stop();
                transform.position = Vector3.MoveTowards(transform.position, movewhenSqash.position, 1000f * Time.deltaTime);
                
                if(_parachute._health <= 0)
                {
                    AudioSourceEgg.PlayOneShot(Squeesh);
                    AudioSourceEgg.Play();
                    _flagEnded = true;
                    this.playEndScreen = true;
                }
                else if (_parachute._health > 0)
                {
                    _parachute.StopMusic();
                    AudioSourceEgg.PlayOneShot(winSong, 0.09f);
                    _flagEnded = true;
                    _win = true;
                    _parachute.gameObject.SetActive(false);
                }

            }
        }        

    }
}

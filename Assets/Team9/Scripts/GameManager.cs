using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Events;
using MatrixJam;


namespace MatrixJam.Team9
{
    //we need to count score for each building we leave
    //we will handle the game movement here so referece to all so birds will fly faster and with less delay
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private int exitNum;

        [SerializeField] private UnityEvent exitEvent;

        public ParachuteScript ParachuteRef;
        public EggScript EggRef;
        public BirdGenerator BirdGeneratorRef;
        public GameObject BirdRef;

        private AudioSource _gameaudiosource;

        public float HowMuchToaccelerate;
        public float HowMuchMaxSpeed;

        [SerializeField] private HealthBar healthBar;
        [SerializeField] private float HelathReduce;

        [SerializeField] private DistanceCalc Distance;

        [SerializeField] private GameObject StartScene;
        [SerializeField] private GameObject Count1;
        [SerializeField] private GameObject Count2;
        [SerializeField] private GameObject Count3;
        [SerializeField] private GameObject WinScreen;
        [SerializeField] private GameObject GameOver;

        public GameOverTrig End1;
        public GameOverTrig End2;



        private bool eggFlagEnded;
        private bool parachuteFlagEnded;


        private float _distance;

/*        [SerializeField] private HealthBar distanceBar;
*/

        private float health = 100f;

        private bool _gameStat = false;

        private bool _tweakBirds = false;

        private bool _setPitch = false;

        // Start is called before the first frame update
        void Start()
        {
            _gameStat = true;
            eggFlagEnded = false;
            StartCoroutine(StartTheGame(EggRef, BirdGeneratorRef));
            StartCoroutine(CheckGame());
            _gameaudiosource = GetComponent<AudioSource>();


            /*             new WaitForSeconds(2);
                        _tweakBirds = true;

            */
        }


        IEnumerator StartTheGame(EggScript eggLogicScript, BirdGenerator birdGeneratorScript)
        {            
            //play start animation

            yield return new WaitForSeconds(3);
            StartScene.SetActive(false);
            Count3.SetActive(true);

            yield return new WaitForSeconds(1);
            Count3.SetActive(false);
            Count2.SetActive(true);

            yield return new WaitForSeconds(1);
            Count2.SetActive(false);
            Count1.SetActive(true);
            yield return new WaitForSeconds(1);
            Count1.SetActive(false);
            
            healthBar.showBar();
            Distance.showBarDistance();
            eggLogicScript.startPopanim = true;
            eggLogicScript._gamesStarted = true;
            birdGeneratorScript._gamesStarted = true;
            ParachuteRef.startWind();
        }

        IEnumerator Speedup1()
        {
            yield return StartCoroutine(Speedup2());
            
        }

        IEnumerator Speedup2()
        {
            yield return null;
            if (ParachuteRef.takeDamageHit)
            {
                yield return null;
                EggRef.Acceleration += HowMuchToaccelerate;
                EggRef.MaxSpeed += HowMuchMaxSpeed;
                ParachuteRef.takeDamageHit = false;
                health -= HelathReduce;
                

                ParachuteRef.modifyWind(EggRef.Acceleration);
            }
        }
        // Update is called once per frame
        void Update()
        {
            
            StartCoroutine(Speedup1());
            /*            StartCoroutine(TwekBirds());
            */
            /*            if(ParachuteRef.takeDamageHit)
                        {
                            UnityEngine.Debug.Log("faster");

                            EggRef.Acceleration += 1;
                            EggRef.MaxSpeed += 1;
                        }

            */
            if (parachuteFlagEnded && !EggRef._win)
            {
                if (EggRef.playEndScreen)
                {
                    GameOver.SetActive(true);
                }
            }


        }

        IEnumerator CheckGame()
        {
            yield return StartCoroutine(checkStatus());            
        }

        IEnumerator checkStatus()
        {
            while (!eggFlagEnded && !parachuteFlagEnded)
            {
                eggFlagEnded = EggRef._flagEnded;
                parachuteFlagEnded = ParachuteRef._flagEnded;
                yield return null;
            }
            if(eggFlagEnded && EggRef._win)
            {                
                WinScreen.SetActive(true);

                if(ParachuteRef._health == 100)
                {
                    End1.ExitInvoke();
                }
                else
                {
                    End2.ExitInvoke();
                }
            }

            healthBar.HideBar();
            EggRef.stopMoveanim = true;
            BirdGeneratorRef._gamesStarted = false;
            BirdRef.GetComponent<BirdScript>().abort = true;
            EggRef.PlayendGame();
            yield return new WaitForSeconds(6);
            if (EggRef._flagEnded && !EggRef._win)
            {
                LevelHolder.Level.Restart();
            }

        }

        IEnumerator TwekBirds()
        {

            while (_tweakBirds)
            {
                yield return new WaitForSeconds(5);
                _tweakBirds = false;
                /*                BirdGeneratorRef.spawnDelayStart = 1f;
                                BirdGeneratorRef._waitbeforeLaunch = 1;
                */
                BirdGeneratorRef.FireAway();
            }


        }
    }
}

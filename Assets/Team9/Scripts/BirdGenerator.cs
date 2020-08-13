using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team9
{
    public class BirdGenerator : MonoBehaviour
    {
        [SerializeField] public GameObject _birdPrefab;

        public int numberOfBirds;

        private float spawnDelay;

        public float spawnDelayStart;

        private List<GameObject> _instantiateList;

        public Transform startingPointLeft;
        public Transform startingPointRight;

        public List<Transform> waysIn;

        public bool _gamesStarted = false;


        public int _waitbeforeLaunch;

        public int birdCounter;


        void Start()
        {

                StartCoroutine(LaunchBirds());
                spawnDelay = spawnDelayStart;
        }

        IEnumerator LaunchBirds()
        {
            while (!_gamesStarted)
            {
                yield return null;

                if (_gamesStarted)
                {
                    bool _side = false;

                    //makes sure they match length
                    for (int i = 0; i < numberOfBirds; i++)
                    {

                        //instantiate on left = false, 
                        if (_side)
                        {
                            if(_gamesStarted)
                            {
                                birdCounter++;
                                Transform randomIndex = waysIn[Random.Range(0, 2)];
                                GameObject bird = Instantiate(_birdPrefab, new Vector3(randomIndex.position.x, randomIndex.position.y, randomIndex.position.z), Quaternion.Euler(new Vector3(0,180f,0)));
                            }
                        }

                        else if (!_side) //instantiate on right = true;
                        {
                            if (_gamesStarted)
                            {
                                birdCounter++;
                                Transform randomIndex = waysIn[Random.Range(3, 5)];
                                GameObject bird = Instantiate(_birdPrefab, new Vector3(randomIndex.position.x, randomIndex.position.y, randomIndex.position.z), Quaternion.identity);
                            }
                        }

                        _side = ShuffleBool();

                        if (birdCounter == 4)
                        {
                            FireAway();
                        }

                        if (birdCounter == 7)
                        {
                            FireAway();
                        }

                        if (birdCounter == 15)
                        {
                            FireAway();
                        }

                        //add start and stuff
                        yield return new WaitForSeconds(spawnDelay);
                    }
                }
                /*            hasFinished = true;
                */
            }

        }
        // Update is called once per frame
        void Update()
        {
        }

        private bool ShuffleBool()
        {
            if (Random.Range(0, 3) == 0) return false;
            else return true;
        }

        public void FireAway()
        {

            //makes sure they match length              
            GameObject bird = Instantiate(_birdPrefab, new Vector3(startingPointLeft.position.x, startingPointLeft.position.y, startingPointLeft.position.z), Quaternion.Euler(new Vector3(0, 180f, 0)));
            new WaitForSeconds(0.4f);
            GameObject bird3 = Instantiate(_birdPrefab, new Vector3(startingPointLeft.position.x, startingPointLeft.position.y, startingPointLeft.position.z), Quaternion.Euler(new Vector3(0, 180f, 0)));
            GameObject bird2 = Instantiate(_birdPrefab, new Vector3(startingPointRight.position.x, startingPointRight.position.y, startingPointRight.position.z), Quaternion.identity);
        }
    }
}

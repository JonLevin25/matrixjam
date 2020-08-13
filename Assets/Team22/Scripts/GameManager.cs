using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace MatrixJam.Team22
{
    public class GameManager : MonoBehaviour
    {
        // stats
        private int hits = 0;
        private int misses = 0;
        // tutorial
        [Header("Tutorial")]
        [SerializeField]
        private bool tutorial = true;
        public GameObject tutorialUI;
        public Text tutorialHitsLeft;
        public int requiredTutorialHits = 6;
        public GameObject tutorialTimeline;
        public AudioClip tutorialComplete;
        // game
        [Header("Game")]
        public GameObject gameTimeline;
        public Text gameTotalHits;
        public int allowedFails = 5;
        // exits
        // exits are triggered in DialogueManager, depending on the result parsed in EndGame() here
        [Header("Exits")]
        public UnityEvent epicFailExit;
        public UnityEvent failExit;
        public UnityEvent winExit;
        // ui
        [Header("Dev")]
        public Text devText;


        private AudioSource source;
        public static GameManager instance;

        private void Awake()
        {
            instance = this;
            source = GetComponent<AudioSource>();

            //Setup();
        }

        public void Setup()
        {
            tutorialTimeline.SetActive(false);
            gameTimeline.SetActive(false);
            gameTotalHits.text = "";
            tutorialUI.SetActive(false);
        }

        public void Update()
        {
            if(devText != null)
            {
                devText.text = "HITS " + hits + "\nMISSES " + misses;
            }

            if(tutorial)
            {
                tutorialHitsLeft.text = (requiredTutorialHits - hits).ToString();
                if (hits >= requiredTutorialHits)
                {
                    tutorial = false;
                    source.PlayOneShot(tutorialComplete);
                    tutorialTimeline.SetActive(false);

                    Invoke("PostTutorialDialogue", 2f);
                }
            }
        }

        public void UpdateStats(int addHits, int addMiss)
        {
            hits += addHits;
            misses += addMiss;

            if (gameTotalHits != null && !tutorial)
            {
                gameTotalHits.text = hits.ToString("D2");
            }
        }

        public void StartTutorial()
        {
            tutorialTimeline.SetActive(true);
            tutorialUI.SetActive(true);
        }

        public void StartGame()
        {
            gameTimeline.SetActive(true);
            hits = 0;
            misses = 0;
        }

        public void EndGame()
        {
            // 1 or less hits
            if(hits <= 1)
                DialogueManager.instance.EnableDialogues(1);

            // not enough hits
            else if (misses > allowedFails)
                DialogueManager.instance.EnableDialogues(2);

            // okay you win
            else
                DialogueManager.instance.EnableDialogues(3);
        }

        void PostTutorialDialogue()
        {
            tutorialUI.SetActive(false);
            DialogueManager.instance.EnableDialogues(0);
        }

        public UnityEvent GetExit(int id)
        {
            if (id == 1)
                return epicFailExit;
            else if (id == 2)
                return failExit;
            else
                return winExit;
        }

        public int GetMisses()
        {
            return misses;
        }
    }
}

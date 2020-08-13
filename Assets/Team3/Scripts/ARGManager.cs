using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace MatrixJam.Team3
{
    public class ARGManager : MonoBehaviour
    {
        [SerializeField] private bool NextCharacter;
        [SerializeField] private bool YoutubeEnabled;
        [SerializeField] private bool TextEnabled;
        [SerializeField] private TextMeshProUGUI textToDisplay; 
        
        [SerializeField] private GameObject currentlEnabled;
        [SerializeField] private string currentlEnabledString;
        [SerializeField] private Dictionary<string, GameObject> Stages;
        [SerializeField] private Dictionary<string, string> Descriptions;
        [SerializeField] private Dictionary<string, bool> forceTexts;
        [SerializeField] private Dictionary<string, bool> forceImages;
        [SerializeField] private Dictionary<string, Sprite> BGImages;
        [SerializeField] private Dictionary<string, AudioClip> audioClips;

        [SerializeField] private Sprite loginScreenImage;
        
        [SerializeField] private InputField _inputField; 
        [SerializeField] private Image backgroundImage;
        [SerializeField] private Sprite BlankImage;
        
        [SerializeField] private UnityEvent [] exitEvent;

        [SerializeField] private AudioSource musicPlayer;
        
        [Serializable]
        public struct StageItem {
            public string name;
            public GameObject theObject;
            public string description;
            public bool forceText;
            public bool forceImage;
            public Sprite BGImage;
            public AudioClip BGmusic;
        }
        
        [SerializeField] private List<StageItem> StagesList;
        
        [SerializeField] public List<GameObject> InitialObjects;
        
        [SerializeField] public List<int> levels;
        [SerializeField] private int currentLevel;
        [SerializeField] private int numLevels =0;
        [SerializeField] private int MaxLevels =5;
        
        // Start is called before the first frame update
        void Start()
        {
            if (Stages == null)
            {
                CreateStages();
            }            
           
        }

        public void StartLevel(int level)
        {
            if (levels == null)
            {
                levels = new List<int>();
                for (int i = 0; i < MaxLevels; i++)
                {
                    levels.Add((level + i) % MaxLevels);
                }
            }

            //if (LevelHolder.PlayPlayed() > 0) {
            // TextEnabled = true;
            // }

        currentLevel = level;
            ChangeItem("000" + currentLevel);
        }
        
        private void CreateStages()
        {
            Stages = new Dictionary<string, GameObject>();
            Descriptions = new Dictionary<string, string>();
            forceTexts = new Dictionary<string, bool>();
            forceImages = new Dictionary<string, bool>();
            BGImages = new Dictionary<string, Sprite>();
            audioClips= new Dictionary<string, AudioClip>();
            foreach ( StageItem item in StagesList)
            {
                
                Stages[item.name.ToLower()] = item.theObject;
                Descriptions[item.name.ToLower()] = item.description;
                forceTexts[item.name.ToLower()] = item.forceText;
                forceImages[item.name.ToLower()] = item.forceImage;
                BGImages[item.name.ToLower()] = item.BGImage;
                audioClips[item.name.ToLower()] = item.BGmusic;
            }
            
            
        }


        public void ChangeItem(string item)
        {
            if (item.ToLower() == "quit")
            {
                Application.Quit();
            }
            /*foreach (GameObject go in InitialObjects)
            {
                go.SetActive(false);
            }*/

            if (Stages == null)
            {
                CreateStages();
            }
            if (Stages.ContainsKey(item.ToLower())){
                currentlEnabled.SetActive(false);
                currentlEnabled = Stages[item.ToLower()];
                currentlEnabledString = item.ToLower();
                _inputField.text = "";

                backgroundImage.sprite = BlankImage;
                textToDisplay.text = "";
                
                if (TextEnabled || forceTexts[item.ToLower()])
                {
                    textToDisplay.text = Descriptions[item.ToLower()];
                    if (BGImages[item.ToLower()] != null)
                    {
                        backgroundImage.sprite = BGImages[item.ToLower()];
                    }
                }
                if (!TextEnabled || forceImages[item.ToLower()])
                {
                    currentlEnabled.SetActive(true);
                }
                
                if (audioClips.ContainsKey(item.ToLower()))
                {
                    if (audioClips[item.ToLower()] != null)
                    {
                        Debug.Log("Playing Clip " + audioClips[item.ToLower()] + " on " + Stages[item.ToLower()]);
                        musicPlayer.clip = audioClips[item.ToLower()];
                        musicPlayer.Play();
                    }
                } 
                
                
            }
        }


        public void handleExit(int exit_num)
        {
            if (NextCharacter)
            {
                if (numLevels++ < MaxLevels -1)
                {
                    currentLevel = (currentLevel + 1 ) % MaxLevels;
                    TextEnabled = true;
                    ChangeItem("000" + currentLevel);
                    return;
                }

                Debug.Log("numlevels =" + numLevels + "maxlelevs   = " + (MaxLevels ) );
                if (YoutubeEnabled && numLevels == MaxLevels )
                {
                    return;
                }
            }
            
            
            // do not move to the next character
            exitEvent[exit_num].Invoke();
            
        }

        public void OpenWebsite(string URL)
        {
            musicPlayer.Stop();
            currentlEnabled.GetComponent<Image>().sprite = loginScreenImage;
            Application.OpenURL(URL);
        }
        // Update is called once per frame
        void Update()
        {
            
        }
    }
}

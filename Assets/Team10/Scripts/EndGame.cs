using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace MatrixJam.Team10
{
    public class EndGame : MonoBehaviour
    {
        [SerializeField] private int exitNum;

        [SerializeField] private UnityEvent[] exitEvent;

        public GameObject Panel;
        public Text PanelTitle;
        public Text PanelMessage;
        public GameObject PanelInput;
        public Text PlayerName;
        public Button PanelButton;
        public Player[] players;
        public float typingSpeed;

        private List<string[]> DeathList;
        private string Name;
        private string currDialogue;

        void Start(){
            DeathList = DeathListGen();
        }
        
        public void cinema(string game){
            Panel.SetActive(true);
        }

        public void leave(){
            Panel.SetActive(false);
        }

        public void startScene(){
            Panel.SetActive(true);
            PanelTitle.gameObject.SetActive(true);
            PanelMessage.gameObject.SetActive(true);
            PanelInput.SetActive(true);
            PanelButton.gameObject.SetActive(true);

            PanelTitle.text = "a day in a life - 2020 edition";
            PanelButton.GetComponentInChildren<Text>().text = "start";
            PanelButton.onClick = new Button.ButtonClickedEvent();
            PanelButton.onClick.AddListener(delegate { 
                onStart();
            });
            StartCoroutine(TypeMessegeAffect(new string[] {"just another day in life...",
                "living with roommates", "and playing around", "waiting for 2021..", "hopefully it will be better then now..."}));
        }

        private bool isSpecialCharacter(string playerName){
            string[] names = new string[] {"BATMAN", "CORONA", "SUPERMAN", "42", "MATRIX", "MAYA", "MIKA", "RAUL"};
            return System.Array.IndexOf(names, playerName) != -1;
        }

        private void onStart(){
            Name = PlayerName.text.ToUpper();
            if(Name == ""){
                Name = "my parents didn't name me";
                System.Array.Find(players, (p) => p.name == "PLAYER").gameObject.SetActive(true);
            }
            else if(isSpecialCharacter(Name)){
                System.Array.Find(players, (p) => p.name == Name).gameObject.SetActive(true);
            }
            else{
                System.Array.Find(players, (p) => p.name == "PLAYER").gameObject.SetActive(true);
            }

            RandomDialogueTree tr = new RandomDialogueTree(Name);
            GameRules g = FindObjectOfType<GameRules>();
            g.t = tr;
            g.playerName = Name;

            //and activate startDialogue
            Panel.SetActive(false);
            PanelInput.gameObject.SetActive(false);
            PanelTitle.gameObject.SetActive(false);
            PanelButton.gameObject.SetActive(false);
            PanelMessage.gameObject.SetActive(false);
            g.DialogueMenu(tr.getStarterDialogue(Name));
        }

        public void testDeath(int id){
            Panel.SetActive(true);
            PanelMessage.gameObject.SetActive(true);
            PanelButton.gameObject.SetActive(true);
            PanelButton.onClick = new Button.ButtonClickedEvent();
            PanelButton.GetComponentInChildren<Text>().text = "Next";
            PanelButton.onClick.AddListener(delegate { 
                testDeath(id+1);
            });
            DeathList = DeathListGen();
            StartCoroutine(TypeMessegeAffect(DeathList[id]));
        }

        private bool isRoommate(){
            string[] names = new string[] {"MAYA", "MIKA", "RAUL"};
            return System.Array.IndexOf(names, Name) != -1;
        }

        private bool isBossCorona(){
            string[] names = new string[] {"BATMAN", "CORONA"};
            return System.Array.IndexOf(names, Name) != -1;
        }

        public void DeathScene(int deathId){
            Panel.SetActive(true);
            PanelMessage.gameObject.SetActive(true);
            PanelButton.gameObject.SetActive(true);

            exitNum = deathId;
            DeathSceneDisplay(deathId);
        }

        private void DeathSceneDisplay(int deathId){
            PanelButton.onClick = new Button.ButtonClickedEvent();
            if((deathId < 4 || deathId > 9) && (deathId < 12) && (isBossCorona() || isRoommate())){
                PanelButton.GetComponentInChildren<Text>().text = "Next";
                PanelButton.onClick.AddListener(delegate { 
                    Continue();
                });
            }
            else{
                PanelButton.GetComponentInChildren<Text>().text = "move on";
                PanelButton.onClick.AddListener(delegate { 
                    moveOn();
                });
            }
            StartCoroutine(TypeMessegeAffect(DeathList[deathId]));
        }

        private void Continue(){
            if(!currDialogue.Equals(PanelMessage.text)){
                return;
            }
            if(isBossCorona()){
                DeathSceneDisplay(12);
                return;
            }
            DeathSceneDisplay(13);
        }
        private void moveOn(){
            Debug.Log(exitNum);
            exitEvent[exitNum].Invoke();
        }

        IEnumerator TypeMessegeAffect(string[] sentences){
            PanelMessage.text = "";
            currDialogue = System.String.Join("\n", sentences) + "\n";
            foreach(string sentence in sentences){
                foreach (char letter in sentence.ToCharArray())
                {
                    PanelMessage.text += letter;
                    yield return new WaitForSeconds(typingSpeed);
                }
                PanelMessage.text += "\n";
            }
        }

        private List<string[]> DeathListGen(){
            List<string[]> a = new List<string[]>();
            //clean - did not get cleaned as needed
            a.Add(new string[] { "you dirty maggot! don't you know it's 2020!!!", "you should, at the very least, wash your hands.",
                "now look, you got the corona virus. are you happy?","well, too bad, we dont care.","anyways, you died..."});
            //hunger - not eating
            a.Add(new string[] { "just because it's a game,", "it does not mean you can starve your character.",
                "poor <" + Name + ">, it died of starvation...." , "and so young..." , "btw", "that's also means you died" });
            //work - getting fired
            a.Add(new string[] { "you do realize you need to work in order to pay rent, rrigth???", 
                "since you didn't work, you just got fired, couldn't pay your bills and now you are homeless... with corona... and dead...", 
                "obviously, since we gave up on you",
                "cause you don't work and we don't like that...", "welph, at least it didnt happen in real life, rightt?" });
            //killOrder - rommate dont like the way you talk
            a.Add(new string[] {"your roommates got annoyed with you", "and decieded to throw you out of the apartment...",
                "welll.... you know the drill...", "you met a corona zombie, got infected and died.", "now go away...", "i'm trying to take a nap here....."});
            //paranoia - reapeting similar actions 6 times
            a.Add(new string[] { "you are seriously paranoid and should get that checked...",
                "the psychologist says repeated actions is a serious case of paranoia and in our world that's a good enough reason to die", "you died!" });
            //win
            a.Add(new string[] { "congratulations!", "you made it till the end", "but just to make sure, you didn't cheat right?",
                "i mean, we made it extremely difficult, you see...", "anyways, you won - you, your roommate, and by default us as well",
                "we have other ending, you should check it out" });
            //sleep (repeat)
            a.Add(new string[] {"zzzzzz", "we love sleeping, don't we?", 
                "it's such a lovely thing", "but guess what?",
                "like in real life, you can also die here from oversleeping",
                "like we said.... you died"});
            //hide (repeat)
            a.Add(new string[] {"life is such a mysterious thing", 
                "you see... some things are necessary in life even if we think otherwise",
                "one of them is the air we breathe",
                "so you died (of suffocation) ... since you didn't want to come out of the closet...."});
            //play (repeat)
            a.Add(new string[] {"wow...", "even inside a game, you are totally binge gaming...",
                "this is trully the case of a character taking after it's player",
                "go on - move on to the next game", "we won't stop you"});
            //world crash (instaDeath)
            a.Add(new string[] {"we have an existentail crisis here",
                "you have done something others wouldn't dare!!", 
                "due to your thoughtless action, the entire worlds is now collapsing",
                "way to go, bro... you just killed everyone else with you",
                "are you happy?"});
            // ---------------- 10 ---------------------
            //leave house (instaDeath)
            a.Add(new string[] {"excuse me?", "did you think it's 2019,",
                "that you can just leave the house? no way!",
                "you got corona now", "andd~ you died"});
            // matrix - 2 pills
            a.Add(new string[] {"you died from pill overdose","just because it's still there",
                "does not mean you should take it"});
            // corona \ batman - special end
            a.Add(new string[] {"Boss corona", "we concoured the world", "everyone is infected and zombiefied",
                "good luck in your next mission" });
            // roomate - special end
            a.Add(new string[] {"THIS IS A FARCE.",
            "you died but you got a clone in another room so in the end you did survive.", "minus one clone though"});
            return a;
        }
    }
}

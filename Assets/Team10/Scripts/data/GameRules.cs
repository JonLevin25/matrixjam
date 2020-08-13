using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.Video;

namespace MatrixJam.Team10
{
    public class GameRules : MonoBehaviour
    {
        #region params
        [SerializeField] private int exitNum;

        [SerializeField] private UnityEvent[] exitEvent;

        private int deathCheck;
        private int repeat;
        private string lastActionID;
        private bool takenPill;

        private int cleanFactor;
        private int hungerFactor;
        private double workFactor;
        private int killFactor;
        private List<string[]> deathList;

        public List<Choice> choices;
        public System.DateTime time = System.DateTime.Parse("9:00 AM");
        public Text Timer;

        public GameObject Panel;
        public Text PanelMessage;
        public Button PanelButton;

        public GameObject vidPanel;
        public VideoPlayer[] vids;
        
        public RandomDialogueTree t;
        public string playerName;
        public bool isDead;
        public float typingSpeed;
        #endregion
        
        void Start(){
            t = new RandomDialogueTree("player1");
            declareChoices();
            deathList = DeathListGen();
            Panel.SetActive(false);
            PanelMessage.gameObject.SetActive(false);
            PanelButton.gameObject.SetActive(false);

            foreach(VideoPlayer vd in vids){
                vd.loopPointReached += (VideoPlayer v) => {
                    vidPanel.SetActive(false);
                };
            }
        }

        void Update(){
            time = time.AddSeconds((int)(Time.deltaTime*50));
            Timer.text = time.ToString("hh:mm tt");
            //update time display
            if((time.Hour > 12 && deathCheck < 1) || (time.Hour > 6 && deathCheck < 2)) //12-start noon, 18-start evening
            {
                Debug.Log(deathCheck);
                checkDeath();
                deathCheck += 1;
            }
            else if((time.Hour > 23 || time.Hour < 4) && deathCheck == 2){ //passed to the next day
                Debug.Log("end");
                deathCheck += 1;
                endOfDay();
            }
        }

        public void OnExit(){
            Debug.Log(exitNum);
            exitEvent[exitNum].Invoke();
        }
        #region Game Death

        public List<string[]> DeathListGen(){
            List<string[]> a = new List<string[]>();
            //clean - did not get cleaned as needed
            a.Add(new string[] { "you dirty maggot! don't you know it's 2020!!!", "you should, at the very least, wash your hands.",
                "now look, you got the corona virus. are you happy?","well, too bad, we dont care.","anyways, you died..."});
            //hunger - not eating
            a.Add(new string[] { "just because it's a game,", "it does not mean you can starve your character.",
                "poor <name>, it died of starvation...." , "and so young..." , "btw", "that's also means you died" });
            //work - getting fired
            a.Add(new string[] { "you do realize you need to work in order to pay rent, rright???", "cause you got fired, couldn't pay your bills",
                "and now you are homeless... with corona... and dead...", "obviously, since we gave up on you",
                "cause you don't work and we don't like that...", "welph, at least it didnt happen in real life, rightt?" });
            //killOrder - rommate dont like the way you talk
            a.Add(new string[] {"your roommates got annoyed with you", "and decieded to throw you out of the apartment...",
                "welll.... you know the drill...", "you met a corona zombie, got infected and died.", "now go away...", "i'm trying to take a nap here....."});
            //paranoia - reapeting similar actions 6 times
            a.Add(new string[] { "you are seriously paranoid and should get that checked...",
                "anyways, this paranoia of yours didn't help and you still got the corona virus.", "you died!" });
            //win
            a.Add(new string[] { "congratulations!", "you made it till the end", "but just to make sure, you didn't cheat right?",
                "i mean, we made it extremely difficult, you see...", "anyways, you won - you, your roommate, and ect.",
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
                "so you died... since you didn't want to come out of the closet...."});
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

        //called by all actions with action id - check if action repeated
        //and do reapeat stuff if true -  modifies the repeted varibles and calls to end if > num?
        public void CheckRepeat(string id){
            if(lastActionID != id){
                repeat = 0;
                return;
            }
            repeat += 1;
            if(repeat > 2){
                if(playerName == "BATMAN" || playerName == "CORONA"){
                    DeathScreen(12);
                    return;
                }
                else if(isRoommate()){
                    DeathScreen(13);
                    return;
                }
                switch (lastActionID)
                {
                    case "sleep":
                    case "nap":
                        DeathScreen(6);
                        break;
                    case "hide":
                        DeathScreen(7);
                        break;
                    case "play":
                        DeathScreen(8);
                        break;
                    default:
                        DeathScreen(4);
                        break;
                }
            }
        }
        public bool canCharacterAvoidDeath(){
            string[] names = new string[] {"BATMAN", "CORONA", "SUPERMAN"};
            return System.Array.IndexOf(names, playerName) != -1 || isRoommate();
        }
        public bool isRoommate(){
            string[] names = new string[] {"MAYA", "MIKA", "RAUL"};
            return System.Array.IndexOf(names, playerName) != -1;
        }
        public void checkDeath(){
            if(canCharacterAvoidDeath() && killFactor > 3){
                DeathScreen(3);
            }
        }

        public void endOfDay(){
            //private int cleanFactor; // if < 3 kill - end of day only
            //private int hungerFactor; // must eat 2 meals during the day if counter < 2 kill - end of day
            //private int workFactor;  // must reach 3 point by the end of the day to survive - end of day
            //private int killFactor; // 3 will kill 
            if(playerName == "BATMAN" || playerName == "CORONA"){
                DeathScreen(12);
            }
            else if(cleanFactor < 5){
                if(isRoommate()){
                    DeathScreen(13);
                }
                else{
                    DeathScreen(0);
                }
            }
            else if(hungerFactor < 2){
                if(isRoommate()){
                    DeathScreen(13);
                }
                else{
                    DeathScreen(1);
                }
            }
            else if(workFactor < 3){
                if(isRoommate()){
                    DeathScreen(13);
                }
                else{
                    DeathScreen(2);
                }
            }
            else if(canCharacterAvoidDeath() && killFactor > 3){
                if(isRoommate()){
                    DeathScreen(13);
                }
                else{
                    DeathScreen(3);
                }
            }
            else{
                // survive
                DeathScreen(5);
            }
        }

        IEnumerator TypeMessegeAffect(string[] sentences){
            foreach(string sentence in sentences){
                foreach (char letter in sentence.ToCharArray())
                {
                    PanelMessage.text += letter;
                    yield return new WaitForSeconds(typingSpeed);
                }
                PanelMessage.text += "\n";
            }
        }

        public void DeathScreen(int id){
            exitNum = id;
            isDead = true;
            PanelMessage.gameObject.SetActive(true);
            PanelButton.gameObject.SetActive(true);
            PanelMessage.text = "";
            Panel.SetActive(true);
            StartCoroutine(TypeMessegeAffect(deathList[id]));
        }
        #endregion

        public void DialogueMenu(Dialogue dialogue){
            FindObjectOfType<DialogueManager>().StartActionChoice(dialogue);
        }
        public void DialogueMenu(DialogueTree dialogue){
            FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
        }
        
        #region Choices Defenitions 

        public void declareChoices(){
            choices = generalActions(); //10
            choices.AddRange(RoomActions()); //11
            choices.AddRange(BathRoomActions()); //4
            choices.AddRange(KitchenActions());//5
            choices.AddRange(LivingRoomActions());//6
            choices.AddRange(DialogueChoiceActions());//28
        }

        // actions that can be accessed from more then one room - 10
        public List<Choice> generalActions(){
            List<Choice> actions = new List<Choice>();
            // bed + couch
            actions.Add(new Choice("sleep", () => {
                CheckRepeat("sleep");
                time = time.AddMinutes(60);
                lastActionID = "sleep";
            }));
            actions.Add(new Choice("nap", () => {
                CheckRepeat("nap");
                time = time.AddMinutes(30);
                lastActionID = "nap";
            }));
            actions.Add(new Choice("jump", () => {
                CheckRepeat("jump");
                time = time.AddMinutes(30);
                lastActionID = "jump";
            }));

            // tv + pc
            actions.Add(new Choice("play games", () => {
                CheckRepeat("play");
                Dialogue a = new Dialogue("I should play...", new int[] {6, 7, 8});
                DialogueMenu(a);
                lastActionID = "play";
            }));
            actions.Add(new Choice("netflix&chill", () => {
                time = time.AddMinutes(60);
                lastActionID = "watch";
            }));
            // bathroom + kitchen
            actions.Add(new Choice("wash hands", () => {
                CheckRepeat("wash");
                time = time.AddMinutes(5);
                if(lastActionID == "number2")
                {
                    cleanFactor = cleanFactor +1;
                }
                lastActionID = "wash";
            }));
            //game-followup
            actions.Add(new Choice("MatrixJam", () => {
                time = time.AddMinutes(45);
                vidPanel.SetActive(true);
            }));
            actions.Add(new Choice("A day in life 2020", () => {
                DeathScreen(9);
            }));
            actions.Add(new Choice("fart in a jar", () => {
                time = time.AddMinutes(30);
                vids[1].Play();
                vidPanel.SetActive(true);
            }));
            actions.Add(new Choice("4", () => {
                time = time.AddMinutes(15);
                vidPanel.SetActive(true);
            }));
            return actions;
        }

        // total: 11
        private List<Choice> RoomActions(){
            List<Choice> actions = new List<Choice>();
            //bed
            actions.Add(new Choice("profess love", () => {
                CheckRepeat("loveBed");
                time = time.AddMinutes(60);
                lastActionID = "loveBed";
            }));
            //closet
            //black screen - options to hide(repeat) or leave
            actions.Add(new Choice("change clothes", () => {
                CheckRepeat("clothes");
                time = time.AddMinutes(10);
                lastActionID = "clothes";
            }));
            actions.Add(new Choice("hide", () => {
                time = time.AddMinutes(30);
                Panel.SetActive(true);
                Dialogue a = new Dialogue("<inside the Closet>", new int[] {13, 14});
                DialogueMenu(a);
                lastActionID = "hide";
            }));
            //hide-followup
            actions.Add(new Choice("come out of the closet", () => {
                Panel.SetActive(false);
                killFactor -= 1;
                lastActionID = "LGBT";
            }));
            actions.Add(new Choice("stay", () => {
                CheckRepeat("hide");
                time = time.AddMinutes(30);
                Dialogue a = new Dialogue("<inside the Closet>", new int[] {13, 14});
                DialogueMenu(a);
                lastActionID = "hide";
            }));
            //pc
            actions.Add(new Choice("work", () => {
                CheckRepeat("work");
                Dialogue a = new Dialogue("work for...", new int[] {17, 18, 19, 20});
                DialogueMenu(a);
                lastActionID = "work";
            }));
            actions.Add(new Choice("call friend", () => {
                CheckRepeat("call");
                DialogueMenu(t.getCallFriendDialogue());
                lastActionID = "call";
            }));
            //work-followup
            actions.Add(new Choice("30 Min", () => {
                time = time.AddMinutes(30);
                workFactor += 0.5;
            }));
            actions.Add(new Choice("1 Hour", () => {
                time = time.AddMinutes(60);
                workFactor += 1;
            }));
            actions.Add(new Choice("2 Hours", () => {
                time = time.AddMinutes(30*4);
                workFactor += 2;
            }));
            actions.Add(new Choice("4 Hours", () => {
                time = time.AddMinutes(30*8);
                workFactor += 4;
            }));
            return actions;
        }

        // total: 4
        private List<Choice> BathRoomActions(){
            List<Choice> actions = new List<Choice>();
            //toilet
            actions.Add(new Choice("number 2", () => {
                CheckRepeat("number2");
                time.AddMinutes(30);
                lastActionID = "number2";
            }));
            //shower + sink
            actions.Add(new Choice("shower", () => {
                CheckRepeat("shower");
                time.AddMinutes(30);
                lastActionID = "shower";
            }));
            actions.Add(new Choice("bubble bath", () => {
                CheckRepeat("shower");
                time.AddMinutes(120);
                lastActionID = "shower";
            }));
            actions.Add(new Choice("swim", () => {
                CheckRepeat("swim");
                Dialogue a = new Dialogue("playerName", "ouch! this is no place to swim...");
                DialogueMenu(a);
                cleanFactor += 1;
                time.AddMinutes(30);
                lastActionID = "swim";
            }));
            return actions;
        }

        // total: 5
        private List<Choice> KitchenActions(){
            List<Choice> actions = new List<Choice>();
            //ref
            actions.Add(new Choice("eat something", () => {
                CheckRepeat("eat");
                if(playerName == "MATRIX"){
                    Dialogue a = new Dialogue("", new int[] {28, 29});
                    DialogueMenu(a);
                }
                else{
                    time.AddMinutes(30);
                    hungerFactor += 1;
                    if(lastActionID == "wash"){
                        cleanFactor += 1;
                    }
                }
                lastActionID = "eat";
            }));
            actions.Add(new Choice("drink something", () => {
                CheckRepeat("drink");
                time.AddMinutes(5);
                lastActionID = "drink";
            }));
            //sink
            actions.Add(new Choice("do the dishes", () => {
                CheckRepeat("dishes");
                time.AddMinutes(10);
                lastActionID = "dishes";
            }));
            //ref-followup
            actions.Add(new Choice("blue pill", () => {
                if(takenPill){
                    DeathScreen(11);
                }
                else{
                    hungerFactor += 1;
                    cleanFactor += 1;
                    killFactor = 0;
                    takenPill = true;
                    lastActionID = "blue";
                }
            }));
            actions.Add(new Choice("red pill", () => {
                if(takenPill){
                    DeathScreen(11);
                    return;
                }
                Dialogue a = new Dialogue("Truth", "you need at least 3 clean, 3 work, 2 food and less then 3 angry roommates to survive");
                Dialogue b = new Dialogue("Truth", "currently: " + cleanFactor +" clean, "
                    + workFactor + " work, " + hungerFactor + " food and " + killFactor + " angry roommates");
                DialogueTree t = new DialogueTree(new Character("Truth"));
                t.addDialogue(a);
                t.addDialogue(b);
                DialogueMenu(t);
                takenPill = true;
                lastActionID = "red";
            }));
            return actions;
        } 

        // total: 6
        private List<Choice> LivingRoomActions(){
            List<Choice> actions = new List<Choice>();
            //tv
            actions.Add(new Choice("watch news", () => {
                CheckRepeat("news");
                time.AddMinutes(30);
                //maybe play video?
                lastActionID = "news";
            }));
            //couch
            actions.Add(new Choice("sit", () => {
                CheckRepeat("sit");
                time.AddMinutes(10);
                lastActionID = "sit";
            }));
            //magazine
            actions.Add(new Choice("read articles", () => {
                CheckRepeat("read");
                time.AddMinutes(15);
                lastActionID = "read";
            }));
            actions.Add(new Choice("read news", () => {
                CheckRepeat("read");
                time.AddMinutes(20);
                lastActionID = "read";
            }));
            actions.Add(new Choice("solve puzzles", () => {
                time.AddMinutes(40);
                lastActionID = "puzzle";
            }));
            //door
            actions.Add(new Choice("leave house", () => {
                DeathScreen(10);
            }));
            return actions;
        } 

        private List<Choice> DialogueChoiceActions(){
            List<Choice> actions = new List<Choice>();
            //kitchen
            //t1
            actions.Add(new Choice("yes", () => {
                DialogueMenu(t.getDialogueById(2));
                lastActionID = "Talk";
            }));
            actions.Add(new Choice("no", () => {
                lastActionID = "Talk";
            }));
            //t2
            actions.Add(new Choice("then you should go", () => {
                killFactor += 1;
                lastActionID = "Talk";
            }));
            actions.Add(new Choice("and?", () => {
                DialogueMenu(t.getDialogueById(5));
                lastActionID = "Talk";
            }));
            //bath - 4+
            //t1
            actions.Add(new Choice("you do this every morning", () => {
                killFactor += 1;
                DialogueMenu(t.getDialogueById(1));
                lastActionID = "Talk";
            }));
            actions.Add(new Choice("fine", () => {
                time = time.AddMinutes(30);
                lastActionID = "Talk";
            }));
            //t2 
            actions.Add(new Choice("when will you finish?", () => {
                killFactor += 1;
                DialogueMenu(t.getDialogueById(7));
                lastActionID = "Talk";
            }));
            actions.Add(new Choice("stop! you're out of tune", () => {
                killFactor += 1;
                DialogueMenu(t.getDialogueById(7));
                lastActionID = "Talk";
            }));
            actions.Add(new Choice("leave", () => {
                lastActionID = "Talk";
            }));
            //living 9+
            //t1
            actions.Add(new Choice("clean the house", () => {
                time = time.AddMinutes(30);
                lastActionID = "Talk";
            }));
            actions.Add(new Choice("it's Maya's turn, not mine", () => {
                DialogueMenu(t.getDialogueById(3));
                lastActionID = "Talk";
            }));
            actions.Add(new Choice("clean? i don't clean", () => {
                killFactor += 1;
                lastActionID = "Talk";
            }));
            //t2
            actions.Add(new Choice("ignore, watch tv", () => {
                DialogueMenu(t.getDialogueById(4));
                lastActionID = "Talk";
            }));
            actions.Add(new Choice("whatcha doin?", () => {
                DialogueMenu(t.getDialogueById(4));
                lastActionID = "Talk";
            }));
            //contDialogs 14+
            //t1
            actions.Add(new Choice("go away!", () => {
                killFactor += 1;
                lastActionID = "Talk";
            }));
            actions.Add(new Choice("give up", () => {
                time = time.AddMinutes(15);
                lastActionID = "Talk";
            }));
            //t2
            actions.Add(new Choice("ok", () => {
                cleanFactor += 1;
                lastActionID = "Talk";
            }));
            actions.Add(new Choice("no way", () => {
                killFactor += 1;
                lastActionID = "Talk";
            }));
            //t3 -18+
            actions.Add(new Choice("leave", () => {
                lastActionID = "Talk";
            }));
            actions.Add(new Choice("wait", () => {
                time = time.AddMinutes(60);
                DialogueMenu(t.getDialogueById(5));
                lastActionID = "Talk";
            }));
            //t4
            actions.Add(new Choice("goto supermarket", () => {
                DeathScreen(10);
            }));
            actions.Add(new Choice("buy online", () => {
                time = time.AddMinutes(30);
                lastActionID = "Talk";
            }));
            actions.Add(new Choice("no", () => {
                killFactor += 1;
                lastActionID = "Talk";
            }));
            //t5
            actions.Add(new Choice("wait", () => {
                time = time.AddMinutes(30);
                lastActionID = "Talk";
            }));
            actions.Add(new Choice("leave", () => {
                lastActionID = "Talk";
            }));
            //calling friend choices - 25+
            //batman
            actions.Add(new Choice("save the world", () => {
                DeathScreen(10);
            }));
            actions.Add(new Choice("sing i'm not batman im spider-pigX3", () => {
                lastActionID = "Talk";
            }));
            //superman
            actions.Add(new Choice("go back", () => {
                DeathScreen(10);
            }));
            // more - 28+
            actions.Add(new Choice("go catch pokemons", () => {
                DeathScreen(10);
            }));
            actions.Add(new Choice("maybe later", () => {
                lastActionID = "Talk";
            }));
            return actions;
        }
        #endregion
    }
}


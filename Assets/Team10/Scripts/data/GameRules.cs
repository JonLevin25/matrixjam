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
        private int deathCheck;
        private int repeat;
        private string lastActionID;
        private bool takenPill;

        private int cleanFactor;
        private int hungerFactor;
        private double workFactor;
        private int killFactor;

        public List<Choice> choices;
        public System.DateTime time = System.DateTime.Parse("9:00 AM");
        public Text Timer;

        public GameObject Panel;

        public GameObject vidPanel;
        public VideoPlayer vid;
        
        public RandomDialogueTree t;
        public string playerName;
        public bool isDead;
        public float typingSpeed;
        #endregion
        
        void Start(){
            t = new RandomDialogueTree("player1");
            declareChoices();

            vid.loopPointReached += (VideoPlayer v) => {
                vidPanel.SetActive(false);
                Panel.SetActive(false);
            };
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

        #region Game Death

        //called by all actions with action id - check if action repeated
        //and do reapeat stuff if true -  modifies the repeted varibles and calls to end if > num?
        public void CheckRepeat(string id){
            if(lastActionID != id){
                repeat = 0;
                return;
            }
            repeat += 1;
            if(repeat > 2){
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
            if(!canCharacterAvoidDeath() && killFactor > 2){
                DeathScreen(3);
            }
        }

        public void endOfDay(){
            //private int cleanFactor; // if < 3 kill - end of day only
            //private int hungerFactor; // must eat 2 meals during the day if counter < 2 kill - end of day
            //private int workFactor;  // must reach 3 point by the end of the day to survive - end of day
            //private int killFactor; // 3 will kill 
            if(cleanFactor < 3){
                DeathScreen(0);
            }
            else if(hungerFactor < 2){
                DeathScreen(1);
            }
            else if(workFactor < 3){
                DeathScreen(2);
            }
            else if(!canCharacterAvoidDeath() && killFactor > 2){
                DeathScreen(3);
            }
            else{
                // survive
                DeathScreen(5);
            }
        }

        public void DeathScreen(int id){
            isDead = true;
            FindObjectOfType<EndGame>().DeathScene(id);
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
            }));
            actions.Add(new Choice("A day in life 2020", () => {
                DeathScreen(9);
            }));
            actions.Add(new Choice("fart in a jar", () => {
                time = time.AddMinutes(30);
                Panel.SetActive(true);
                vidPanel.SetActive(true);
                vid.Play();
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


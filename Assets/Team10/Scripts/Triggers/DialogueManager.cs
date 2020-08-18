using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MatrixJam.Team10
{
    public class DialogueManager : MonoBehaviour
    {
        public GameObject dialoguePanel;

        public GameObject TextPanel;
        public GameObject continueButton;
        public Text nameDisplay;
        public Text textDisplay;

        public GameObject OptionsPanel;
        public GameObject cancelButton;
        public Button[] Options;
        public Queue<Dialogue> dialogues;
        public float typingSpeed;

        private string currSentence;

        // Start is called before the first frame update
        void Start()
        {
            dialogues = new Queue<Dialogue>();
        }

        public void StartDialogue(DialogueTree tree){
            Debug.Log("start a conversation");
            dialogues.Clear();
            dialogues = tree.getDialogues();
            dialoguePanel.SetActive(true);
            DisplayNextDialogue();
        }

        public void StartActionChoice(Dialogue actions){
            Debug.Log("do something");
            dialogues.Clear();
            dialogues.Enqueue(actions);
            dialoguePanel.SetActive(true);
            DisplayNextDialogue();
        }

        IEnumerator Type(string sentence){
            foreach (char letter in sentence.ToCharArray())
            {
                textDisplay.text += letter;
                yield return new WaitForSeconds(typingSpeed);
            }
        }

        public void UpdateChoices(Dialogue curr){
                foreach(Button b in Options){
                    b.gameObject.SetActive(false);
                }
                List<Choice> choices = FindObjectOfType<GameRules>().choices;
                for(int i=0; i < curr.choices.Length; i++)
                {
                    int c = curr.choices[i];
                    Options[i].GetComponentInChildren<Text>().text = choices[c].info;
                    Options[i].onClick = new Button.ButtonClickedEvent();
                    Options[i].onClick.AddListener(delegate { 
                        Debug.Log("click " + choices[c].info);
                        EndDialogue();
                        choices[c].action();
                    });
                    Options[i].gameObject.SetActive(true);
                }
        }

        public void DisplayNextDialogue(){
            continueButton.SetActive(false);
            cancelButton.SetActive(false);
            OptionsPanel.SetActive(false);
            TextPanel.SetActive(false);
            textDisplay.text = "";

            if(dialogues.Count == 0){
                EndDialogue();
                return;
            }
            Dialogue curr = dialogues.Dequeue();
            nameDisplay.text = curr.name;
            if(curr.choices != null){
                OptionsPanel.SetActive(true);
                cancelButton.SetActive(true);
                UpdateChoices(curr);
            }
            else{
                TextPanel.SetActive(true);
                currSentence = curr.sentences;
                StartCoroutine(Type(curr.sentences));
            }
        }

        public void EndDialogue(){
            Debug.Log("End");
            dialoguePanel.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
            if(textDisplay.text == currSentence){
                continueButton.SetActive(true);
            }
            if(FindObjectOfType<GameRules>().isDead){
                dialoguePanel.SetActive(false);
            }
        }


    }
}

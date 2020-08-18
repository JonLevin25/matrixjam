using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team10
{
    public class CharaTrigger : MonoBehaviour
    {
        public int id;
        private DialogueManager Dialoguer;
        private GameObject BlockPanel;

        void Start(){
                Dialoguer = FindObjectOfType<DialogueManager>();
                BlockPanel = FindObjectOfType<EndGame>().Panel;
            }

        void OnMouseDown(){
            if(!BlockPanel.activeSelf && !Dialoguer.dialoguePanel.activeSelf){
                TriggerDialogue();
            }
        }

        public void TriggerDialogue(){
            if(id == 1){
                Dialoguer.StartDialogue(FindObjectOfType<GameRules>().t.getRoommate1Dialogue());
            }
            else if(id == 2){
                Dialoguer.StartDialogue(FindObjectOfType<GameRules>().t.getRoommate3Dialogue());
            }
        }
    }
}

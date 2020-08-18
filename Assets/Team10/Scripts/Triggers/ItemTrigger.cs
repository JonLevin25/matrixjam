using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team10
{
    public class ItemTrigger : MonoBehaviour
        {
            public int[] choices;
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
                Dialogue dialogue = new Dialogue("", choices);
                Dialoguer.StartActionChoice(dialogue);
            }
        }
}

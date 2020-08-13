using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team10
{
    public class ItemTrigger : MonoBehaviour
        {
            public int[] choices;
            public GameObject BlockPanel;
            public GameObject DialoguePanelBlock;

            void OnMouseDown(){
                if(!BlockPanel.activeSelf && !DialoguePanelBlock.activeSelf){
                    TriggerDialogue();
                }
            }

            public void TriggerDialogue(){
                Dialogue dialogue = new Dialogue("", choices);
                FindObjectOfType<DialogueManager>().StartActionChoice(dialogue);
            }
        }
}

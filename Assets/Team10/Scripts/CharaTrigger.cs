using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team10
{
    public class CharaTrigger : MonoBehaviour
    {
        public int id;
        public GameObject BlockPanel;
        public GameObject DialoguePanelBlock;

        void OnMouseDown(){
            if(!BlockPanel.activeSelf && !DialoguePanelBlock.activeSelf){
                TriggerDialogue();
            }
        }

        public void TriggerDialogue(){
            if(id == 1){
                FindObjectOfType<DialogueManager>().StartDialogue(FindObjectOfType<GameRules>().t.getRoommate1Dialogue());
            }
            else if(id == 2){
                FindObjectOfType<DialogueManager>().StartDialogue(FindObjectOfType<GameRules>().t.getRoommate3Dialogue());
            }
        }
    }
}

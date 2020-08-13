using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team10
{
    // will be placed on doors
    public class RoomTrigger : MonoBehaviour
    {
        public AudioSource bgSound;
        public string RoomName;
        public bool isInRoom = false;

        void Start(){
            bgSound.loop = true;
            bgSound.Play(0);
            bgSound.Pause();
        }

        void OnTriggerEnter2D(Collider2D c){
            isInRoom = !isInRoom;
            if(isInRoom){
                // bgSound.UnPause();
                if(RoomName == "LivingRoom" || RoomName == "Kitchen" || RoomName == "Bath"){
                    TriggerRandomDialogue();
                }
            }
            else{
                bgSound.Pause();
            }
        }

        public void TriggerRandomDialogue()
        {
            DialogueTree a;
            bool dialog = FindObjectOfType<GameRules>().t.getRandomDialog(RoomName, out a);
            if(dialog)
            {
                FindObjectOfType<DialogueManager>().StartDialogue(a);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team10
{
    public class DialogueTree
    {
        public Character npc;
        private Queue<Dialogue> dialogue;
        
        public DialogueTree(){
            npc = new Character("Test NPC");
            dialogue = new Queue<Dialogue>();
        }

        public DialogueTree(Character talkingNpc){
            npc = talkingNpc;
            dialogue = new Queue<Dialogue>();
        }
        public DialogueTree(Character talkingNpc, Queue<Dialogue> dialogueQueue){
            npc = talkingNpc;
            dialogue = dialogueQueue;
        }

        public void addDialogue(Dialogue message){
            dialogue.Enqueue(message);
        }

        public Queue<Dialogue> getDialogues(){
            return dialogue;
        }

        public Dialogue getDialogue(){
            return dialogue.Dequeue();
        }
    }
}

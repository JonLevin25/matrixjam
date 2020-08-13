using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MatrixJam.Team22
{
    public class DialogueManager : MonoBehaviour
    {
        public float openingDelay = 2f;

        [TextArea]
        public string[] introDialogues;

        [TextArea]
        public string[] postTutorialDialogues;

        [TextArea]
        public string[] epicFailDialogues;

        [TextArea]
        public string[] failDialogues;

        [TextArea]
        public string[] winDialogues;

        public Animator messageBoxAnimator;
        public Text messageBoxText;
        public AudioSource source;
        public AudioClip confirmSound;
        public AudioClip scaryConfirmSound;

        private string[] currentDialogue;
        private int dIndex = 0;
        private bool canDialogue = false;

        public static DialogueManager instance;

        private void Start()
        {
            messageBoxAnimator.gameObject.SetActive(false);
            currentDialogue = introDialogues;
            Invoke("ShowBox", openingDelay);

            instance = this;
        }

        private void ShowBox()
        {
            messageBoxAnimator.gameObject.SetActive(true);
            messageBoxText.text = currentDialogue[dIndex];
            source.PlayOneShot(confirmSound);

            canDialogue = true;
        }

        private void Update()
        {
            if (Input.anyKeyDown && !Input.GetKeyDown(KeyCode.Escape) && canDialogue)
            {
                NextDialogue();
            }

         }

        private void NextDialogue()
        {
            if(dIndex + 1 >= currentDialogue.Length)
            {
                // no more dialogues
               //messageBoxText.text = "";
               messageBoxAnimator.SetTrigger("Out");
               canDialogue = false;

               if (currentDialogue == introDialogues)
                {
                    GameManager.instance.StartTutorial();
                }

                else if (currentDialogue == postTutorialDialogues)
                {
                    GameManager.instance.StartGame();
                }

                else if (currentDialogue == epicFailDialogues)
                {
                    GameManager.instance.GetExit(1).Invoke();
                }

                else if (currentDialogue == failDialogues)
                {
                    GameManager.instance.GetExit(2).Invoke();
                }

                else if (currentDialogue == winDialogues)
                {
                    GameManager.instance.GetExit(3).Invoke();
                }
            }
            else
            {
                dIndex++;
                messageBoxAnimator.SetTrigger("In");

                if(currentDialogue == epicFailDialogues)
                    source.PlayOneShot(scaryConfirmSound);
                else
                    source.PlayOneShot(confirmSound);

                messageBoxText.text = currentDialogue[dIndex].Replace("$MISS", GameManager.instance.GetMisses().ToString()); ;
            }
        }

        public void EnableDialogues(int i)
        {
            // inb4 yanderedev yes I know this is inefficent but fuck it this is a game jam
            if (i == 0)
                currentDialogue = postTutorialDialogues;

            else if (i == 1)
                currentDialogue = epicFailDialogues;

            else if (i == 2)
                currentDialogue = failDialogues;

            else if (i == 3)
                currentDialogue = winDialogues;

            canDialogue = true;
            dIndex = -1;
            NextDialogue();
        }

        public bool IsInDialogue()
        {
            return canDialogue;
        }
    }
}

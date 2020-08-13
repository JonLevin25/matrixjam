using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace MatrixJam.Team11
{
    public class TutorialManager : MonoBehaviour
    {
        float playerGravity;
        PlayerController player;
        [SerializeField]  TextFader bubbleTut, pickupTut, mapTut, buttonTut;

        [SerializeField] SpriteRenderer bubbleRenderer, keyRenderer, buttonRenderer, blackBackground;
        int bubbleDefaultOrder, keyDefaultOrder, buttonDefaultOrder;
        [SerializeField] int boldOrder = 51;
       

        bool isWaitingForSpaceHold;
        bool isWaitingForButtonPress;
        bool isWaitingForMapView;
        static bool finishedTutorial;
        bool pressedM;

       



        // Start is called before the first frame update
        void Start()
        {
            
            player = FindObjectOfType<PlayerController>();
            playerGravity = player.GetComponent<Rigidbody2D>().gravityScale;
            player.GetComponent<Rigidbody2D>().gravityScale = 0;
            blackBackground.enabled = true;
            bubbleDefaultOrder = bubbleRenderer.sortingOrder;
            keyDefaultOrder = keyRenderer.sortingOrder;
            buttonDefaultOrder = buttonRenderer.sortingOrder;
            if (finishedTutorial)
            {
                blackBackground.enabled = false;
                bubbleTut.gameObject.SetActive(false);
            }

        }

        // Update is called once per frame
        void Update()
        {
           /* if(Input.GetKeyDown(KeyCode.Q))
            {
                finishedTutorial = false;
            }*/
            if(finishedTutorial)
            {
                return;
            }
            if(isWaitingForSpaceHold && Input.GetKey(KeyCode.Space))
            {
                isWaitingForSpaceHold = false;
                keyRenderer.sortingOrder = keyDefaultOrder;
                blackBackground.enabled = false;

                Time.timeScale = 1;
                pickupTut.gameObject.SetActive(false);

            }

            if(isWaitingForButtonPress && Input.GetKeyDown(KeyCode.Space))
            {
                isWaitingForButtonPress = false;
                Time.timeScale = 1;
                buttonTut.gameObject.SetActive(false);
                buttonRenderer.sortingOrder = buttonDefaultOrder;
            }
            if(isWaitingForMapView && Input.GetKey(KeyCode.M))
            {
                isWaitingForMapView = false;
                Time.timeScale = 1;
                blackBackground.enabled = false;

                mapTut.gameObject.SetActive(false);
                finishedTutorial = true;
            }
            if(Input.GetKey(KeyCode.M))
            {
                pressedM = true;
            }
            
        }

        public void PlayerEnteredBubble()
        {
           if(finishedTutorial)
            {
                return;
            }
            Time.timeScale = 0.5f;
            Invoke("BubbleFinished", 0.5f);

            // player.canMoveVertically = true;
        }

        void BubbleFinished()
        {
            if(finishedTutorial)
            {
                return;
            }
            bubbleTut.gameObject.SetActive(false);
            blackBackground.enabled = false;
            Time.timeScale = 1;
        }

        public void PlayerTouchedKey()
        {
            if (finishedTutorial)
            {
                return;
            }
            pickupTut.gameObject.SetActive(true);
            blackBackground.enabled = true;

            keyRenderer.sortingOrder = boldOrder;
            Time.timeScale = Mathf.Epsilon;
            isWaitingForSpaceHold = true;


        }

        public void PlayerTouchedButton()
        {
            if (finishedTutorial)
            {
                return;
            }
            buttonTut.gameObject.SetActive(true);
            blackBackground.enabled = true;

            buttonRenderer.sortingOrder = boldOrder;


            Time.timeScale = Mathf.Epsilon;
        }

        public void ButtonWasClicked()
        {
            if (finishedTutorial)
            {
                return;
            }
            blackBackground.enabled = false;

            buttonTut.gameObject.SetActive(false);
            Time.timeScale = 1f;

        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (finishedTutorial)
            {
                return;
            }
            if (collision.GetComponent<PlayerController>() && !finishedTutorial)
            {
                if(pressedM)
                {

                    finishedTutorial = true;
                    return;
                }
                mapTut.gameObject.SetActive(true);
                isWaitingForMapView = true;
                Time.timeScale = Mathf.Epsilon;
                blackBackground.enabled = true ;


            }
        }




    }
}

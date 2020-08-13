using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team11
{
    public class DoorSwitcher : MonoBehaviour
    {
        bool leftOpenRightClosed;

        [SerializeField] Door leftDoor;
        [SerializeField] Door rightDoor;
       // [SerializeField] CinemachineVirtualCamera cmL;
     //   [SerializeField] CinemachineVirtualCamera cmR;
        [SerializeField] float initialDelay = 0.5f;
        [SerializeField] float delayBetweenCams = 0.5f;
        Animator _animator;


        // Start is called before the first frame update
        void Start()
        {
            _animator = GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update()
        {

        }


        
        

        public void StartSwitch()
        {
            if (!leftOpenRightClosed)
            {
                leftOpenRightClosed = true;
                _animator.SetBool("right", leftOpenRightClosed);
                leftDoor.Open();
                //yield return new WaitForSeconds(delayBetweenCams);
                // cmL.enabled = false;
                // cmR.enabled = true;
                rightDoor.Close();
                //yield return new WaitForSeconds(initialDelay);
                //  cmR.enabled = false;
            }
            else
            {
                leftOpenRightClosed = false;
                _animator.SetBool("right", leftOpenRightClosed);
                leftDoor.Close();
                //   yield return new WaitForSeconds(delayBetweenCams);
                //  cmL.enabled = false;
                //  cmR.enabled = true;
                rightDoor.Open();
                // yield return new WaitForSeconds(initialDelay);
                // cmR.enabled = false;
            }
        }


        /* private IEnumerator SwitchCoroutine()
         {
           //  cmL.enabled = true;
            /// yield return new WaitForSeconds(initialDelay);
            if(!leftOpenRightClosed)
             {
                 leftOpenRightClosed = true;
                 _animator.SetBool("right", leftOpenRightClosed);
                 leftDoor.Open();
                 //yield return new WaitForSeconds(delayBetweenCams);
                // cmL.enabled = false;
                // cmR.enabled = true;
                 rightDoor.Close();
                 //yield return new WaitForSeconds(initialDelay);
               //  cmR.enabled = false;
             }
            else
             {
                 leftOpenRightClosed = false;
                 _animator.SetBool("right", leftOpenRightClosed);
                 leftDoor.Close();
              //   yield return new WaitForSeconds(delayBetweenCams);
               //  cmL.enabled = false;
               //  cmR.enabled = true;
                 rightDoor.Open();
                // yield return new WaitForSeconds(initialDelay);
                // cmR.enabled = false;
             }
         }*/

        public void PlayLeverSound()  // called by animation event
        {
            SFXPlayer.instance.PlaySFX(SFXPlayer.instance.handleSFX);
        }

    }
}

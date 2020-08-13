using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MatrixJam.Team11
{
    public class InteractableItem : MonoBehaviour
    {
        public bool canInteract = true;
        [SerializeField] bool isTutorialButton;
        [SerializeField] bool oneTimeUse = false;
        [SerializeField] protected UnityEvent OnInteract;
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

        public void Interact()
        {
            Debug.Log("You tried to press " + gameObject.name);
            if (!canInteract || !gameObject.activeInHierarchy)
            {
                return;
            }
            _animator.SetTrigger("interact");
            canInteract = false;
            Debug.Log("You sucessfully pressed " + gameObject.name);

            if (this.OnInteract != null)
            {
                this.OnInteract.Invoke();
            }

            

        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (isTutorialButton)
            {
                var player = FindObjectOfType<PlayerController>();

                if (!player.hasObjectInHands && Object.ReferenceEquals(player.FocusedObject, this.gameObject))
                {
                    isTutorialButton = false;
                    FindObjectOfType<TutorialManager>().PlayerTouchedButton();
                }
                
            }

        }

        public void ResetButton() // called by animation event
        {
            _animator.ResetTrigger("interact");

            

            if (!oneTimeUse)
            {
                canInteract = true;
            }
        }

     

        public void PlayButtonSound()  // called by animation event
        {
            SFXPlayer.instance.PlaySFX(SFXPlayer.instance.buttonSFX);

        }
    }
}

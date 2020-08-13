using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace MatrixJam.Team2
{
    public abstract class InteractableController : MonoBehaviour, IInteractable
    {
        [SerializeField] protected GameObject interactionTextObject;

        protected bool isInteractable = true;


        protected abstract void Interaction();

        public void Interact()
        {
            if (isInteractable)
            {
                Interaction();
            }
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (!isInteractable) return;

            EnableInteractionDisplay();
        }

        void OnTriggerExit2D(Collider2D other)
        {
            DisableInteractionDisplay();
        }

        protected void EnableInteractionDisplay()
        {
            if (interactionTextObject != null)
            {
                interactionTextObject.SetActive(true);
            }
        }

        protected void DisableInteractionDisplay()
        {
            if (interactionTextObject != null)
            {
                interactionTextObject.SetActive(false);
            }
        }

        protected void DisableInteraction()
        {
            isInteractable = false;
            DisableInteractionDisplay();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team2
{
    public class PlayerInteractionController : MonoBehaviour
    {
        private List<IInteractable> interactables;

        void Start()
        {
            interactables = new List<IInteractable>();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.E) && interactables.Count > 0)
            {
                interactables[interactables.Count - 1].Interact();
            }
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            IInteractable interactable;
            if (other.TryGetComponent<IInteractable>(out interactable))
            {
                interactables.Add(interactable);
            }
        }

        void OnTriggerExit2D(Collider2D other)
        {
            IInteractable interactable;
            if (other.TryGetComponent<IInteractable>(out interactable))
            {
                interactables.Remove(interactable);
            }
        }
    }
}

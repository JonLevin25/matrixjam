using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MatrixJam.Team2
{
    public class SwitchButtonController : InteractableController
    {
        [SerializeField] private UnityEvent onInteracted;

        protected override void Interaction()
        {
            DisableInteraction();
            onInteracted.Invoke();
        }
    }
}

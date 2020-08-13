using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team11
{
    public class OutlineHelper : MonoBehaviour
    {
        [SerializeField] public Color OutlineGroupColor = Color.black;

        private bool _error;

        private void Awake()
        {
            this.UpdateOutlines();
        }

        public void UpdateOutlines()
        {
            System.Array.ForEach(Object.FindObjectsOfType<LiftableObject>(), this.ApplyStandarts);
            System.Array.ForEach(Object.FindObjectsOfType<InteractableItem>(), this.ApplyStandarts);

            if (this._error)
            {
                Debug.LogError($"{this.name}: Some items are missing an outline!");
            }
        }

        public void ApplyStandarts(Component obj)
        {
            this.ApplyStandarts(obj.gameObject);
        }

        public void ApplyStandarts(GameObject obj)
        {
            // check if has Outline, no -> error
            if (obj.TryGetComponent<Outlineotron>(out Outlineotron outline) == false)
            {
                Debug.LogWarning($"{this.name}: Outline is missing at item <{obj.name}>.");
                this._error = true;
                return;
            }

            // set outline color.
            outline.OutlineColor = this.OutlineGroupColor;
            if (outline.IsReady)
            {
                outline.UpdateOutlineParts();
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team2
{
    public class ItemPickingController : MonoBehaviour
    {
        [SerializeField] 
        [Range(0f, 1f)]
        private float rotateSpeed;

        private Pickable pickable;

        private void Update()
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (Input.GetMouseButtonDown(0))
            {
                if (IsPickablePicked())
                {
                    DropPickable();
                }
                else
                {
                    Pickup(mousePosition);
                }
            }

            if (IsPickablePicked())
            {
                MovePickable(mousePosition);
                RotatePickable(mousePosition);
            }
        }

        public void Pickup(Vector2 mousePosition)
        {
            RaycastHit2D raycastHit = Physics2D.Raycast(mousePosition, Vector2.zero, 0f);
            if (raycastHit.collider != null)
            {
                Pickable pickable;
                if (raycastHit.collider.TryGetComponent(out pickable))
                {
                    pickable.Pick();
                    this.pickable = pickable;
                }
            }
        }

        private bool IsPickablePicked()
        {
            return pickable != null;
        }

        private void DropPickable()
        {
            pickable.Drop();
            pickable = null;
        }

        private void MovePickable(Vector2 mousePosition)
        {
            pickable.transform.position = mousePosition;
        }

        private void RotatePickable(Vector2 mousePosition)
        {
            if (Input.GetKey(KeyCode.Q))
            {
                pickable.transform.Rotate(0, 0, rotateSpeed);
            }
            else if (Input.GetKey(KeyCode.E))
            {
                pickable.transform.Rotate(0, 0, -rotateSpeed);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team2
    {
        namespace MatrixJam.Team2
{
    public class RotateTowardsMouse : MonoBehaviour
        {
            private Camera cam;
            private Vector3 offset;

            void Start()
            {
                cam = Camera.main;
                offset = new Vector3(transform.position.x - transform.parent.position.x, 0, 0);
            }

            void Update()
            {
                Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

                Vector3 aimDir = (mousePos - transform.parent.position);
                float angle = Mathf.Atan2(aimDir.y, aimDir.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                transform.position = transform.parent.position + transform.rotation * offset;
            }
        }
    }
}

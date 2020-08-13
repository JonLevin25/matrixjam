using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team2
{
    public class TurretStupid : MonoBehaviour
    {
        public float correction = 0;

        private Vector3 object_pos;
        private Vector3 mousePosition;

        //public float moveSpeed = 0.1f;
        //public Transform target; //Assign to the object you want to rotate

        void Update()
        {
            //rotation
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 5.23f;

            Vector3 objectPos = Camera.main.WorldToScreenPoint(transform.position);
            mousePos.x = mousePos.x - objectPos.x;
            mousePos.y = mousePos.y - objectPos.y;
            float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg - correction;
            // TODO: keep the original rotation on y, and fix the flip script
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
    }
}
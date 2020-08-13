using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team21 {
    public class CameraFollow : MonoBehaviour {
        public Transform target;
		public float moveSpeed = 0.1f;
		public Vector3 offset;

        // Update is called once per frame
        void Update() {
			transform.position = (new Vector3(target.position.x, transform.position.y, transform.position.z)) + offset;
        }
    }
}

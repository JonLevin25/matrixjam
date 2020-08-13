using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team21 {
    public class WindArea : MonoBehaviour {
        public float strength = 3f;
		public Vector3 direction;
		public Transform target;
		private Vector3 mousePosition;

		void Update() {
            mousePosition = Input.mousePosition;
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
			float posX = 0f;
			if (mousePosition.x > target.position.x) {
				posX = -0.3f;
			} else if (mousePosition.x < target.position.x) {
				posX = 0.3f;
			}
            direction = new Vector3(posX, direction.y, direction.z);
        }
    }
}

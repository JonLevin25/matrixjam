using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team21 {
    public class MovingObstacle : MonoBehaviour {
        public float speed = 5f;
		public Vector3 movingDirection = Vector3.up;
		public float maxBorder = 2f;
		public float minBorder = -1f;

		private Vector3 dir;
		private float directionAxis;
        // Update is called once per frame
		void Start() {
			dir = movingDirection;
		}

        void Update() {
			if (movingDirection.x > 0) {
				directionAxis = transform.localPosition.x;
			} else {
				directionAxis = transform.localPosition.y;
			}
			if (directionAxis >= maxBorder) {
				dir = -movingDirection;
			} else if (directionAxis <= minBorder) {
				dir = movingDirection;
			}
            transform.Translate(dir * speed * Time.deltaTime, Space.World);
        }
    }
}

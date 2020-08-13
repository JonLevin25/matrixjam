using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team21 {
    public class WindAreaContainer : MonoBehaviour {
    	public float moveSpeed = 0.1f;
		public float rotateSpeed = 1f;
		public float maxRotationDeg = 45f;
		public float rotationZ;
		public ParticleSystem vfx;

		private Vector3 mousePosition;
		private bool isMove = true;

        // Update is called once per frame
        void Update() {
			if (isMove && GameState.state == State.PLAY) {
				// float translation = Input.GetAxis("Horizontal") * moveSpeed;
				// transform.Translate(translation, 0, 0);
				mousePosition = Input.mousePosition;
				mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
				transform.position = Vector2.Lerp(transform.position, new Vector3(mousePosition.x, transform.position.y, transform.position.z), moveSpeed);
				// rotationZ = Mathf.Clamp(Input.GetAxis("Mouse X") + 0.01f, -maxRotationDeg, maxRotationDeg);
				// transform.Rotate(new Vector3(0, 0, rotationZ) * Time.deltaTime * rotateSpeed, Space.Self);
				var colorOverLifetime = vfx.colorOverLifetime;
				if (Input.GetMouseButtonDown(0)) {
        			colorOverLifetime.enabled = true;
				}
				if (Input.GetMouseButtonUp(0)) {
					colorOverLifetime.enabled = false;
				}
			}
        }

		void OnCollisionEnter(Collision collision) {
			if (collision.collider.tag == "Tag1") {
				Rigidbody rb = GetComponentInChildren<Rigidbody>();
				rb.useGravity = true;
				rb.constraints = RigidbodyConstraints.None;
				isMove = false;
			}
		}
    }
}

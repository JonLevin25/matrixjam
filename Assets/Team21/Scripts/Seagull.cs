using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team21 {
    public class Seagull : MonoBehaviour {
		public float speed = 10f;
		public Transform target;
		public float distanceToStart = 50f;
		public float distance;
		public AudioSource sfx;

        // Update is called once per frame
        void Update() {
			distance = Vector3.Distance(transform.position, target.position);
			if (distance <= distanceToStart) {
				transform.Translate(Vector3.right * speed * Time.deltaTime, Space.World);
				if (!sfx.isPlaying) {
					sfx.Play();
				}
			}
        }
    }
}

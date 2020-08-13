using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team22
{
    public class StopMotion : MonoBehaviour
    {
        public bool shakeOnStart = true;
        [Range(1f, 24f), Tooltip("How many times per second should the GameObject jitter?")]
        public float frameRate = 8f;
        [Range(0f, 10f), Tooltip("Jitter intensity for position values.")]
        public float positionAmplitude = 3f;
        [Range(0f, 10f), Tooltip("Jitter intensity for rotation values.")]
        public float rotationAmplitude = 1.5f;

        private bool shaking;
        private Vector3 pos;
        private Quaternion rot;

        void Start()
        {
            shaking = true;
            pos = transform.localPosition;
            rot = transform.localRotation;

            if (shakeOnStart) StartCoroutine(ShakeController());
        }

        IEnumerator ShakeController()
        {
            while (shaking)
            {
                Vector3 newPos = pos + Random.insideUnitSphere * positionAmplitude;
                newPos.z = pos.z;
                Vector3 newRot = rot.eulerAngles + Random.insideUnitSphere * rotationAmplitude;
                newRot.x = rot.x;
                newRot.y = rot.y;

                transform.localPosition = newPos;
                transform.localRotation = Quaternion.Euler(newRot);
                yield return new WaitForSeconds(1 / frameRate);
            }
        }

        public void StartShake()
        {
            shaking = true;
            StartCoroutine(ShakeController());
        }

        public void StopShake()
        {
            shaking = false;
            transform.localPosition = pos;
            transform.localRotation = rot;
        }
    }
}

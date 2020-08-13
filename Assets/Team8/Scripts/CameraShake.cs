using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team8
{
    public class CameraShake : MonoBehaviour
    {
        // Transform of the camera to shake. Grabs the gameObject's transform
        // if null.
        public Transform camTransform;

        // How long the object should shake for.
        public float shakeDuration = 0f;

        // Amplitude of the shake. A larger value shakes the camera harder.
        public float shakeAmount = 0.7f;
        public float decreaseFactor = 1.0f;

        Vector3 originalPos;

        void Awake()
        {
            if (camTransform == null)
            {
                camTransform = GetComponent(typeof(Transform)) as Transform;
            }
        }

        void OnEnable()
        {
            originalPos = camTransform.localPosition;
        }

        void Update()
        {
            if (shakeDuration > 0)
            {
                camTransform.localPosition = Vector3.Lerp(camTransform.localPosition,originalPos + Random.insideUnitSphere * shakeAmount,0.1f);

                shakeDuration -= Time.deltaTime * decreaseFactor;
            }
            else
            {
                shakeDuration = 0f;
                camTransform.localPosition = Vector3.Lerp(camTransform.localPosition, originalPos,0.1f);
            }
        }
    }
}

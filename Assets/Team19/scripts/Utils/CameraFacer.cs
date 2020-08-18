using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team19
{
    public class CameraFacer : MonoBehaviour
    {
        private static Camera _facedCamera;

        void Awake()
        {
            if (_facedCamera == null)
            {
                _facedCamera = Camera.main;
            }
        }

        void Update()
        {
            transform.forward = _facedCamera.transform.forward;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team22
{
    public class ShakeCam : MonoBehaviour
    {
        public static ShakeCam instance;
        public float time = 0.1f;
        public float amp = 0.25f;

        private bool shaking;
        private Vector3 pos;
        private Quaternion rot;

        void Start()
        {
            instance = this;
            pos = transform.localPosition;
            rot = transform.localRotation;
        }

        void Update()
        {
            if (shaking)
            {
                transform.localPosition = pos + Random.insideUnitSphere * amp;
                //transform.localRotation = Quaternion.Euler(rot.eulerAngles + Random.insideUnitSphere * amp);

                time -= Time.deltaTime;
                if (time <= 0)
                    Stop();
            }
        }

        public void Shake(float Time, float Amp)
        {
            time = Time;
            amp = Amp;
            shaking = true;
        }

        void Stop()
        {
            shaking = false;
            transform.localPosition = pos;
            transform.localRotation = rot;
        }
    }
}

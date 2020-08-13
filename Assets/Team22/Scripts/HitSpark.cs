using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team22
{
    public class HitSpark : MonoBehaviour
    {
        public float disappearTime = 0.1f;

        private void Start()
        {
            transform.Rotate(new Vector3(0, 0, Random.Range(-180, 180)));
            Destroy(gameObject, disappearTime);
        }
    }
}

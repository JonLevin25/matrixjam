using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team2
{
    namespace MatrixJam.Team2
    {
        public class RotateTowardsPoint : MonoBehaviour
        {
            [SerializeField] private Transform target;
            [SerializeField] private float correction;

            void Update()
            {
                if (target != null)
                {
                    var aimDir = target.position - transform.position;

                    float angle = Mathf.Atan2(aimDir.y, aimDir.x) * Mathf.Rad2Deg - correction;
                    transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
                }
            }
        }
    }
}

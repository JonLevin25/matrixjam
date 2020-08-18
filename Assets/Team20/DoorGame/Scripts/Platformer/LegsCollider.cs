using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team20
{
    public class LegsCollider : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other != null)
            {
                if (other.CompareTag("PlatformTrigger"))
                {
                    other.transform.parent.gameObject.layer = 0;
                    Debug.Log("comeone");
                }

            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other != null)
            {
                if (other.CompareTag("PlatformTrigger"))
                {
                    other.transform.parent.gameObject.layer = 10;
                    Debug.Log("comeone yeah");
                }

            }
        }

    }
}

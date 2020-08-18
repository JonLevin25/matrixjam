using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team20
{
    public class HeadCollider : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other != null)
            {
                if (other.CompareTag("Platform"))
                {
                    Debug.Log("head collision with platform");
                    GetComponentInParent<Player>().StopRising();
                }

            }
        }

    }
}

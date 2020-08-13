using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team2
{
    [RequireComponent(typeof(Collider2D))]
    public class Checkpoint : MonoBehaviour
    {
        public void Activate(PlayerController playerController)
        {
            for (int i = 0; i < transform.parent.childCount; i++)
            {
                transform.parent.GetChild(i).gameObject.SetActive(true);
            }
            playerController.transform.position = transform.position;
        }

        void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, 0.5f);
        }
    }
}

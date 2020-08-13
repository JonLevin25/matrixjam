using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team2
{
    public class EndGameController : MonoBehaviour
    {
        [SerializeField] private Exit exit;

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out PlayerController playerController))
            {
                Debug.Log("Ended game");
                exit.EndLevel();
            }
        }

        void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, 0.5f);
        }
    }
}

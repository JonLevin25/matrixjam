using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team2
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class Boundary : MonoBehaviour
    {
        [SerializeField] private bool destroyBullets = false;

        private BoxCollider2D boxCollider;

        void OnTriggerEnter2D(Collider2D other)
        {
            if (destroyBullets && other.TryGetComponent(out BulletController bulletController))
            {
                Destroy(other.gameObject);
            }
        }

        void OnValidate()
        {
            boxCollider = GetComponent<BoxCollider2D>();
        }

        void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, 0.5f);
        }
    }
}

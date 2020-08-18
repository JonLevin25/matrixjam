using System.Collections;
using UnityEngine;

namespace MatrixJam.Team17
{
    public class Projectile : MonoBehaviour, IPlayerOwned
    {
        public ProjectileConfig projectileConfig;
        
        Player owner;

        public Player Owner
        {
            get => owner;
            set
            {
                owner = value;
                StartCoroutine(IgnoreOwnerCollisionMomentarily());
            }
        }

        Rigidbody2D body;
        public Rigidbody2D Body
        {
            get
            {
                if (body == null)
                {
                    body = GetComponent<Rigidbody2D>();
                }

                return body;
            }
            set => body = value;
        }

        public virtual void Start()
        {
            if (projectileConfig.expires)
            {
                Destroy(gameObject, projectileConfig.expireTime);
            }
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (projectileConfig.explodes)
            {
                Destroy(gameObject);
                Explosion explosionPrefab = projectileConfig.explosionConfig.explosionPrefab;
                if (explosionPrefab != null)
                {
                    Instantiate(explosionPrefab, transform.position, Quaternion.identity);
                }
            }
        }

        IEnumerator IgnoreOwnerCollisionMomentarily()
        {
            Collider ownerFeetCollider = owner.feet.Collider;
            Collider ownerHeadCollider = owner.head.Collider;
            Collider projCollider = GetComponent<Collider>();

            Physics.IgnoreCollision(ownerFeetCollider, projCollider);
            Physics.IgnoreCollision(ownerHeadCollider, projCollider);
            yield return new WaitForSeconds(1f);
            Physics.IgnoreCollision(ownerFeetCollider, projCollider, false);
            Physics.IgnoreCollision(ownerHeadCollider, projCollider, false);

            yield return null;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team2
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class BulletController : MonoBehaviour
    {
        [SerializeField] private float movementSpeed;

        private Rigidbody2D rb;

        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        void FixedUpdate()
        {
            // כוס אמא של rb.MovePosition
            // שתקע אותי במשך שעה במקום שאני אהיה עז עם הגיון שתשתמש ב 
            // rb.position
            // תודה ביי
            rb.position += (Vector2)(transform.right * movementSpeed * Time.deltaTime);
        }

        void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.TryGetComponent(out BulletController otherBulletController))
            {
                Physics2D.IgnoreCollision(other.collider, GetComponent<Collider2D>());
                return;
            }
            if (other.gameObject.TryGetComponent(out PlayerController playerController))
            {
                playerController.ActivateCheckpoint();
                Destroy(gameObject);
                return;
            }
            if (other.gameObject.TryGetComponent(out Floopable floopable))
            {
                floopable.ChangeBulletTrajectory(this.transform, other.contacts[0]);
                return;
            }
            if (other.gameObject.TryGetComponent(out Turret turret))
            {
                turret.gameObject.SetActive(false);
                return;
            }

            Destroy(gameObject);
        }
    }
}
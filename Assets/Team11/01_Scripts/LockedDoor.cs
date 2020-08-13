using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MatrixJam.Team11
{
    public class LockedDoor : MonoBehaviour
    {
       [SerializeField] private UnityEvent OnDoorUnlocked;
        bool wasOpened = false;

        [SerializeField] GameObject matchingKey;
        CapsuleCollider2D hurtingCollider;
        Animator _animator;
        Collider2D playerCollider;
        [SerializeField] Vector2 forceToAdd;

        // Start is called before the first frame update
        void Start()
        {
            hurtingCollider = GetComponent<CapsuleCollider2D>();
            playerCollider = FindObjectOfType<PlayerController>().GetComponent<Collider2D>();
            _animator = GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if(!collision.isTrigger && (collision.gameObject == matchingKey) && !wasOpened)
            {
                if(collision.GetComponent<LiftableObject>().isLifted)
                {
                    UnlockDoor();
                    wasOpened = true;

                }
            }
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            if(hurtingCollider.IsTouching(playerCollider) && playerCollider.GetComponent<PlayerController>().wasHurt == false)
            {
                playerCollider.GetComponent<PlayerController>().HurtPlayer();
                playerCollider.GetComponent<Rigidbody2D>().MovePosition((Vector2)playerCollider.transform.position + forceToAdd);
                playerCollider.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

            }
        }

        void UnlockDoor()
        {
            SFXPlayer.instance.PlaySFX(SFXPlayer.instance.unlockSFX);
            _animator.SetTrigger("poof");
            if(this.OnDoorUnlocked != null)
            {
                this.OnDoorUnlocked.Invoke();
            }

        }

        void DestroyOnAnimationFinish() // called by animation event
        {
            GetComponent<BoxCollider2D>().enabled = false;
            hurtingCollider.enabled = false;
        }
    }
}

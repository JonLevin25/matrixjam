using System;
using UnityEngine;

namespace MatrixJam.Team17
{
    [RequireComponent(typeof(Player))]
    public class PlayerAnimation : MonoBehaviour
    {
        Player player;
        Animator animator;
        int animatorSpeedId;
        int animatorGroundedId;
        int animatorCrouchingId;

        public PlayerConfig PlayerConfig => player.playerConfig;
        public PlayerInputData Input => player.Input;
        
        void Awake()
        {
            player = GetComponent<Player>();
            animator = GetComponentInChildren<Animator>();

            animatorSpeedId = Animator.StringToHash("Speed_f");
            animatorGroundedId = Animator.StringToHash("Grounded");
            animatorCrouchingId = Animator.StringToHash("Crouch_b");
        }

        void LateUpdate()
        {
            //animator.SetFloat(animatorSpeedId, Math.Abs(Input.move.x));
            //animator.SetBool(animatorGroundedId, player.IsGrounded);
            //animator.SetBool(animatorCrouchingId, player.IsCrouching);

            /*if (Math.Abs(Input.move.x) > 0.001f)
            {
                if (Input.move.x < 0f)
                {
                    LookLeft();
                }
                else if (Input.move.x > 0f)
                {
                    LookRight();
                }
            }*/
        }

        public void LookLeft()
        {
            Vector3 eulerAngles = transform.rotation.eulerAngles;
            transform.rotation = Quaternion.Euler(eulerAngles.x, 180, eulerAngles.z);
        }

        public void LookRight()
        {
            Vector3 eulerAngles = transform.rotation.eulerAngles;
            transform.rotation = Quaternion.Euler(eulerAngles.x, 0, eulerAngles.z);
        }
    }
}

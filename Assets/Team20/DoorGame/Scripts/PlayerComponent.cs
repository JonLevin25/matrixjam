using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team20
{
    public class PlayerComponent : MonoBehaviour
    {
        MovementComponent movement;
        ApplyGravityComponent applyGravity;
        Animator animator;
        public float walkSpeed = 10f;
        public float jumpHeight = 10f;
        public bool resetHorizontal = false;
        float lastHorizontal = 0f;
        [SerializeField] private bool _onMovingPlatform = false;
        public float movingPlatformFix = 1;
        [SerializeField] private float _coinNumber = 0f;
        public AudioClip jumpSound, landingSound, footstep1, footstep2, footstep3, attachDoor, flipDoor;
        AudioSource _audio;

        private enum LookDirection
        {
            Right,
            Left
        }

        [SerializeField] private LookDirection _lookDirection;

        void Start()
        {
            _audio = gameObject.GetComponent<AudioSource>();
            movement = GetComponent<MovementComponent>();
            applyGravity = GetComponent<ApplyGravityComponent>();
            animator = GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update()
        {
            UpdateVelocity();
            DoorCollection();
        }

        void UpdateVelocity()
        {
            float horizontal = Input.GetAxis("Horizontal");
            if (horizontal == 0f)
            {
                resetHorizontal = false;
            }

            if (!resetHorizontal)
            {
                lastHorizontal = horizontal;
                movement.velocity.x = horizontal * walkSpeed;
            }

            if (applyGravity.grounded)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    movement.velocity.y = jumpHeight;
                    //_audio.clip = jumpSound;
                    _audio.PlayOneShot(jumpSound);
                }

            }
        }

        private void LateUpdate()
        {
            if (animator)
            {
                if (applyGravity.grounded && Mathf.Abs(movement.velocity.x) > 0.001)
                {
                    animator.speed = Mathf.Abs(movement.velocity.x);
                }
                else
                {
                    animator.speed = 1f;
                }
                
                animator.SetBool("grounded", applyGravity.grounded);
                animator.SetFloat("xVelocity", movement.velocity.x);
                animator.SetFloat("yVelocity", movement.velocity.y);
            }
        }

        public void PlayDetach()
        {
            _audio.PlayOneShot(attachDoor);
        }

        public void PlayFootstep()
        {
            int i = Random.Range(0, 2);
            if(i==0)
                _audio.PlayOneShot(footstep1);
            if (i == 1)
                _audio.PlayOneShot(footstep2);
            if (i == 2)
                _audio.PlayOneShot(footstep3);

        }

        public void PlayFlip()
        {
            _audio.PlayOneShot(flipDoor);
        }

        public void DoorCollection()
        {
            if (Input.GetKeyDown(KeyCode.D))
                _lookDirection = LookDirection.Right;
            if (Input.GetKeyDown(KeyCode.A))
                _lookDirection = LookDirection.Left;
        }

        public bool PlayerStands()
        {
            return movement.velocity.x == 0;
        }

        public bool PlayerIsStill()
        {
            return PlayerStands() && applyGravity.grounded;
        }

        public void OnMovingPlatform(bool onThePlatform)
        {
            _onMovingPlatform = onThePlatform;
        }

        public void AddCoins(int number)
        {
            _coinNumber += number;
        }

        public void ExitPlatformVelocity(float velocity, float velocityX, float velocityY)
        {
            // //if (Input.GetAxis("Horizontal") == 0f)
            // //{
            // _addedVelocity = velocity;
            // _addedVelocityX = velocityX;
            // _addedVelocityY = velocityY;
            // //Debug.Log(Input.GetAxis("Horizontal"));
            // // }
            // 
            // Debug.Log(_addedVelocity);
        }
    }
}

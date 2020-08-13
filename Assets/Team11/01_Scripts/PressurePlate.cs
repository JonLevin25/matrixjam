using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team11
{
    public class PressurePlate: MonoBehaviour
    {
        public bool isPressed = false;
        PressurePlatePuzzleManager puzzleManager;
        Animator _animator;
        Collider2D _collider;
        int collisionCount = 0;

        // Start is called before the first frame update
        void Start()
        {
            puzzleManager = FindObjectOfType<PressurePlatePuzzleManager>();
            _animator = GetComponent<Animator>();
            _collider = GetComponent<Collider2D>();
        }

        // Update is called once per frame
        void Update()
        {
            if(collisionCount > 0 && !isPressed)
            {
                isPressed = true;
                _animator.SetBool("isBeingPressed", isPressed);
                SFXPlayer.instance.PlaySFX(SFXPlayer.instance.pressurePlatePressedSFX);

            }
            else if(collisionCount == 0 && isPressed)
            {
                isPressed = false;
                _animator.SetBool("isBeingPressed", isPressed);
                SFXPlayer.instance.PlaySFX(SFXPlayer.instance.pressurePlateReleasedSFX);

            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            collisionCount++;
            puzzleManager.CheckForValueChange();
        }



        private void OnCollisionExit2D(Collision2D collision)
        {
            collisionCount--;
            puzzleManager.CheckForValueChange();

            if (collisionCount <= 0)
            {
                collisionCount = 0;
            }
            
        }
    }
}
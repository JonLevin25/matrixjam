using UnityEngine;

namespace MatrixJam.Team17
{
    public class PlayerInputListener : MonoBehaviour
    {
        public PlayerInputData data;

        void Update()
        {
            // Move
            data.move.x = Input.GetAxis("Horizontal");
            data.move.y = Input.GetAxis("Vertical");

            // Jump
            data.jump = Input.GetAxis("Jump") > 0f;
            if (data.jump)
            {
                data.jumpReady = true;
                data.jumpTime = Time.time;
            }

            // Crouch
            //data.crouch = Input.GetAxis("Crouch") > 0f;

            // Fire primary
            data.fire1 = Input.GetAxis("Fire1") > 0f;

            // Fire alternate
            data.fire2 = Input.GetAxis("Fire2") > 0f;

            // Use item
            data.useItem = Input.GetAxis("Fire3") > 0f;

            // Cycle prev
            //data.cyclePrev = Input.GetAxis("CyclePrev") > 0f;

            // Cycle next
            //data.cycleNext = Input.GetAxis("CycleNext") > 0f;
        }
        
    }
}

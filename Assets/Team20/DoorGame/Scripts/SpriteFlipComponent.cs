using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team20
{
    public class SpriteFlipComponent : MonoBehaviour
    {
        MovementComponent movement;
        SpriteRenderer renderer;

        // Start is called before the first frame update
        void Start()
        {
            movement = GetComponent<MovementComponent>();
            renderer = GetComponent<SpriteRenderer>();
        }

        // Update is called once per frame
        void Update()
        {
            if(movement.velocity.x > float.Epsilon)
            {
                renderer.flipX = true;
            }
            else if(movement.velocity.x < -float.Epsilon)
            {
                renderer.flipX = false;
            }
        }
    }
}

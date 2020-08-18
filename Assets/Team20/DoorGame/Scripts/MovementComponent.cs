using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team20
{
    public class MovementComponent : MonoBehaviour
    {
        Rigidbody2D rigidBody;
        public Vector2 velocity = Vector2.zero;
        public float yAcceleration = 0f;
        public float minYVelocity = -60f;

        // Start is called before the first frame update
        void Start()
        {
            rigidBody = GetComponent<Rigidbody2D>();
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        private void FixedUpdate()
        {
            velocity.y += yAcceleration * Time.fixedDeltaTime;

            velocity.y = Mathf.Max(velocity.y, minYVelocity);

            Vector2 pos = ((Vector2)this.transform.position) + velocity * Time.fixedDeltaTime;
            rigidBody.MovePosition(pos);
        }
    }
}

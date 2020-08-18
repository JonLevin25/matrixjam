using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team20
{
    public class Timer
    {
        public Timer(float secs)
        {
            updateEvery = secs;
            nextUpdteIn = updateEvery;
        }

        public bool TimerEnd(float deltaTime)
        {
            nextUpdteIn -= deltaTime;
            if(nextUpdteIn < 0)
            {
                nextUpdteIn = updateEvery;
                return true;
            }
            return false;
        }

        float updateEvery;
        float nextUpdteIn;
    }

    public class EnemyControllerComponent : MonoBehaviour
    {
        enum State
        {
            Patrol,
            Chase
        }

        State state = State.Patrol;
        MovementComponent movement;
        public float movementX = 10f;
        public float minAdvanceDistance = 0.001f;
        Vector2 previousPosition;
        public float updateEverySecs = 0.1f;
        public float patrolDuration = 2f;
        public float viewDistance = 5f; 
        Timer updateTimer;
        Timer patrolTimer;
        public LayerMask playerLayerMask;
        // Start is called before the first frame update
        void Start()
        {
            movement = this.GetComponent<MovementComponent>();
            previousPosition = this.transform.position;
            updateTimer = new Timer(updateEverySecs);
            patrolTimer = new Timer(patrolDuration);
        }

        // Update is called once per frame
        void Update()
        {
            if(updateTimer == null || !updateTimer.TimerEnd(Time.deltaTime))
            {
                return;
            }

            switch (state)
            {
                case State.Chase:
                    UpdateChase();
                    break;
                case State.Patrol:
                    UpdatePatrol();
                    break;
            }

            if(Input.GetKeyDown(KeyCode.M))
                SceneManagerComponent.instance.RestartScene();
        }

        void UpdatePatrol()
        {
            var hit = Physics2D.Raycast(this.transform.position, movement.velocity.normalized, viewDistance, playerLayerMask);
            if(hit)
            {
                if(hit.collider.gameObject.GetComponent<PlayerComponent>())
                {
                    state = State.Chase;
                    return;
                }
            }

            if(patrolTimer.TimerEnd(updateEverySecs))
            {
                movementX = -movementX;
            }
            movement.velocity.x = movementX;
        }

        void UpdateChase()
        {
            Vector2 curretPosition = this.transform.position;

            var dist = curretPosition - previousPosition;
            if (Vector2.SqrMagnitude(dist) < minAdvanceDistance * minAdvanceDistance)
            {
                movementX = -movementX;
            }
            movement.velocity.x = movementX;
            previousPosition = curretPosition;
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            var player = collision.gameObject.GetComponent<PlayerComponent>();
            if (player)
            {
                SceneManagerComponent.instance.RestartScene();
            }
        }
    }
}

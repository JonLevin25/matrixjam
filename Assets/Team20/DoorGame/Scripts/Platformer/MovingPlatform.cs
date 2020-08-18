using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team20
{
    public class MovingPlatform : MonoBehaviour
    {
        [SerializeField] private Transform _targetA, _targetB;
        private Transform _currentTarget;
        [SerializeField] private float _speed = 1.0f;


        void Start()
        {
            _currentTarget = _targetB;
        }

        void FixedUpdate()
        {
            transform.position = Vector3.MoveTowards(transform.position, _currentTarget.position, _speed * Time.fixedDeltaTime);
            if (transform.position == _targetB.position)
            {
                _currentTarget = _targetA;
            }
            if (transform.position == _targetA.position)
            {
                _currentTarget = _targetB;
            }
        }

        public float GetSpeed()
        {
            float speed = (1);
            if (transform.position.x > _currentTarget.position.x)
            {
                speed = -1;
            }
            return speed;
        }

        public float GetDirection()
        {
            float speed = (1);
            if (transform.position.x > _currentTarget.position.x)
            {
                speed = -1;
            }
            return speed;
        }

        public float GetSpeedNew()
        {
            return _speed * Time.deltaTime;
        }

    }
}

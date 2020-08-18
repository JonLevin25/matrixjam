using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team20
{
    public class Player : MonoBehaviour
    {
        private CharacterController _controller;
        [SerializeField] private float _speed = 3f;
        [SerializeField] private bool _sprint = true;
        [SerializeField] private float _sprintBonus = 0f;
        [SerializeField] private float _gravity = 1f;
        [SerializeField] private float _jumpHeight = 15f;
        [SerializeField] private bool _autoMove = false;
        private bool _doorKeyReady = true;
        private bool _isDoubleJumping = false;
        private float _yVelocity;
        [SerializeField] private float _coinNumber = 0f;
        [SerializeField] private int _doorNumber = 0;
        [SerializeField] private float _doorYValue = 0;
        private Vector3 _velocity;
        private float _addedVelocity = 0f;
        private float _addedVelocityX = 0f;
        private float _addedVelocityY = 0f;
        [SerializeField] private GameObject _pointA, _pointB, _collectedDoor;
        private int _lives = 3;
        [SerializeField] private Vector3 _startingLocation;

        [SerializeField] private float _doorOffset = 1f;
        private float _doorYOffset = 0f;
        private enum LookDirection
        {
            Right,
            Left
        }

        [SerializeField] private LookDirection _lookDirection;
        // Start is called before the first frame update
        void Start()
        {
            _controller = GetComponent<CharacterController>();
            _startingLocation = transform.position;
            _lookDirection = LookDirection.Right;
        }

        // Update is called once per frame
        void Update()
        {
            float horizontalInput = (Input.GetAxis("Horizontal"));
            if (_autoMove)
            {
                horizontalInput = 1f;
            }
            Vector3 direction = new Vector3(horizontalInput + _sprintBonus + (_addedVelocityX /_speed), 0 + (_addedVelocityY / _speed), 0);
            _velocity = direction * _speed;

            if (Input.GetKeyDown(KeyCode.D))
                _lookDirection = LookDirection.Right;
            if (Input.GetKeyDown(KeyCode.A))
                _lookDirection = LookDirection.Left;

            if (_controller.isGrounded)
            {
                _addedVelocity = 0f;
                _addedVelocityX = 0f;
                _addedVelocityY = 0f;
                _yVelocity = -_gravity;
                _isDoubleJumping = false;
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    _yVelocity = _jumpHeight;
                }
                if (Input.GetKeyDown(KeyCode.E) && _doorNumber == 1 && PlayerStands())
                {
                    Debug.Log("Door Down");
                    _doorKeyReady = false;
                    SetDoors(0);
                    _collectedDoor.SetActive(true);
                    Vector3 playerLocation = gameObject.transform.position;
                    if (_lookDirection == LookDirection.Right)
                        _doorOffset = Mathf.Abs(_doorOffset);
                    else
                        _doorOffset = -Mathf.Abs(_doorOffset);
                    playerLocation.x = playerLocation.x + _doorOffset;
                    playerLocation.y = playerLocation.y + _doorYOffset;
                    _collectedDoor.transform.position = playerLocation;
                }
                if (Input.GetKeyUp(KeyCode.E) & _doorNumber == 0)
                    _doorKeyReady = true;
            }
            else
            {
                //if (Input.GetKeyDown(KeyCode.Space) && !_isDoubleJumping)
                //{
                //    _yVelocity = _jumpHeight + _gravity;
                //    _isDoubleJumping = true;
                //}
                _yVelocity -= _gravity;
            }
            _velocity.y = _yVelocity;

            _controller.Move(_velocity * Time.deltaTime);

            if (Input.GetKey(KeyCode.LeftShift) && horizontalInput!= 0 && _sprint)
            {
                _sprintBonus = 0.4f*(horizontalInput/Mathf.Abs(horizontalInput));
            }
            else
            {
                _sprintBonus = 0f;

            }

            float xBorder = transform.position.x;
            if (transform.position.x <= _pointA.transform.position.x)
            {
                xBorder = _pointA.transform.position.x;
            }
            else if (transform.position.x >= _pointB.transform.position.x)
            {
                xBorder = _pointB.transform.position.x;
            }
            transform.position = new Vector3(xBorder, transform.position.y, transform.position.z);

            if (transform.position.y < _pointB.transform.position.y)
            {
                Die();
            }
        }

        public void Jump()
        {
            StartCoroutine(JumpTime(_jumpHeight));
        }

        private IEnumerator JumpTime(float jumpHeight)
        {
            float jumpStart = 0;
            while (jumpStart < jumpHeight)
            {
                yield return new WaitForEndOfFrame();
                jumpStart++;
            }

        }

        public void Die()
        {
            _lives--;
            if (_lives > 0)
            {
                transform.position = _startingLocation;
            }
            else if (_lives < 0)
            {
                _lives = 0;
            }
        }

        public int GetLives()
        {
            return _lives;
        }
        public int GetDoors()
        {
            return _doorNumber;
        }

        public bool DoorKeyReady()
        {
            return _doorKeyReady;
        }

        public void SetDoorsY(float yValue)
        {
            _doorYValue = yValue;
        }

        public void StopRising()
        {
            _yVelocity = -_gravity;
        }

        public float GetCoinNumber()
        {
            return _coinNumber;
        }

        public void AddCoins(int number)
        {
            _coinNumber += number;
        }

        public void SetDoors(int number)
        {
            _doorNumber = number;
        }

        public void CollectDoor(GameObject door)
        {
            _collectedDoor = door;
            _doorYOffset = _doorYValue - gameObject.transform.position.y;
        }

        public bool PlayerGrounded()
        {
            return _controller.isGrounded;
        }

        public bool PlayerStands()
        {
            if (_velocity.x == 0)
            {
                return true;
            }
            return false;
        }

        public bool PlayerIsStill()
        {
            if(PlayerStands() && PlayerGrounded())
            {
                return true;
            }
            return false;
        }

        public void ExitPlatformVelocity(float velocity, float velocityX, float velocityY)
        {
            //if (Input.GetAxis("Horizontal") == 0f)
            //{
                _addedVelocity = velocity;
                _addedVelocityX = velocityX;
                _addedVelocityY = velocityY;
                //Debug.Log(Input.GetAxis("Horizontal"));
           // }

            Debug.Log(_addedVelocity);
        }

    }
}

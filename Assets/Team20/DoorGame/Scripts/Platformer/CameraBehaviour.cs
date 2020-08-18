using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team20
{
    public class CameraBehaviour : MonoBehaviour
    {

        [SerializeField] private bool _cameraOn;
        private Camera _camera;
        private float _fixedPosition;
        private enum Type { None, Main, Secondary }
        [SerializeField] private Type _type;
        [SerializeField] private GameObject _player;
        //private float _offsetHTemp = 0;
        private float _offsetVTemp = 0;
        [Space(5)]
        [Header("Smart Camera")]
        [Space(15)]
        [SerializeField] private bool _smartCameraOn;
        [SerializeField] private float _offsetH = 0;
        [SerializeField] private float _offsetV = 0;
        [SerializeField] private float _offsetTempRate = 0.2f;
        [SerializeField] private float _offsetTempReturnRate = 1.5f;
        [SerializeField] private float _offsetTempMaxDistance = 10f;
        [SerializeField] private bool _xBorders = false;
        [SerializeField] private bool _yBorders = false;
        [SerializeField] private GameObject _pointA, _pointB;

        private enum YFollowType
        {
            None,
            Smart,
            Fixed
        }
        [Space(15)]
        [SerializeField] private YFollowType _yFollowType;
        [Space(5)]
        [SerializeField] private float _yUpperLimit = 2f;
        //[SerializeField] private float _yUpperLimitRate = 2f;
        [SerializeField] private float _speed = 2f;
        [SerializeField] private float _ungroundedSpeed = 0.5f;
        private bool _cameraUpKey = false;
        private bool _cameraDownKey = false;
        private float _newY = 0;
        private float _lookCounter = 20f;
        private float _lookCounterTemp;
        private float _startFollowCounter = 20f;
        private float _startFollowCounterTemp;
        private bool _startFollowing = false;
        private int _currentFollowType;

        void Start()
        {
            _currentFollowType = (int)_yFollowType;
            _fixedPosition = transform.position.y;
            _camera = gameObject.GetComponent<Camera>();
            if (_type.ToString() == "Main")
            {
                _cameraOn = true;
            }
            else if (_type.ToString() == "Secondary")
            {
                _cameraOn = false;
            }
            _camera.enabled = _cameraOn;
            _lookCounterTemp = _lookCounter;
            _startFollowCounterTemp = _startFollowCounter;
        }

        void Update()
        {

            //Camera Switch
            if (Input.GetKeyDown(KeyCode.L) && !_cameraUpKey)
            {
                if (_type.ToString() == "Main" || _type.ToString() == "Secondary")
                {
                    EnableCamera();
                }
            }

            if (_smartCameraOn && _player != null)
            {
                SmartCameraTransform();
            }
        }


        public void SmartCameraTransform()
        {
            //Smart Camera Type Switch
            if (Input.GetKeyDown(KeyCode.V) && _currentFollowType < 2)
            {
                _currentFollowType++;
            }
            else if (Input.GetKeyDown(KeyCode.V))
            {
                _currentFollowType = 0;
            }

            Player player = _player.GetComponent<Player>();
            // Camera Look Keys
            if (Input.GetKey(KeyCode.S) && !_cameraUpKey && player.PlayerIsStill())
            {
                _lookCounterTemp--;
                _cameraDownKey = true;
                if (_offsetVTemp > -_offsetTempMaxDistance && _lookCounterTemp <= 0f)
                {
                    _offsetVTemp -= _offsetTempRate;
                }
            }
            else if (Input.GetKey(KeyCode.W) && !_cameraDownKey && player.PlayerIsStill())
            {
                _lookCounterTemp--;
                _cameraUpKey = true;
                if (_offsetVTemp < _offsetTempMaxDistance && _lookCounterTemp <= 0f)
                {
                    _offsetVTemp += _offsetTempRate;
                }
            }
            else
            {
                _lookCounterTemp = _lookCounter;
                if (_offsetVTemp > 0)
                {
                    if (_offsetVTemp - (_offsetTempRate * _offsetTempReturnRate) >= 0)
                    {
                        _offsetVTemp -= (_offsetTempRate * _offsetTempReturnRate);
                    }
                    else
                    {
                        _offsetVTemp = 0;
                    }

                }
                else if (_offsetVTemp < 0)
                {
                    if (_offsetVTemp + (_offsetTempRate * _offsetTempReturnRate) <= 0)
                    {
                        _offsetVTemp += (_offsetTempRate * _offsetTempReturnRate);
                    }
                    else
                    {
                        _offsetVTemp = 0;
                    }

                }
                _cameraUpKey = false;
                _cameraDownKey = false;
            }

            _newY = _fixedPosition + _offsetV + _offsetVTemp;

            if (_currentFollowType == (int)YFollowType.Smart)
            {
                if (_player.transform.position.y > _yUpperLimit)
                {
                    if (_startFollowCounterTemp <= 0) //_player.transform.position.y >= _yUpperLimit + 2f &&
                    {
                        _startFollowing = true;
                    }
                    _startFollowCounterTemp--;
                    transform.position = new Vector3(_player.transform.position.x + _offsetH, transform.position.y, transform.position.z);
                    _newY = _player.transform.position.y + _offsetVTemp; 
                    Vector3 yTarget = new Vector3(transform.position.x, _newY, transform.position.z);
                    if (_startFollowing && !player.PlayerGrounded())
                    {
                        //if (!player.PlayerGrounded())
                        //{
                        transform.position = Vector3.MoveTowards(transform.position, yTarget, _speed * _ungroundedSpeed * Time.fixedDeltaTime);
                        //}
                    }
                    else if (_startFollowing)
                    {
                        transform.position = Vector3.MoveTowards(transform.position, yTarget, _speed * Time.fixedDeltaTime);
                    }
                }
                else
                {
                    _startFollowing = false;
                    _startFollowCounterTemp = _startFollowCounter;
                    transform.position = new Vector3(_player.transform.position.x + _offsetH, transform.position.y, transform.position.z);
                    Vector3 yTarget = new Vector3(transform.position.x, _newY, transform.position.z);
                    transform.position = Vector3.MoveTowards(transform.position, yTarget, _speed * 1.4f * Time.fixedDeltaTime);
                }
            }
            else if (_currentFollowType == (int)YFollowType.Fixed)
            {
                transform.position = new Vector3(_player.transform.position.x + _offsetH, _player.transform.position.y + _offsetVTemp, transform.position.z);
            }
            else
            {
                transform.position = new Vector3(_player.transform.position.x + _offsetH, _fixedPosition + _offsetVTemp, transform.position.z);
            }

            if (_xBorders)
            {
                float xPosition = transform.position.x;
                if (transform.position.x >= _pointB.transform.position.x)
                {
                    xPosition = _pointB.transform.position.x;
                }
                else if (transform.position.x <= _pointA.transform.position.x)
                {
                    xPosition = _pointA.transform.position.x;
                }

                transform.position = new Vector3(xPosition, transform.position.y, transform.position.z);
            }

            if (_yBorders)
            {
                float yPosition = transform.position.y;
                if (transform.position.y >= _pointA.transform.position.y)
                {
                    yPosition = _pointA.transform.position.y;
                }
                transform.position = new Vector3(transform.position.x, yPosition, transform.position.z);
            }
        }
        public void EnableCamera()
        {
            _camera.enabled = !_camera.enabled;
        }
    }
}

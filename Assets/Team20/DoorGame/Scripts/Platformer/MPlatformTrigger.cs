using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team20
{
    public class MPlatformTrigger : MonoBehaviour
    {
        // Start is called before the first frame update

        private MovingPlatform _movingPlatform;
        private Vector3 _lastPos, _currentPosition;
        [SerializeField] private bool _debug = false;

        void Start()
        {
            _movingPlatform = GetComponentInParent<MovingPlatform>();
            _lastPos = new Vector3(_movingPlatform.transform.position.x, _movingPlatform.transform.position.y);
            _currentPosition = new Vector3(_movingPlatform.transform.position.x, _movingPlatform.transform.position.y);
        }

        void Update()
        {
            if(_currentPosition != _movingPlatform.transform.position)
            {
                _lastPos = _currentPosition;
                _currentPosition = new Vector3(_movingPlatform.transform.position.x, _movingPlatform.transform.position.y);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log("Enter");
                other.transform.parent = gameObject.transform.parent;
                
            }
        
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Player player = other.GetComponent<Player>();
                Debug.Log("EXIT");
               // _isColliding = false;
                other.transform.parent = null;
                if(player != null && _movingPlatform != null)
                {
                    float xVelocity = Mathf.Abs(_currentPosition.x - _lastPos.x);
                    float yVelocity = Mathf.Abs(_currentPosition.y - _lastPos.y);
                    float fullVelocity = Mathf.Sqrt(Mathf.Pow(xVelocity, 2) + Mathf.Pow(yVelocity, 2));
                    player.ExitPlatformVelocity(fullVelocity/Time.deltaTime*_movingPlatform.GetSpeed(), xVelocity / Time.deltaTime * _movingPlatform.GetSpeed(), yVelocity / Time.deltaTime * _movingPlatform.GetSpeed());
                    Debug.Log(fullVelocity/Time.deltaTime + "is it " + _movingPlatform.GetSpeed());
                }
                 
            }
        
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team20
    {
        namespace MatrixJam.Team20
{
    public class MPlatformTrigger2D : MonoBehaviour
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


            private void OnTriggerEnter2D(Collider2D other)
            {
                if (other.CompareTag("Tag0"))
                {
                    Debug.Log("Enter");
                    other.transform.parent = gameObject.transform.parent;
                    other.GetComponent<PlayerComponent>().OnMovingPlatform(true);
                }
            
            }

            private void OnTriggerExit2D(Collider2D other)
            {
                if (other.CompareTag("Tag0"))
                {
            //        PlayerComponent player = other.GetComponent<PlayerComponent>();
                    Debug.Log("EXIT");
                   // _isColliding = false;
                    other.transform.parent = null;
                    other.GetComponent<PlayerComponent>().OnMovingPlatform(false);
            //        if(player != null && _movingPlatform != null)
            //        {
            //            float xVelocity = Mathf.Abs(_currentPosition.x - _lastPos.x);
            //            float yVelocity = Mathf.Abs(_currentPosition.y - _lastPos.y);
            //            float fullVelocity = Mathf.Sqrt(Mathf.Pow(xVelocity, 2) + Mathf.Pow(yVelocity, 2));
            //            player.ExitPlatformVelocity(fullVelocity/Time.deltaTime*_movingPlatform.GetSpeed(), xVelocity / Time.deltaTime * _movingPlatform.GetSpeed(), yVelocity / Time.deltaTime * _movingPlatform.GetSpeed());
            //            Debug.Log(fullVelocity/Time.deltaTime + "is it " + _movingPlatform.GetSpeed());
            //        }
                     
                }
            
            }
        }
    }
}

using UnityEngine;

namespace MatrixJam.Team19.Input
{

    /// <summary>
    /// PlayerInput
    /// 
    /// Allows consumers to get delayed information.
    /// Handles cases where keys are pressed mid-movement, but still need to register.
    /// </summary>

    [CreateAssetMenu(fileName = "Player Input", menuName = "Input Handlers/Player Input")]
    public class PlayerInput : Base.BaseInputHandler
    {
        private Team19.DataStructures.FixedSizedQueue<Vector3> _inputQueue;
        
        public override bool IsInputAvailable
        {
            get
            {
                return _inputQueue.Count > 0;
            }
        }

        public PlayerInput()
        {
            _inputQueue = new Team19.DataStructures.FixedSizedQueue<Vector3>(5);
        }

        public override Vector3 GetNextDirection()
        {
            return _inputQueue.Dequeue();
        }

        public override void UpdateInput()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.W) || UnityEngine.Input.GetKeyDown(KeyCode.UpArrow)) {
                _inputQueue.Enqueue(Vector3.forward);
            }
            else if (UnityEngine.Input.GetKeyDown(KeyCode.S) || UnityEngine.Input.GetKeyDown(KeyCode.DownArrow)) {
                _inputQueue.Enqueue(Vector3.back);
            }
            else if (UnityEngine.Input.GetKeyDown(KeyCode.A) || UnityEngine.Input.GetKeyDown(KeyCode.LeftArrow)) {
                _inputQueue.Enqueue(Vector3.left);
            }
            else if (UnityEngine.Input.GetKeyDown(KeyCode.D) || UnityEngine.Input.GetKeyDown(KeyCode.RightArrow)) {
                _inputQueue.Enqueue(Vector3.right);
            }
        }

    }
}

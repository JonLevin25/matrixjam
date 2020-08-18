using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace MatrixJam.Team19.Gameplay.Controls
{

    public class CreatureController : MonoBehaviour
    {
        [SerializeField]
        private float _stepDistance = 1f;

        [SerializeField]
        private float _stepTimeSeconds = 1f;

        [SerializeField]
        private LayerMask _stepDenyingLayerMask;

        [SerializeField]
        private Team19.Input.Base.BaseInputHandler _inputHandlerPrefab;

        [SerializeField]
        private bool _shouldCloneInputHandler = false;

        private Team19.Input.Base.BaseInputHandler _inputHandler;

        private bool _isReadyToStep = true;


        private Vector3 _activeStepDirectionVector = Vector3.zero;
        private Vector3 _activeStepOrigin = Vector3.zero;
        private Vector3 _activeStepDestination = Vector3.zero;

        private float _activeStepProgress = 0;

        private Collider _collider;

        private void Awake()
        {
            if (_shouldCloneInputHandler)
            {
                _inputHandler = Instantiate(_inputHandlerPrefab);
            }
            else
            {
                _inputHandler = _inputHandlerPrefab;
            }

            _collider = GetComponent<Collider>();
            if (_collider == null)
            {
                _collider = GetComponentInChildren<Collider>();
            }
        }

        private void Update()
        {
            _inputHandler.UpdateInput();
        }

        private void FixedUpdate()
        {
            UpdateMovement(Time.deltaTime);
        }

        private void UpdateMovement(float deltaTime)
        {
            if (_inputHandler.IsInputAvailable && _isReadyToStep)
            {
                Vector3 nextLocalDirectionVector = _inputHandler.GetNextDirection();
                Vector3 nextWorldDirectionVector = transform.TransformDirection(nextLocalDirectionVector);
                Vector3 nextStepDestination = transform.position + (nextWorldDirectionVector * _stepDistance);

                if (CanPerformNextStep(nextStepDestination))
                {
                    UpdateActiveStepData(nextWorldDirectionVector);
                }
            }

            if (_isReadyToStep == false && _activeStepDirectionVector != Vector3.zero)
            {
                float unitPerSecond = (_stepDistance / _stepTimeSeconds);
                _activeStepProgress += unitPerSecond * deltaTime;
                transform.position = Vector3.Lerp(_activeStepOrigin, _activeStepDestination, _activeStepProgress);
            }

            if (_activeStepProgress > 0.98f) {
                EndStep();
            }
        }

        private void UpdateActiveStepData(Vector3 directionVector)
        {
            _activeStepDirectionVector = directionVector;

            _activeStepOrigin = transform.position;
            _activeStepDestination = _activeStepOrigin + (directionVector * _stepDistance);

            _activeStepProgress = 0;
            _isReadyToStep = false;
        }

        protected virtual void EndStep()
        {
            transform.position = _activeStepDestination;
            _activeStepDirectionVector = Vector3.zero;

            _isReadyToStep = true;
        }

        private bool CanPerformNextStep(Vector3 _destination)
        {
            return ! (Physics.OverlapBox(_destination, _collider.bounds.extents, transform.rotation, _stepDenyingLayerMask.value).Length > 0);
        }
    }
}

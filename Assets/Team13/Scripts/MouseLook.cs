using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team13
{
    public class MouseLook : MonoBehaviour{
        
		private float _mouseX;
		private float _mouseY;
		[SerializeField] private float _mouseSensetivity = 100f;
		[SerializeField] private Transform _body;
		private float _xRot = 0f;

		private CursorLockMode _origCursorLockMode;

		void Start(){
			_origCursorLockMode = Cursor.lockState;
			Cursor.lockState = CursorLockMode.Locked;
		}

		void OnDestroy(){
			Cursor.lockState = _origCursorLockMode;
		}

		void Update(){
			_mouseX = Input.GetAxis("Mouse X") * _mouseSensetivity * Time.deltaTime;
			_mouseY = Input.GetAxis("Mouse Y") * _mouseSensetivity * Time.deltaTime;

			_xRot -= _mouseY;
			_xRot = Mathf.Clamp(_xRot, -90f, 90f);

			transform.localRotation = Quaternion.Euler(_xRot, 0f, 0f);
			_body.Rotate(Vector3.up * _mouseX);
		}
    }
}

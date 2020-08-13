using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace MatrixJam.Team13
{
    public class PlayerMovement : MonoBehaviour{
        private float _x;
		private float _z;
		private Vector3 _move;
		private Vector3 _prevVelocity;

		private enum moveType {Walking, Running, Sneaking};
		private moveType _moveState = moveType.Walking;

		[SerializeField] private CharacterController _controller;
		[SerializeField] private float _walkSpeed = 10f;
		[SerializeField] private float _runSpeed = 15f;
		[SerializeField] private float _sneakSpeed = 5f;
		private float _speed;

		[SerializeField] private UnityEvent _OnSneak;
		[SerializeField] private UnityEvent _onWalk;
		[SerializeField] private UnityEvent _onRun;

		[SerializeField] private UnityEvent _onMoveStart;
		[SerializeField] private UnityEvent _onMove;
		[SerializeField] private UnityEvent _onMoveEnd;

		void Start(){
			_speed = _walkSpeed;
			_onWalk.Invoke();
			_prevVelocity = Vector3.zero;
		}
        
        void Update(){
			_x = 0;
			_z = 0;
			if(Input.GetKey(KeyCode.W)){
				_z += 1;
			}
			if(Input.GetKey(KeyCode.S)){
				_z -= 1;
			}
			if(Input.GetKey(KeyCode.A)){
				_x -= 1;
			}
			if(Input.GetKey(KeyCode.D)){
				_x += 1;
			}
			//_x = Input.GetAxis("Horizontal");
			//_z = Input.GetAxis("Vertical");

			if(Input.GetKeyDown(KeyCode.LeftControl)){
				if(_moveState == moveType.Sneaking){
					_moveState = moveType.Walking;
					_speed = _walkSpeed;
					_onWalk.Invoke();
				}else{
					_moveState = moveType.Sneaking;
					_speed = _sneakSpeed;
					_OnSneak.Invoke();
				}
			}

			if(Input.GetKeyDown(KeyCode.LeftShift)){
				if(_moveState == moveType.Running){
					_moveState = moveType.Walking;
					_speed = _walkSpeed;
					_onWalk.Invoke();
				}else{
					_moveState = moveType.Running;
					_speed = _runSpeed;
					_onRun.Invoke();
				}
			}
			
			_move = transform.right * _x + transform.forward * _z;

			_controller.Move(_move * _speed * Time.deltaTime);

			
			if(_controller.velocity != Vector3.zero){
				if(_prevVelocity == Vector3.zero){
					//Debug.Log("Starting move");
					_onMoveStart.Invoke();
				}else{
					//Debug.Log("Moving");
					_onMove.Invoke();
				}
			}else{
				if(_prevVelocity != Vector3.zero){
					//Debug.Log("Ending Move");
					_onMoveEnd.Invoke();
				}
			}

			_prevVelocity = _controller.velocity;
        }

		public void ShowUiActive(Image img){
			img.color = Color.red;
		}

		public void ShowUiInactive(Image img){
			img.color = Color.gray;
		}

		public void MoveToPosition(Transform pos){
			transform.position = pos.position;
		}
    }
}

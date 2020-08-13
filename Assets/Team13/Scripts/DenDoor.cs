using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MatrixJam.Team13
{
    public class DenDoor : MonoBehaviour {
        
		[SerializeField] private UnityEvent _onComing;
		[SerializeField] private UnityEvent _onLeaving;
		[SerializeField] private Sprite _spriteUp;
		[SerializeField] private Sprite _spriteDown;
		[SerializeField] private Sprite _spriteLeft;
		[SerializeField] private Sprite _spriteRight;
		[SerializeField] private Sprite _spriteA;
		[SerializeField] private Sprite _spriteB;

		[SerializeField] private DoorLock[] _lock;

		[SerializeField] private GameObject _endScreen;
		[SerializeField] private Exit _exit;

		private KeyCode[] _keyPresses = new KeyCode[10];

		private bool _isRecording = false;
		private bool _codeIsGood = false;

		private KeyCode[] _correctCode = {KeyCode.UpArrow, KeyCode.UpArrow,
											KeyCode.DownArrow, KeyCode.DownArrow,
											KeyCode.LeftArrow, KeyCode.RightArrow,
											KeyCode.LeftArrow, KeyCode.RightArrow,
											KeyCode.A, KeyCode.B};
		
		void OnTriggerEnter(Collider collider){
			if(!_codeIsGood){
				_onComing.Invoke();
				_isRecording = true;
			}
		}

		void OnTriggerExit(Collider collider){
			if(!_codeIsGood){
				_onLeaving.Invoke();
				_isRecording = false;
			}
		}

		void Update(){
			if(_isRecording){
				//Debug.Log("Recording");
				if(Input.GetKeyDown(KeyCode.UpArrow)){
					PressKey(KeyCode.UpArrow, _spriteUp);
				}else if(Input.GetKeyDown(KeyCode.DownArrow)){
					PressKey(KeyCode.DownArrow, _spriteDown);
				}else if(Input.GetKeyDown(KeyCode.LeftArrow)){
					PressKey(KeyCode.LeftArrow, _spriteLeft);
				}else if(Input.GetKeyDown(KeyCode.RightArrow)){
					PressKey(KeyCode.RightArrow, _spriteRight);
				}else if(Input.GetKeyDown(KeyCode.A)){
					PressKey(KeyCode.A, _spriteA);
				}else if(Input.GetKeyDown(KeyCode.B)){
					PressKey(KeyCode.B, _spriteB);
				}
			}
		}

		private bool IsCodeGood(){
			//Debug.Log("Next try");
			for(int i = 0; i < _keyPresses.Length; i++){
				//Debug.Log(_keyPresses[i] + " -> " + _correctCode[i]);
				if(_keyPresses[i] != _correctCode[i]){
					//Debug.Log("Code is bad");
					return false;
				}
			}
			//Debug.Log("Code is good");
			
			return true;
		}

		private void PressKey(KeyCode key, Sprite sprite){
			for(int i = _lock.Length - 1; i > 0; i--){
				_lock[i].SetSprite(_lock[i - 1].sprite);
			}
			_lock[0].SetSprite(sprite);

			for(int i = 0; i < 9; i++){
				_keyPresses[i] = _keyPresses[i + 1];
			}
			_keyPresses[9] = key;
			if(IsCodeGood()){
				GetComponent<Animation>().Play();
				StartCoroutine(CountTo10());
			}
		}

		private IEnumerator CountTo10(){
			yield return new WaitForSeconds(5);
			_endScreen.SetActive(true);
			yield return new WaitForSeconds(5);
			_exit.EndLevel();
		}
    }
}

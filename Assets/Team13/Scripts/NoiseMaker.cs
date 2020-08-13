using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team13
{
	[RequireComponent(typeof(AudioSource))]
	public class NoiseMaker : MonoBehaviour {
        //[SerializeField] private AudioClip _walking;

		[SerializeField] private float _sneakingNoiseRadius;
		[SerializeField] private float _walkingNoiseRadius;
		[SerializeField] private float _runningNoiseRadius;
		//[SerializeField] private SphereCollider _noiseSphere;
		//[SerializeField] private LayerMask _charactersLayer;

		private bool _makingNoise;
		public bool makingNoise{
			get{
				return _makingNoise;
			}
		}

		public float radius{
			get{
				return _radius;
			}
		}

		private AudioSource _audioSource;

		private float _radius;

		void Awake(){
			_audioSource = GetComponent<AudioSource>();
		}

		public void Sneak(){
			Debug.Log("Sneaking");
			_radius = _sneakingNoiseRadius;
			//_audioSource.pitch = 0.2f;
			_audioSource.volume = 0.2f;
			//_noiseSphere.radius = _radius;
		}

		public void Walk(){
			Debug.Log("Walking");
			_radius = _walkingNoiseRadius;
			//_audioSource.pitch = 0.8f;
			_audioSource.volume = 0.4f;
			//_noiseSphere.radius = _radius;
		}

		public void Run(){
			Debug.Log("Running");
			_radius = _runningNoiseRadius;
			//_audioSource.pitch = 1.2f;
			_audioSource.volume = 0.6f;
			//_noiseSphere.radius = _radius;
		}

		public void PlayMoveSound(){
			if(!_audioSource.isPlaying){
				_audioSource.Play();
			}
		}

		public void StartMove(){
			_makingNoise = true;
		}

		public void EndMove(){
			_makingNoise = false;
		}

		void OnDrawGizmosSelected(){
			if(_makingNoise){
				if(_radius + Mathf.Epsilon >= _runningNoiseRadius){
					Gizmos.color = Color.red;
					Gizmos.DrawWireSphere(transform.position, _runningNoiseRadius);
				}else if(_radius + Mathf.Epsilon >= _walkingNoiseRadius){
					Gizmos.color = Color.yellow;
					Gizmos.DrawWireSphere(transform.position, _walkingNoiseRadius);
				}else if(_radius + Mathf.Epsilon >= _sneakingNoiseRadius){
					Gizmos.color = Color.green;
					Gizmos.DrawWireSphere(transform.position, _sneakingNoiseRadius);
				}
			}
		}
    }
}
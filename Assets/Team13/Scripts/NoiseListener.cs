using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MatrixJam.Team13
{
    public class NoiseListener : MonoBehaviour {
		
		[SerializeField] private UnityEvent _onNoiseHeardStart;
		[SerializeField] private UnityEvent _onNoiseHeardProgress;
		[SerializeField] private UnityEvent _onNoiseHeardEnd;

		[SerializeField] private float _hearingRadius;
		[SerializeField] private NoiseMaker[] _listeningTargets;

		private bool _heardNoise;
		private Vector3 _noisePosition;
		[SerializeField] private float _newNoiseDistanceDismiss;

		void Awake(){

		}

		void OnDrawGizmosSelected(){
			Gizmos.color = Color.blue;
			Gizmos.DrawWireSphere(transform.position, _hearingRadius);

			if((_listeningTargets[0].transform.position - transform.position).sqrMagnitude < (_listeningTargets[0].radius + _hearingRadius) * (_listeningTargets[0].radius + _hearingRadius)){
				Gizmos.color = Color.magenta;
				Gizmos.DrawLine(transform.position, _listeningTargets[0].transform.position);
				//Debug.Break();
			}else{
				Gizmos.color = Color.cyan;
				Gizmos.DrawLine(transform.position, _listeningTargets[0].transform.position);
			}
		}
		
		void FixedUpdate(){
			for(int i = 0; i < _listeningTargets.Length; i++){
				if(_listeningTargets[i].makingNoise){
					if((_listeningTargets[i].transform.position - transform.position).sqrMagnitude < (_listeningTargets[i].radius + _hearingRadius) * (_listeningTargets[i].radius + _hearingRadius)){
						//Debug.Log("I can hear " + _listeningTargets[i].gameObject.name);
						if(_listeningTargets[i].makingNoise){
							if(_heardNoise){
								if((_listeningTargets[i].transform.position - _noisePosition).sqrMagnitude <= _newNoiseDistanceDismiss){
									_onNoiseHeardProgress.Invoke();
								}else{
									_heardNoise = true;
									_noisePosition = _listeningTargets[i].transform.position;
									_onNoiseHeardStart.Invoke();
								}
							}else{
								_heardNoise = true;
								_noisePosition = _listeningTargets[i].transform.position;
								_onNoiseHeardStart.Invoke();
							}
							break;
						}else{
							continue;
						}
					}else{
						if(_heardNoise){
							_heardNoise = false;
							_onNoiseHeardEnd.Invoke();
						}
					}
				}
			}
			/*Collider[] cols = Physics.OverlapSphere(transform.position, _hearingRadius, _charactersLayer);
			
			if(cols.Length > 0){
				Debug.Log("I can hear "+cols[0].name);
				if(cols[0].GetComponent<NoiseMaker>().makingNoise){
					if(_heardNoise){
						if((cols[0].transform.position - _noisePosition).sqrMagnitude <= _newNoiseDistanceDismiss * _newNoiseDistanceDismiss){
							_onNoiseHeardProgress.Invoke();
						}else{
							_heardNoise = true;
							_noisePosition = cols[0].transform.position;
							_onNoiseHeardStart.Invoke();
						}
					}else{
						_heardNoise = true;
						_noisePosition = cols[0].transform.position;
						_onNoiseHeardStart.Invoke();
					}
				}
			}else{
				if(_heardNoise){
					_heardNoise = false;
					_onNoiseHeardEnd.Invoke();
				}
			}*/
		}
    }
}

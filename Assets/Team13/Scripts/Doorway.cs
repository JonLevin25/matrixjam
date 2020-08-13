using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team13
{
    public class Doorway : MonoBehaviour {
		[SerializeField] private Exit _exit;

		[SerializeField] private GameObject _exitScreen;

		private void OnTriggerEnter(Collider collider){
			Debug.Log(collider.gameObject.name + " has left the building!");
			_exitScreen.SetActive(true);
			StartCoroutine(CountTo5());
		}

		private IEnumerator CountTo5(){
			yield return new WaitForSeconds(5);
			_exit.EndLevel();
		}
    }
}

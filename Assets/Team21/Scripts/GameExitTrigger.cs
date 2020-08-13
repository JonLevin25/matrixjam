using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MatrixJam.Team21 {
	public class GameExitTrigger : MonoBehaviour {
    [SerializeField] private int exitNum;

    [SerializeField] private UnityEvent exitEvent;
    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    private void OnTriggerEnter(Collider other) {
        Debug.Log("Trigger Enter" + exitNum);
        exitEvent.Invoke();
    }

}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MatrixJam.Team8
{
    public class EndGame : MonoBehaviour
    {
        [SerializeField] private int exitNum;

        [SerializeField] private UnityEvent exitEvent;

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                Debug.Log("Trigger Enter" + exitNum);
                exitEvent.Invoke();
            }
        }
    }
}

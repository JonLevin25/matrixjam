using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MatrixJam.Team9
{
    public class GameOverTrig : MonoBehaviour
    {
        // Start is called before the first frame update
        [SerializeField] private int exitNum;

        [SerializeField] private UnityEvent exitEvent;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void ExitInvoke()
        {
            exitEvent.Invoke();
        }
    }
}

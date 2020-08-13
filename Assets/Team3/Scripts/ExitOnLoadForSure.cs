using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MatrixJam.Team3
{
    public class ExitOnLoadForSure : MonoBehaviour
    {
        [SerializeField] private UnityEvent exitEvent;
        // Start is called before the first frame update
        void Start()
        {
            
        }

        private void OnEnable()
        {
            exitEvent.Invoke();
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}

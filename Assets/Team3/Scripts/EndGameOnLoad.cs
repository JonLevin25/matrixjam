using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MatrixJam.Team3
{
    public class EndGameOnLoad : MonoBehaviour
    {
        
        [SerializeField] private int exitNum;

        [SerializeField] private ARGManager manager;
        
        // Start is called before the first frame update
        void Start()
        {
           // manager.handleExit(exitNum);

        }


        private void OnEnable()
        {
            manager.handleExit(exitNum);
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}

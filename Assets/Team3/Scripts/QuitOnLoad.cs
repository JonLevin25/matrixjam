using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team3
{
    public class QuitOnLoad : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            
        }
        private void OnEnable()
        {
            Debug.Log("Quit1");
            Application.Quit();
        }

        
        public void quit()
        {
            Debug.Log("Quit2");
            Application.Quit();
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}

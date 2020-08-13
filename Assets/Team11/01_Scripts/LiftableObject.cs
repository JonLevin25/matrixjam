using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MatrixJam.Team11
{

    public class LiftableObject : MonoBehaviour
    {
        [SerializeField] bool isTutorialKey;
        public bool isLifted = false;



        // Start is called before the first frame update
        void Start()
        {

        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(isTutorialKey && collision.GetComponent<PlayerController>())
            {
                isTutorialKey = false;
                FindObjectOfType<TutorialManager>().PlayerTouchedKey();
            }
        }





        // Update is called once per frame
        void Update()
        {
            
        }

        private void OnDisable()
        {
        }



    }
}

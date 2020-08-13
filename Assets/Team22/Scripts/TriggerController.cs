using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team22
{
    public class TriggerController : MonoBehaviour
    {
        private bool inTrigger = false;
        private GameObject insideBamboo;

        /*
        private void Update()
        {
            print(inTrigger);
        }
        */

        public void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Tag0"))
            {
                inTrigger = true;
                insideBamboo = collision.gameObject;
            }
        }

        public void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Tag0"))
            {
                inTrigger = false;
                insideBamboo = null;
            }
        }

        public bool GetTriggerStatus()
        {
            return inTrigger;
        }

        public void DestroyInside()
        {
            if(insideBamboo != null)
            {
                insideBamboo.SendMessage("Slice", SendMessageOptions.DontRequireReceiver);
            }
            else
            {
                Debug.LogError("No bamboo reference found inside trigger. This should not happen.");
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam
{
    public class Entrance : Portal
    {
        public void Enter()
        {
            if (GetComponent<StartHelper>() != null)
            {
                GetComponent<StartHelper>().StartHelp(num_portal);
            }
            else
            {
                Debug.Log("No StartHelper found on entrance!");
               
            }
        }

    }
}
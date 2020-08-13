using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam
{
    public class StartHelper : MonoBehaviour
    {
            public virtual void StartHelp(int num_ent)
            {
              //override this for your starting of game function(s).
              Debug.Log("Start the game at " + num_ent);
            }
    }
}

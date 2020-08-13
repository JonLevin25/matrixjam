using System.Collections;
using System.Collections.Generic;
using MatrixJam;
using UnityEngine;

namespace MatrixJam.Team10
{
    public class StartGame : StartHelper
    {
        public override void StartHelp(int num_ent)
        {
            // this is how the game starts
            Debug.Log("game start... please enter name");
            base.StartHelp(num_ent);
        }
        
        // Start is called before the first frame update
        void Start()
        {
        }

        public void OnSubmit()
        {
        }
    }
}

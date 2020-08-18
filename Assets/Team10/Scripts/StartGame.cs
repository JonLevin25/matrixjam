using System.Collections;
using System.Collections.Generic;
using MatrixJam;
using UnityEngine;
using UnityEngine.UI;

namespace MatrixJam.Team10
{
    public class StartGame : StartHelper
    {
        public override void StartHelp(int num_ent)
        {
            base.StartHelp(num_ent);
            FindObjectOfType<EndGame>().startScene();
            // FindObjectOfType<EndGame>().testDeath(6); //startScene();
        }
        
        // Start is called before the first frame update
        void Start(){}
    }
}

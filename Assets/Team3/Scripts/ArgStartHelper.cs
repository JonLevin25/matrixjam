using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team3
{
    public class ArgStartHelper : StartHelper
    {
        [SerializeField] private ARGManager manager;
        public override void StartHelp(int num_ent)
        {
            // this is how the game starts
            Debug.Log("Player Entered ARGG through entrance number:" + num_ent);
            //manager.InitialObjects[num_ent].SetActive(true);
            manager.StartLevel(num_ent);
            
        }
        
        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}

using UnityEngine;

namespace MatrixJam.Team5
{
    public class GuessTheCodeStartHelper : StartHelper
    {
        public GameManager Manager;

        public Data data;
        
        public override void StartHelp(int num_ent)
        {
            Debug.Log(num_ent);
            // this is how the game starts
            Manager.Init(data);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team22
{
    public class RhythmStartHelper : StartHelper
    {
        public override void StartHelp(int num_ent)
        {
            GameManager.instance.Setup();
        }
    }
}

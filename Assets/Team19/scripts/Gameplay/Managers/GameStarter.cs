using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team19.Gameplay.Managers
{
    public class GameStarter : StartHelper
    {
        public override void StartHelp(int num_ent)
        {
            LevelManager.Instance.NotifyLevelStarted(this, num_ent);
        }
    }
}

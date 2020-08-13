using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MatrixJam.Team24
{
    public class Exit : Portal
    {
        public void EndLevel()
        {
            LevelHolder.Level.ExitLevel(this);
        }
    }
}

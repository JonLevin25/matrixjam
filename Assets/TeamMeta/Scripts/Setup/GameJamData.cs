using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.TeamMeta
{
    public static class GameJamData
    {
        private static int _teamNumber;
        public static int TeamNumber
        {
            get { return _teamNumber;}
            set { _teamNumber = value; }
        }
        
        public static string TeamString
        {
            get;
            set;
        }
        
        public static string TeamName
        {
            get { return "Team"+TeamNumber; }
        }
    }
}

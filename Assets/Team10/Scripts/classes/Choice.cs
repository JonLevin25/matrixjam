using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team10
{
    public delegate void Action();
    public class Choice
    {
        public string info;
        public Action action;

        public Choice(){}
        public Choice(string i, Action a){
            info = i;
            action = a;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team10
{
    public class Dialogue
    {
        public string name;
        [TextArea(3, 10)]
        public string sentences;
        public int[] choices;

        public Dialogue(){}

        public Dialogue(string n, string s){
            name = n;
            sentences = s;
        }

        public Dialogue(string n, int[] c){
            name = n;
            choices = c;
        }
    }
}

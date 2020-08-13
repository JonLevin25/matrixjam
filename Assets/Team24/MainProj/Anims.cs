using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team24
{
    public class Anims : MonoBehaviour
    {
        public static Anims instance;

        public Sprite[] mgvAnim = new Sprite[17];
        public Sprite[] copAnim = new Sprite[11];
        public Sprite[] playerAnimRight;
        public Sprite[] playerAnimLeft;

        public Sprite protesterIdle;
        public Sprite protesterFlower;

        public Sprite unicorn;

        private void Start()
        {
            instance = this;
        }


    }
}

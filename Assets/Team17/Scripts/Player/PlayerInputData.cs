using UnityEngine;

namespace MatrixJam.Team17
{
    [CreateAssetMenu(menuName = "TheFlyingDragons/PlayerInputData")]
    public class PlayerInputData : ScriptableObject
    {
        public Vector3 move;
        public bool jump;
        public bool jumpReady;
        public float jumpTime;
        //public bool crouch;
        [Space]
        public bool fire1;
        public bool fire2;
        [Space]
        public bool useItem;
        //public bool cycleNext;
        //public bool cyclePrev;

        public void Reset()
        {
            move = Vector3.zero;
            jump = false;
            jumpReady = false;
            jumpTime = 0f;
            //crouch = false;

            fire1 = false;
            fire2 = false;

            useItem = false;
            //cycleNext = false;
            //cyclePrev = false;
        }
    }
}

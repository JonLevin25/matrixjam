using UnityEngine;

namespace MatrixJam.Team19.Input
{
    [CreateAssetMenu(fileName = "Lane Walker Input", menuName = "Input Handlers/Lane Walker")]
    public class LaneWalkerInput : Base.BaseInputHandler 
    {
        public override bool IsInputAvailable
        {
            get
            {
                return true;
            }
        }

        public override Vector3 GetNextDirection()
        {
            return Vector3.forward;
        }

        public override void UpdateInput()
        {
            return;
        }
    }
}

using UnityEngine;

namespace MatrixJam.Team19.Gameplay.Controls
{

    public class LaneWalkerController : CreatureController
    {
        [SerializeField]
        private int _stepLimit;

        private int _stepsTaken;

        protected override void EndStep()
        {
            base.EndStep();

            _stepsTaken ++;

            if (_stepsTaken == _stepLimit)
            {
                EndController();
            }
        }

        private void EndController()
        {
            Destroy(gameObject);
        }
    }
}

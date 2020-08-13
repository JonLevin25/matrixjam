using UnityEngine;

namespace MatrixJam.Team14
{
    public class PlaySoundOnState : StateMachineBehaviour
    {
        [SerializeField] private TrainMove onStateEnter = TrainMove.None;
        [SerializeField] private TrainMove onStateExit = TrainMove.None;
        
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex)
        {
            TrainController.Instance.PlaySFX(onStateEnter);
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex)
        {
            TrainController.Instance.PlaySFX(onStateExit);
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex)
        {
        }

        public override void OnStateMove(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex)
        {
        }

        public override void OnStateIK(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex)
        {
        }
    }
}
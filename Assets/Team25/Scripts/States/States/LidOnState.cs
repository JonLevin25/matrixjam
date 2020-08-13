using MatrixJam.Team25.Scripts.Jar;
using MatrixJam.Team25.Scripts.Managers;
using UnityEngine;

namespace MatrixJam.Team25.Scripts.States.States
{
    public class LidOnState : IState
    {
        private readonly DragMove jarDrag, lidDrag;
        private readonly LidTwist lidTwist;
        private DataManager dataManager;
        private GameManager gameManager;
        public LidOnState(GameObject jarGameObject, GameObject lidGameObject)
        {
            dataManager = GameObject.FindObjectOfType<DataManager>();
            gameManager = GameObject.FindObjectOfType<GameManager>();
            jarDrag = jarGameObject.GetComponent<DragMove>();
            lidTwist = lidGameObject.GetComponent<LidTwist>();
            lidDrag = lidGameObject.GetComponent<DragMove>();
        }
        public void Tick()
        {
            
        }

        public void OnEnter()
        {
            jarDrag.enabled = false;
            jarDrag.arrows.SetActive(false);
            lidDrag.enabled = false;
            lidTwist.enabled = true;
        }

        public void OnExit()
        {
            
        }
    }
}

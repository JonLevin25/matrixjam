using UnityEngine;

namespace MatrixJam.Team17
{
    public abstract class Grappler : MonoBehaviour, IPlayerInventoryItem, ITrigger
    {
        public GrapplerConfig grapplerConfig;
        
        public Player Owner { get; set; }
        public bool IsPassive => false;

        public abstract void TriggerDown();
        public abstract void TriggerUp();
        public abstract void ChangeLength(float delta);
        public abstract void Detach();
    }
}
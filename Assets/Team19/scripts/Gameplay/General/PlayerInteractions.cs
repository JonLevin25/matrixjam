using UnityEngine;
using MatrixJam.Team19.Gameplay.Managers;

namespace MatrixJam.Team19.Gameplay.General
{
    public class PlayerInteractions : MonoBehaviour
    {
        [SerializeField]
        private LayerMask _levelLostLayerMask;

        [SerializeField]
        private LayerMask _levelPassedLayerMask;

        [SerializeField]
        private LayerMask _collectibleMask;
        [SerializeField]
        public GameObject pillview;

        static private bool _wasKeyPickedThisLevel = false;

        static public bool WasKey
        {
            set
            {
                if(GameObject.FindObjectOfType<PlayerInteractions>()!=null)
                {
                    GameObject.FindObjectOfType<PlayerInteractions>().pillview.SetActive(value);
                }
                    _wasKeyPickedThisLevel = value;
            }
            get
            {
                return _wasKeyPickedThisLevel;
            }
        }
        
        private void OnCollisionEnter(Collision collision)
        {
            bool isCollidedLayerInLevelLostMask = (_levelLostLayerMask.value == (_levelLostLayerMask.value | (1 << collision.gameObject.layer)));

            if (isCollidedLayerInLevelLostMask)
            {
                LevelManager.Instance.NotifyLevelLost();
            }

            if (_wasKeyPickedThisLevel)
            {
                bool isCollidedLayerInLevelPassedMask = (_levelPassedLayerMask.value == (_levelPassedLayerMask.value | (1 << collision.gameObject.layer)));

                if (isCollidedLayerInLevelPassedMask)
                {
                    
                    LevelManager.Instance.NotifyLevelPassed();
                    WasKey = false;
                }
            }

            bool isCollidedLayerInCollectibleMask = _collectibleMask.value == (_collectibleMask.value | (1 << collision.gameObject.layer));

            if (isCollidedLayerInCollectibleMask)
            {
               
                Destroy(collision.gameObject);
                WasKey = true;
            }
        }
    }
}

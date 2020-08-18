using UnityEngine;

namespace MatrixJam.Team17
{
    public class Explosion : MonoBehaviour
    {
        public ExplosionConfig config;
    
        private void Awake()
        {
            Destroy(gameObject, config.lifetime);
        }
    }
}

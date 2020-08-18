using UnityEngine;

namespace MatrixJam.Team17
{
    [CreateAssetMenu(menuName = "TheFlyingDragons/ProjectileConfig")]
    public class ProjectileConfig : ScriptableObject
    {
        public Projectile projectilePrefab;
        
        [Space]
        
        public float speed = 20f;
        
        [Space]
        
        public bool expires = true;
        
        //[ShowIf("expires")]
        public float expireTime = 5f;
        
        [Space]
        
        public bool explodes = true;
        
        //[ShowIf("explodes")]
        public ExplosionConfig explosionConfig;
    }
}

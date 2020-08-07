﻿using UnityEngine;

namespace TheFlyingDragons
{
    [CreateAssetMenu(menuName = "TheFlyingDragons/NinjaRopeHookConfig")]
    public class NinjaRopeHookConfig : ProjectileConfig
    {
        [Space]
        
        public LayerMask raycastLayer;
        
        [Space]
        
        public Material ropeMaterial;
        public float ropeMinLength = 2f;
        public float ropeMaxLength = 24f;
    }
}

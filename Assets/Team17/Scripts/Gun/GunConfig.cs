using UnityEngine;

namespace MatrixJam.Team17
{
    [CreateAssetMenu(menuName = "TheFlyingDragons/GunConfig")]
    public class GunConfig : ScriptableObject
    {
        public Gun gunPrefab;
        public ProjectileConfig projectileConfig;
        public int animatorWeaponType = 1;
        public int shotsPerClip = 10;
        public float shotsPerSecond = 1;
        public float reloadTime = 1f;
        public FireMode fireMode = FireMode.SemiAuto;
    }
}

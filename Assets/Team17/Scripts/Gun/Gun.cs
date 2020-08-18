using System.Collections;
using UnityEngine;

namespace MatrixJam.Team17
{
    public class Gun : MonoBehaviour, IPlayerInventoryItem, ITrigger
    {
        public GunConfig gunConfig;
        public Transform projectileSpawnPoint;

        bool isTriggerDown;
        float nextShotCountdown;
        int shotsInClip;
        bool isReloading;

        public Player Owner { get; set; }
        public bool IsPassive => false;
        public float ReloadProgress { get; set; }

        public virtual void Awake()
        {
            shotsInClip = gunConfig.shotsPerClip;
        }

        public virtual void TriggerDown()
        {
            if (isReloading || shotsInClip <= 0)
            {
                return;
            }
            
            if (isTriggerDown)
            {
                nextShotCountdown -= Time.fixedDeltaTime;
                
                if (gunConfig.fireMode == FireMode.SemiAuto || nextShotCountdown > 0f)
                {
                    return;
                }
            }
            else
            {
                isTriggerDown = true;
            }
            
            // HACK: Just doing this until we get the character's arms rotating.
            Vector3 fireDir = Vector3.Normalize(Mouse.GetWorldPosition() - projectileSpawnPoint.position);
            Fire(fireDir);

            ResetNextShotCountdown();

            shotsInClip -= 1;
            if (shotsInClip <= 0)
            {
                Reload();
            }
        }

        public virtual void TriggerUp()
        {
            isTriggerDown = false;
            ResetNextShotCountdown();
        }

        public virtual void Fire(Vector3 direction)
        {
            Projectile projectile = SpawnProjectile();
            Rigidbody2D projectileBody = projectile.GetComponent<Rigidbody2D>();
            projectileBody.velocity = direction * gunConfig.projectileConfig.speed;
        }

        public virtual Projectile SpawnProjectile()
        {
            Projectile projectile = Instantiate(
                gunConfig.projectileConfig.projectilePrefab,
                projectileSpawnPoint.position,
                projectileSpawnPoint.rotation
            );
            projectile.Owner = Owner;
            return projectile;
        }

        public void Reload()
        {
            if (isReloading || shotsInClip == gunConfig.shotsPerClip)
            {
                return;
            }

            StartCoroutine(PerformReload());
        }

        IEnumerator PerformReload()
        {
            isReloading = true;
            
            float timeElapsed = 0;
            while (timeElapsed < gunConfig.reloadTime)
            {
                yield return new WaitForEndOfFrame();
                timeElapsed += Time.deltaTime;
                ReloadProgress = Mathf.Clamp01(timeElapsed / gunConfig.reloadTime);
            }
            
            shotsInClip = gunConfig.shotsPerClip;
            isReloading = false;
            ReloadProgress = 0;
        }

        void ResetNextShotCountdown()
        {
            nextShotCountdown = 1f / gunConfig.shotsPerSecond;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team2
{
    public class TurretGunController : MonoBehaviour
    {
        [SerializeField] private float shootingInterval;
        [SerializeField] private Transform firePoint;
        [SerializeField] private GameObject bulletPrefab;

        void Start()
        {
            OnEnable();
        }

        private void SpawnBullet()
        {
            Instantiate(bulletPrefab, firePoint.position, transform.rotation);
        }

        private void OnDisable()
        {
            CancelInvoke();
        }

        private void OnEnable()
        {
            InvokeRepeating("SpawnBullet", 1f, shootingInterval);
        }
    }
}

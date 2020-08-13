using UnityEngine;

namespace MatrixJam.Team1
{
    public class SBPlayerBullet : MonoBehaviour
    {
        [SerializeField] private float bulletSpeed = 10f;
        [SerializeField] private GameObject bulletExplode;

        private void Update()
        {
            transform.position += transform.forward * (Time.deltaTime * bulletSpeed);
        }

        private void OnCollisionEnter(Collision other)
        {
            var temp = other.gameObject.GetComponent<SBEnemyController>();
            if (temp != null)
            {
                temp.Kill();
            }
            else
            {
                Instantiate(bulletExplode, transform.position, transform.rotation);
            }
            
            Destroy(gameObject);
        }
    }
}

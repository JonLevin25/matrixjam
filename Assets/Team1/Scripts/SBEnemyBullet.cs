using UnityEngine;

namespace MatrixJam.Team1
{
    public class SBEnemyBullet : MonoBehaviour
    {
        [SerializeField] private float bulletSpeed = 10f;
        [SerializeField] private GameObject bulletExplode;
        [SerializeField] private GameObject playerExplode;

        public int exit;
        
        private void Update()
        {
            transform.position += transform.forward * (Time.deltaTime * bulletSpeed);
        }

        private void OnCollisionEnter(Collision other)
        {
            var temp = other.gameObject.GetComponent<SBPlayerController>();
            if (temp != null)
            {
                temp.Kill();
                Invoke("DelayExit", 1.5f);
                gameObject.SetActive(false);
                Instantiate(playerExplode, transform.position, transform.rotation);
            }
            else
            {
                Instantiate(bulletExplode, transform.position, transform.rotation);
                Destroy(gameObject);
            }
            
        }

        private void DelayExit()
        {
            var exits = FindObjectsOfType<Exit>();
            foreach (var exitObj in exits)
            {
                if (exitObj.num_portal == exit)
                {
                    exitObj.EndLevel();
                    return;
                }
            }
        }
    }
}

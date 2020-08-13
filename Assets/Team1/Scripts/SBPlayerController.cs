using System;
using UnityEngine;

namespace MatrixJam.Team1
{
    public class SBPlayerController : MonoBehaviour
    {

        [SerializeField] private SBPlayerBullet bulletPrefab;

        [SerializeField] private Transform gunPos;
        [SerializeField] private float moveSpeed;
        [SerializeField] private float shootingRate;
        
        private float timer = 0;
        private Camera cam;

        [SerializeField] private Entrance[] entrances;

        private void Start()
        {
            transform.position = entrances[LevelHolder.Level.Entrnce_Used].transform.position;
            cam = Camera.main;
        }

        private void Update()
        {
            LookAtMouse();
            CheckDirection();
            CheckShooting();
            timer += Time.deltaTime;
        }

        private void CheckDirection()
        {
            Vector3 moveAmount = Input.GetAxis("Vertical") * Vector3.forward + Input.GetAxis("Horizontal") * Vector3.right;
            transform.position += moveAmount * (Time.deltaTime * moveSpeed);
        }

        private void LookAtMouse()
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                transform.LookAt(new Vector3(hit.point.x, transform.position.y, hit.point.z));
            }
        }

        private void CheckShooting()
        {
            if (Input.GetMouseButton(0) && timer >= shootingRate)
            {
                timer = 0;
                Instantiate(bulletPrefab, gunPos.position, gunPos.rotation);
            }
        }
        
        public void Kill()
        {
            Destroy(gameObject);
        }
    }
}

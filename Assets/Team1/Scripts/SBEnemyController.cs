using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace MatrixJam.Team1
{
    public class SBEnemyController : MonoBehaviour
    {
        private enum EnemyState
        {
            idle,
            chase,
            attack
        }
        
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private SBEnemyBullet bulletPrefab;
        [SerializeField] private Transform gunPos;
        [SerializeField] private float shootingRate;
        
        [SerializeField] private float distanceToChase = 15f;
        [SerializeField] private float distanceToShoot = 5f;

        [SerializeField] private GameObject enemyExplode;

        [SerializeField] private bool isChaser = false;
        
        private float timer = 0;
        
        private List<Vector3> idlePositions = new List<Vector3>();
        private int currentIdlePos = 0;
        
        private SBPlayerController targetPlayer;
        [SerializeField] private EnemyState state = EnemyState.idle;

        [SerializeField] private TextMesh textMesh;

        private int exit;
        
        public void Init(string nameTeam, int index)
        {
            textMesh.text = nameTeam;
            exit = index;
        }
        
        private void Start()
        {
            targetPlayer = FindObjectOfType<SBPlayerController>();

            var lastPos = transform.position;
            for (int i = 0; i < Random.Range(3,6); i++)
            {
                lastPos += new Vector3(Random.Range(2f,6f), 0,Random.Range(2f,6f));
                idlePositions.Add(lastPos);
            }

            isChaser = true; //Random.Range(0, 2) == 0;
        }

        private void Update()
        {
            if (targetPlayer == null)
            {
                return;
            }
            
            CheckState();
            DoAccordingToState();
            timer += Time.deltaTime;
        }

        private void CheckState()
        {
            var distance = Vector3.Distance(transform.position, targetPlayer.transform.position);
            
            if (distanceToShoot > distance)
            {
                state = EnemyState.attack;
            }
            else if(isChaser || distanceToChase > distance)
            {
                state = EnemyState.chase;
            }
            else
            {
                state = EnemyState.idle;
            }
        }
        
        private void DoAccordingToState()
        {
            switch (state)
            {
                
                case EnemyState.chase:
                    Chase();
                    break;
                
                case EnemyState.attack:
                    Shoot();
                    break;
                
                default:
                case EnemyState.idle:
                    Idle();
                    break;
            }
        }

        private void Chase()
        {
            agent.SetDestination(targetPlayer.transform.position);
        }
        
        private void Shoot()
        {
            
            if (timer >= shootingRate /*&& 
                Physics.Raycast(gunPos.position, transform.forward, out RaycastHit hit) && 
                hit.transform.CompareTag("Player")*/)
            {
                timer = 0;
                var tempBullet = Instantiate(bulletPrefab, gunPos.position, gunPos.rotation);
                tempBullet.exit = exit;
            }
            else
            {
                Chase();
            }
        }
        
        private void Idle()
        {
            if (agent.remainingDistance  <= 0.1f)
            {
                currentIdlePos++;
                if (currentIdlePos >= idlePositions.Count)
                {
                    currentIdlePos = 0;
                }
            }
            
            agent.SetDestination(idlePositions[currentIdlePos]);
        }

        public void Kill()
        {
            Instantiate(enemyExplode, transform.position, transform.rotation);
            FindObjectOfType<SBScore>().AddScore();
            Destroy(gameObject);
        }
        
    }
}

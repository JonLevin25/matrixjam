using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

namespace MatrixJam.Team14
{
    public class ObstaclePayload
    {
        public readonly Obstacle Obstacle;
        public readonly bool Successful;
        public readonly TrainMove Move;
        public readonly Transform MoveCue;
        public readonly Transform ObstacleHolder;

        public ObstaclePayload(Obstacle obstacle, bool successful, TrainMove move, Transform moveCue, Transform obstacleHolder)
        {
            Obstacle = obstacle;
            Successful = successful;
            Move = move;
            MoveCue = moveCue;
            ObstacleHolder = obstacleHolder;
        }
    }

    public class Obstacle : MonoBehaviour
    {
        [SerializeField] private bool isMoveHold;
        [SerializeField] private TrainMove trainMove;
        [SerializeField] private Transform moveCue; // Where should actually do the move. Null = do when triggers
        [SerializeField] private BoxCollider trigger; // For Gizmos
        [SerializeField] private Transform obstacleHolder;

        public static IReadOnlyList<Obstacle> AllObstaclesSortedByZ;
        public static Dictionary<TrainMove, List<Obstacle>> CurrObstacles;

        private bool _succeeded;

        public TrainMove Move => trainMove;
        public Transform MoveCue => moveCue;
        public Transform ObstacleHolder => obstacleHolder;
        
        public static event Action<ObstaclePayload> OnObstacleEvent;
        
        private void Awake()
        {
            GameManager.ResetEvent += OnGameReset;
            CurrObstacles = Enum
                .GetValues(typeof(TrainMove))
                .Cast<TrainMove>()
                .ToDictionary(
                    move => move, 
                    move => new List<Obstacle>()
                );

            AllObstaclesSortedByZ = FindObjectsOfType<Obstacle>()
                .OrderBy(obstacle => obstacle.transform.position.z).ToArray();
        }

        private void OnDrawGizmos()
        {
            var regPos = trigger.transform.position;
            var trigPos = regPos + trigger.center;
            var size = trigger.size;
            var color = new Color(0.0f, 1.0f, 0.25f, 0.3f);

            Gizmos.color = color;
            Gizmos.DrawCube(trigPos, trigger.size);

            Gizmos.color = new Color(1f, 0f, 0f,0.5f);
            Gizmos.DrawCube(regPos, new Vector3(size.x, size.y, 0.2f));
        }

        private void OnDestroy()
        {
            GameManager.ResetEvent -= OnGameReset;
        }

        public void OnPressedInZone()
        {
            if (_succeeded) return;

            _succeeded = true;
            SendEventUsingFields();
        }

        /// <summary>
        /// Returns true if was in an obstacle zone
        /// </summary>
        public static Obstacle HandleMovePressed(TrainMove move)
        {
            var obstacles = CurrObstacles[move];
            var obs = obstacles.FirstOrDefault();

            Assert.IsTrue(obstacles.Count <= 1, "More than one obstacle should not overlap!");
            if (!obs) return null;
            
            obs.OnPressedInZone();

            return obs;
        }

        private void OnGameReset()
        {
            _succeeded = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            
            CurrObstacles[trainMove].Add(this);
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            
            CurrObstacles[trainMove].Remove(this);

            if (isMoveHold && TrainMoves.GetKeyHold(Move))
            {

                Debug.Log($"KEY HOLD! ({Move})");
                OnPressedInZone();
            }
            else if (!_succeeded) SendEventUsingFields();
        }

        private void SendEventUsingFields()
        {
            Debug.Log($"Sending Obs Event ({Move}, {_succeeded})");
            OnObstacleEvent?.Invoke(new ObstaclePayload(this, _succeeded, Move, moveCue, obstacleHolder));
        }

        public static Obstacle GetNextObstacle(Vector3 pos)
        {
            foreach (var obs in AllObstaclesSortedByZ)
            {
                if (obs.transform.position.z > pos.z) return obs;
            }

            return null;
        }
    }
}

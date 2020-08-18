using UnityEngine;

namespace MatrixJam.Team19.Gameplay.Authoring
{
    public class PrefabSpawner : MonoBehaviour
    {
        [SerializeField]
        private GameObject _spawnedPrefab;

        [SerializeField]
        private float _spawnIntervalSeconds;

        [SerializeField]
        private bool _shouldSpawnOnStart;

        private float _lastSpawnTime;

        private void Start()
        {
            if (_shouldSpawnOnStart)
            {
                ForceSpawn();
            }
            else
            {
                ResetSpawnTime();
            }
        }

        private void Update()
        {   
            if (Time.time - _lastSpawnTime > _spawnIntervalSeconds)
            {
                Instantiate(_spawnedPrefab, transform.position, transform.rotation);

                ResetSpawnTime();
            }
        }

        private void ForceSpawn()
        {
            _lastSpawnTime = Time.time - _spawnIntervalSeconds;
        }

        private void ResetSpawnTime()
        {
            _lastSpawnTime = Time.time;
        }
    }
}

using UnityEngine;

namespace MatrixJam.Team19.Gameplay.Authoring
{
    public class PrefabSpawnerRandomIntervals : MonoBehaviour
    {
        [SerializeField]
        private GameObject _spawnedPrefab;

        [SerializeField]
        private Team19.DataStructures.Range _spawnIntervalRange;

        [SerializeField]
        private bool _shouldSpawnOnStart;

        private float _lastSpawnTime;
        private float _currentSpawnInterval;

        private void Start()
        {
            if (_shouldSpawnOnStart)
            {
                _currentSpawnInterval = 0;
            }
            else
            {
                SetRandomSpawnInterval();
            }

            ResetSpawnTime();

        }

        private void Update()
        {   
            if (Time.time - _lastSpawnTime > _currentSpawnInterval)
            {
                Instantiate(_spawnedPrefab, transform.position, transform.rotation);

                ResetSpawnTime();
                SetRandomSpawnInterval();
            }
        }

        private void ResetSpawnTime()
        {
            _lastSpawnTime = Time.time;
        }

        private void SetRandomSpawnInterval()
        {
            _currentSpawnInterval = Random.Range(_spawnIntervalRange.from, _spawnIntervalRange.to);
        }
    }
}

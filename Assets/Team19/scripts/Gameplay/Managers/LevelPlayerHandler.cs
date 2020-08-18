using MatrixJam.Team19.Gameplay.Managers;
using UnityEngine;

namespace MatrixJam.Team19.Gameplay.Managers
{
    [System.Serializable]
    public class LevelPlayerHandler
    {
        [SerializeField]
        private GameObject _playerPrefab;

        private GameObject _playerInstance;

        public void StartLevel(GameStarter starter, int entrance_number)
        {
            if (_playerInstance != null)
            {
                GameObject.Destroy(_playerInstance);
            }

            _playerInstance = GameObject.Instantiate(_playerPrefab, starter.transform.position, starter.transform.rotation);

            
        }
    }
}

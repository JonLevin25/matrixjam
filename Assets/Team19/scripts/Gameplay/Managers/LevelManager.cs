using System;
using UnityEngine;
using MatrixJam.Team19.Gameplay.General;

namespace MatrixJam.Team19.Gameplay.Managers
{

    public class LevelManager : MonoBehaviour
    {

        public static LevelManager Instance;

        [SerializeField]
        private int _winRequiredPassCount;

        [SerializeField]
        private int _failRequireLossCount;

        [SerializeField]
        private Exit _levelWinExit;

        [SerializeField]
        private Exit _levelFailExit;

        [SerializeField]
        private LevelPlayerHandler _playerHandler;

        [SerializeField]
        private ModifiedContentManager _modifiedContentManager;

        [SerializeField]
        private BeaconManager _beaconManager;

        [SerializeField]
        private UIManager _uiManager;

        private int _levelWinCount = 0;
        private int _levelLossCount = 0;

        private GameStarter _gameStarter;
        private int _startedEntranceNumber;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(this);
            }
        }

        public void NotifyLevelStarted(GameStarter starter, int entrance_number)
        {   
            _gameStarter = starter;
            _startedEntranceNumber = entrance_number;

            _uiManager.Initialize();
            _beaconManager.Initialize();
            _modifiedContentManager.InitializeByEntrance(entrance_number, _winRequiredPassCount);
            _modifiedContentManager.ModifyContentByProgress(_levelWinCount);

            _playerHandler.StartLevel(starter, entrance_number);
            PlayerInteractions.WasKey = false;
            
        }

        public void NotifyKeyPicked()
        {
            
        }

        public void NotifyLevelLost()
        {
            _levelLossCount ++;

            if (_levelLossCount == _failRequireLossCount)
            {
                ActOnLevelFail();
            }
            else
            {
                ActOnLevelLost();
            }
        }

        public void NotifyLevelPassed()
        {
            _levelWinCount++;

            if (_levelWinCount == _winRequiredPassCount)
            {
                ActOnLevelWin();
            }
            else
            {
                ActOnLevelPassed();
            }
        }

        private void ActOnLevelPassed()
        {
            RespawnPlayer();

            _beaconManager.LightBeaconByProgress(_levelWinCount);
            _modifiedContentManager.ModifyContentByProgress(_levelWinCount);
        }

        private void ActOnLevelLost()
        {

            RespawnPlayer();
            _uiManager.UpdateHealth(_levelLossCount);
        }

        private void ActOnLevelFail()
        {
            Debug.Log("[Team19] Level Failed");
            _levelFailExit.EndLevel();

        }

        private void ActOnLevelWin()
        {
            Debug.Log("[Team19] Level Won");
            _levelWinExit.EndLevel();
        }

        private void RespawnPlayer()
        {
            _playerHandler.StartLevel(_gameStarter, _startedEntranceNumber);
            PlayerInteractions.WasKey = PlayerInteractions.WasKey;
        }
    }
}

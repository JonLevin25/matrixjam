using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team19.Gameplay.Managers
{
    [System.Serializable]
    public class BeaconManager
    {
        [SerializeField]
        private GameObject[] _beacons;

        public void Initialize()
        {
            foreach (GameObject beacon in _beacons)
            {
                beacon.SetActive(false);
            }
        }

        public void LightBeaconByProgress(int progress)
        {
            int progressIndex = progress - 1;

            _beacons[progressIndex].SetActive(true);
        }
    }
}

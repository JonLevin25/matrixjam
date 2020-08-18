using UnityEngine;

namespace MatrixJam.Team19.Gameplay.Managers
{
    public enum ETimeline
    {
        BRIGHTEST_TIMELINE,
        DARKEST_TIMELINE,
    }

    [System.Serializable]
    public class ModifiedContentManager
    {
        [SerializeField]
        private Transform _modifiableRoot;

        [SerializeField]
        private GameObject[] _darkestTimelinePrefabs;

        [SerializeField]
        private GameObject[] _brightestTimelinePrefabs;
        
        private GameObject _activeContent;

        private ETimeline _activeTimeline = ETimeline.DARKEST_TIMELINE;

        public void InitializeByEntrance(int entrance_num, int max_progress)
        {

            switch (entrance_num)
            {
                case 0:
                    _activeTimeline = ETimeline.DARKEST_TIMELINE;
                    break;

                case 1:
                    _activeTimeline = ETimeline.BRIGHTEST_TIMELINE;
                    break;
            }
        }

        public void ModifyContentByProgress(int progress)
        {
            GameObject.Destroy(_activeContent);

            GameObject activeContentPrefab = GetNextPrefabForTimeline(progress);
            _activeContent = GameObject.Instantiate(activeContentPrefab, _modifiableRoot);
        }

        private GameObject GetNextPrefabForTimeline(int progress)
        {
            switch (_activeTimeline)
            {
                case ETimeline.DARKEST_TIMELINE:
                    return _darkestTimelinePrefabs[progress];

                case ETimeline.BRIGHTEST_TIMELINE:
                    return _brightestTimelinePrefabs[progress];
                default:
                    throw new System.Exception("[MOD_CONTENT] No Timeline was Selected");
            }
        }
    }
}

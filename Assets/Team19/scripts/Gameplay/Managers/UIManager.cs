using UnityEngine;
using UnityEngine.UI;

namespace MatrixJam.Team19.Gameplay.Managers
{
    [System.Serializable]
    public class UIManager
    {
        [SerializeField]
        private RawImage[] _healthBars;

        private int _previousLosses;

        public void Initialize()
        {
            foreach (RawImage bar in _healthBars)
            {
                bar.CrossFadeAlpha(1.0f, 1f, true);
            }
        }

        public void UpdateHealth(int losses)
        {
            float healthDifference = losses - _previousLosses;

            if (healthDifference < 0)
            {
                for (int healthBarIndex = _previousLosses; healthBarIndex < losses; ++healthBarIndex)
                {
                    _healthBars[healthBarIndex].CrossFadeAlpha(0.9f, 1f, true);
                }
            }

            if (healthDifference > 0)
            {
                for (int healthBarIndex = _previousLosses; healthBarIndex < losses; ++healthBarIndex)
                {
                    if (healthBarIndex < _healthBars.Length && healthBarIndex >= 0)
                    {
                        _healthBars[healthBarIndex].CrossFadeAlpha(0.1f, 1f, true);
                    }
                }
            }

            _previousLosses = losses;
        }
    }
}

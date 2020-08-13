using UnityEngine;

namespace MatrixJam.Team14
{
    public class LivesController : MonoBehaviour
    {
        public GameObject[] lives;

        private void Start()
        {
            SetLives(TrainController.Instance.Lives);
            GameManager.ResetEvent += OnReset;
        }

        private void OnDestroy()
        {
            GameManager.ResetEvent -= OnReset;
        }

        private void OnReset() => SetLives(TrainController.Instance.Lives);

        private void SetLives(int remainingLives)
        {
            for (var i = 0; i < lives.Length; i++)
            {
                var life = lives[i];
                life.SetActive(i < remainingLives);
            }
        }
    }
}

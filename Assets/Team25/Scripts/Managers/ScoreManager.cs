using UnityEngine;

namespace MatrixJam.Team25.Scripts.Managers
{
    public class ScoreManager
    {
        public float CalculateScore(int particles, float stink)
        {
            Debug.Log("Score: " + particles * stink);
            return particles * stink;
        }
    }
}

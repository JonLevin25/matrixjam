using UnityEngine;

namespace MatrixJam.Team25.Scripts.Managers
{
    public class DataManager : MonoBehaviour
    {
        public Exit exit1, exit2, exit3, exit4, exit5;
        [HideInInspector] public float fartForce, stink;
        [HideInInspector] public GameObject gamePrefab;
        public int round;
        [HideInInspector] public int pooped;
        [HideInInspector] public bool tutorial;
        [HideInInspector] public bool intro;
        [HideInInspector] public float totalScore;
        
        public void ResetGame()
        {
            var currentGame = GameObject.FindWithTag("GameController");
            Destroy(currentGame);
            var newGame = Instantiate(gamePrefab);
            newGame.SetActive(true);
        }

        public void ExitGame()
        {
            if (totalScore == 0f)
            {
                if (pooped == 3)
                {
                    exit1.EndLevel();
                }
                else
                {
                    exit2.EndLevel();
                }
            }
            else
            {
                if (totalScore > 0 && totalScore <= 750)
                {
                    exit3.EndLevel();
                }
                else if (totalScore > 750 && totalScore <= 1500)
                {
                    exit4.EndLevel();
                }
                else if (totalScore > 1500)
                {
                    exit5.EndLevel();
                }
            }
        }
    }
}

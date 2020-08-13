using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MatrixJam.Team1
{
    public class SBScore : MonoBehaviour
    {
        [SerializeField] private Text scoreText;

        private int score = 0;

        public void AddScore()
        {
            score += 10;
            scoreText.text = score.ToString();
        }
        
    }
}

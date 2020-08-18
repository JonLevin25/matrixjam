using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MatrixJam.Team20
{
    public class UIManager : MonoBehaviour
    {
        private Player _player;
        [SerializeField] private Text _coinNumberText, _livesText;
        // Start is called before the first frame update
        void Start()
        {
            _player = GameObject.Find("Player").GetComponent<Player>();
            if (_player == null)
            {
                Debug.LogError("player is null");
            }
            //_coinNumberText.text = "Coins " + 0;
        }

        // Update is called once per frame
        void Update()
        {
            _livesText.text = "Lives " + _player.GetLives();
            _coinNumberText.text = "Coins " + _player.GetCoinNumber();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team4
{
    [RequireComponent(typeof(Player))]
    public class NumberPool : MonoBehaviour
    {
        public NumberButtonScript NumberButtonPrefab;

        private NumberButtonScript[] _numbers;
        private PlayerSide _playerSide;


        // Start is called before the first frame update
        void Awake()
        {
            _numbers= new NumberButtonScript[NumberButtonsManager.BOARD_SIZE];
            _numbers[0] = NumberButtonPrefab;

            _playerSide = GetComponent<Player>().playerSide;
            NumberButtonPrefab.SetValue(1, _playerSide);
            var squareWidth = NumberButtonsManager.SquareWidth(NumberButtonPrefab);
            var originalPosition = NumberButtonPrefab.transform.localPosition;
            for (int i = 1; i < NumberButtonsManager.BOARD_SIZE; i++)
            {
                var x = i % 3;
                var y = i / 3;
                var clone = Instantiate(NumberButtonPrefab, NumberButtonPrefab.transform.parent);
                clone.SetValue(i+1, _playerSide);
                clone.transform.localPosition = originalPosition + new Vector3(x * squareWidth, -y * squareWidth);
                _numbers[i] = clone;
            }
            
        }

        public void SetAvailableNumbers(List<int> choices, bool selectable)
        {
            for (int i = 0; i < _numbers.Length; i++)
            {
                if (choices.Count > i)
                {
                    _numbers[i].gameObject.SetActive(true);
                    _numbers[i].SetValue(choices[i], _playerSide);
                    _numbers[i].MakeSelectable(selectable);
                }
                else
                {
                    _numbers[i].gameObject.SetActive(false);
                }
            }
        }
        
        public void MakeNoneSelectable()
        {
            for (int i = 0; i < _numbers.Length; i++)
            {
                
                _numbers[i].MakeSelectable(false);
                
            }
        }
    }
}

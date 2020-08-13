using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MatrixJam.Team4
{
    [RequireComponent(typeof(Button))]
    public class NumberButtonScript : MonoBehaviour
    {
        public Image ImageBox;
        public Sprite[] HumanNumberSprites;
        public Sprite[] AiNumberSprites;
        public Sprite[] NeutralNumberSprites;
        public Sprite Empty;
        private int _currentValue;
        private Button _button;
        public Vector2 Index { get; set; }

        private void Awake()
        {
            _button = GetComponent<Button>();
            
        }

        public void Clear()
        {
            ImageBox.sprite = Empty;
        }

        public bool HasValue()
        {
            return _currentValue != 0;
        }
        
        public void SetValue(int current, PlayerSide playerSide)
        {
            _currentValue = current;
            var numbers = NeutralNumberSprites;
            switch (playerSide)
            {
                case PlayerSide.Human:
                    numbers = HumanNumberSprites;
                    break;
                case PlayerSide.AI:
                    numbers = AiNumberSprites;
                    break;
            }
            ImageBox.sprite = numbers[current-1];
        }
        
        public void Choose()
        {
            if (_currentValue > 0)
            {
                UIManager.NumberChosen(_currentValue);
            }
            else
            {
                UIManager.SquareChosen(Index);
            }
        }

        public void MakeSelectable(bool b)
        {
            _button.interactable = b;
        }
        
   

    }
}

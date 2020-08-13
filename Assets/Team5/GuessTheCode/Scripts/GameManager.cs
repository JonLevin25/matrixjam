using UnityEngine;
using UnityEngine.UI;

namespace MatrixJam.Team5
{
    public class GameManager : MonoBehaviour
    {
        public CodeLetter[] display;
        public AudioSource audio;
        public CodeLetter[] letters;
        public Door[] doors;
        public Sprite defaultIcon;
        public AudioSource effect;
        public AudioClip click;
        public AudioClip win;
        public AudioClip lose;

        private Data _data;

        public void Init(Data data)
        {
            _data = data;
            
            var panel = _data.letters.ToCharArray();
            for (int i = 0; i < letters.Length; i++)
            {
                letters[i].Init(this, panel[i].ToString());
            }
            
            Reset();
        }

        private string _guess; 
        
        public void Clicked(CodeLetter letter)
        {
            var text = letter.label.text;
            _guess += text;
            
            if(letter.display || _guess.Length > 5)
            {
                effect.clip = lose;
                effect.Play();
                Reset();
                return;
            }
            
            letter.button.interactable = false;
            audio.volume = 1;
            
            display[_guess.Length-1].label.text = text;
            effect.clip = click;
            if (_guess.Length < 5)
            {
                for (int i = 0; i < _data.codes.Length; i++)
                {
                    if (_guess == _data.codes[i].Substring(0, _guess.Length))
                    {
                        doors[i].audio.volume = _guess.Length / 5f;
                        audio.volume = (5f - _guess.Length) / 5;
                        display[_guess.Length-1].button.image.sprite = doors[i].icon;
                    }
                    else
                    {
                        doors[i].audio.volume = 0;
                    }
                }
            }
            else if (_guess.Length == 5)
            {
                bool found = false;
                for (int i = 0; i < _data.codes.Length; i++)
                {
                    if (_guess == _data.codes[i])
                    {
                        found = true;
                        audio.volume = 0;
                        doors[i].Open();
                        display[_guess.Length-1].button.image.sprite = doors[i].icon;
                        break;
                    }
                }
                if (found)
                {
                    effect.clip = win;
                }
            }
            effect.Play();
        }

        public void Reset()
        {
            _guess = "";
            foreach (var letter in letters)
            {
                letter.button.interactable = true;
            }
            
            foreach (var door in doors)
            {
                door.Lock();
            }

            foreach (var letter in display)
            {
                letter.Init(this, "");
                letter.button.image.sprite = defaultIcon;
            }

            audio.volume = 1;
        }
    }

    [System.Serializable]
    public struct Data
    {
        public string[] codes;
        public string letters;
    }
}

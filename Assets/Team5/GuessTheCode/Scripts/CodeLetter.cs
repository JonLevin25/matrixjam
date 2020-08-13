using UnityEngine;
using UnityEngine.UI;

namespace MatrixJam.Team5
{
    public class CodeLetter : MonoBehaviour
    {
        public bool display;
        public Button button;

        public Text label;

        private GameManager _manager;

        public void Init(GameManager manager, string letter)
        {
            _manager = manager;
            label.text = letter;
        }

        public void Clicked()
        {
            _manager.Clicked(this);
        }
    }
}

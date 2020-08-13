using UnityEngine;
using UnityEngine.UI;

namespace MatrixJam.Team5
{
    public class Door : MonoBehaviour
    {
        public Button button;
        public AudioSource audio;
        public Sprite icon;

        public void Lock()
        {
            button.interactable = false;
            audio.volume = 0;
        }

        public void Open()
        {
            button.interactable = true;
            audio.volume = 1;
        }
    }
}

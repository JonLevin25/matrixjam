using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team11
{
    public class Bubble : MonoBehaviour
    {
        [SerializeField] TextFader text;
        MusicPlayer musicPlayer;
        float defaultMusicVolume;
        [SerializeField] float adjustedMusicVolume = 0.3f;

        bool wasUsed = false;

        // Start is called before the first frame update
        void Start()
        {
            musicPlayer = FindObjectOfType<MusicPlayer>();
            defaultMusicVolume = musicPlayer.GetComponent<AudioSource>().volume;
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(!wasUsed)
            {
                FindObjectOfType<TutorialManager>().PlayerEnteredBubble();
               // text.FadeOut();
                wasUsed = true;
            }
            if(collision.GetComponent<PlayerController>())
            {
                SFXPlayer.instance.PlayBreathSFX();
                musicPlayer.GetComponent<AudioSource>().volume = adjustedMusicVolume;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if(collision.GetComponent<PlayerController>())
            {
                SFXPlayer.instance.StopBreathing();
                
                musicPlayer.GetComponent<AudioSource>().volume = defaultMusicVolume;

            }
        }
    }
}

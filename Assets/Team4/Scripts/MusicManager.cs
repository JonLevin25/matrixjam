using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team4
{
    [RequireComponent(typeof(AudioSource))]

    public class MusicManager : MonoBehaviour
    {
        public AudioSource audio;
        public AudioClip startClip;
        public AudioClip loopClip;
        public AudioClip mainLoopClip;
        private bool allowUpdate = false;
        private bool fadeout = false;
        void Start()
        {
            audio.loop = false;
            StartCoroutine(playTutorialLoop());
            EventManager.Singleton.IntroDone += OnIntroDone;
        }

        private void OnIntroDone()
        {
            allowUpdate = true;
            fadeout = true;
        }

        private void Update()
        {
            if ( !allowUpdate )
            {
                return;
            }

            if (audio.volume < 0.06 )
            {
                audio.Stop();
                audio.volume = 0;
                audio.clip = mainLoopClip;
                audio.Play();
                fadeout = false;
            }

            if ( fadeout)
            {
                audio.volume -= Time.deltaTime / 2;
                return;
            }

            //amit requested lower volume for the main loop
            if (audio.volume < 0.8f)
            {
                audio.volume = Mathf.Min(audio.volume + Time.deltaTime, 1);
            }
            else
            {
                allowUpdate = false;
            }
            
        }

        IEnumerator playTutorialLoop()
        {
            audio.clip = startClip;
            audio.Play();
            yield return new WaitForSeconds(audio.clip.length);
            audio.clip = loopClip;
            audio.loop = true;
            audio.Play();
        }

        IEnumerator playMainEngineSound()
        {

            audio.clip = startClip;
            audio.Play();
            yield return new WaitForSeconds(audio.clip.length);
            if (audio.clip == mainLoopClip)
            {
                StopCoroutine("playMainEngineSound");
            }
            audio.clip = loopClip;
            audio.loop = true;
            audio.Play();
        }
    }
}


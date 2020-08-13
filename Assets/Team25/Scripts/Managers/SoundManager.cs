using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MatrixJam.Team25.Scripts.Managers
{
    public class SoundManager : MonoBehaviour
    {
        public AudioClip bgm;
        public AudioClip ohShit;
        public AudioClip kaChing;
        public List<AudioClip> farts;
        public AudioSource bgmSource, fartsSource;

        private void Start()
        {
            bgmSource.clip = bgm;
            bgmSource.loop = true;
            bgmSource.Play();
        }

        public void Fart()
        {
            fartsSource.PlayOneShot(farts[Random.Range(0, farts.Count)]);
        }

        public void Shit()
        {
            fartsSource.PlayOneShot(ohShit);
        }

        public void KaChing()
        {
            fartsSource.PlayOneShot(kaChing);
        }
    }
}

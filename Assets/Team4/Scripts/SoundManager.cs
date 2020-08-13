using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team4
{
    public class SoundManager : MonoBehaviour
    {
        public AudioClip Attack;
        public AudioClip PickNumber;
        public AudioClip PickSquare;
        public AudioClip NextTooltip;
        
        public AudioClip Victory;
        public AudioClip Defeat;

        public AudioSource Source;
        public static SoundManager Instance { get; set; }

        private void Start()
        {
            Instance = this;
            EventManager.Singleton.NextMessage += PlayNextTooltip;
        }

        public void PlayAttack()
        {
            Source.clip = Attack;
            Source.Play();

        }
        
        public void PlayPickNumber()
        {
            Source.clip = PickNumber;
            Source.Play();

        }
        
        public void PlayPickSquare()
        {
            Source.clip = PickSquare;
            Source.Play();

        }
        
        public void PlayNextTooltip()
        {
            Source.clip = NextTooltip;
            Source.Play();

        }
        
        public void PlayVictory()
        {
            Source.clip = Victory;
            Source.Play();

        }
        
        public void PlayDefeat()
        {
            Source.clip = Defeat;
            Source.Play();

        }
    }
}

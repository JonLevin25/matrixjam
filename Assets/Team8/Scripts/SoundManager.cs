using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team8
{
    public class SoundManager : MonoBehaviour
    {
        public AudioClip[] squishSounds;
        [SerializeField]
        private AudioSource soundEffectsAudioSource;
        public void PlaySound(AudioClip sound,float volume = 1f)
        {
            soundEffectsAudioSource.volume = volume;
            soundEffectsAudioSource.PlayOneShot(sound);
        }
    }
}

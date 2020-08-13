using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MatrixJam.Team11
{
    public class SFXPlayer : MonoBehaviour
    {
        private static SFXPlayer _instance;
        public static SFXPlayer instance { get { if (_instance == null) Debug.Log("SFXPlayer is NULL"); return _instance; } }
        AudioSource audioSource;
        public AudioClip pickUpSFX, drownSFX, unlockSFX, solvePuzzle1SFX, solvePuzzleSFX2, solvePuzzleSFX3, lightbulbSFX, keyCodeSFX, pressurePlatePressedSFX, pressurePlateReleasedSFX, collisionSFX, iceCrushSFX, hammertimeSFX, doorOpenSFX, doorCloseSFX, breathSFX, buttonSFX, handleSFX;
        public AudioClip[] swimSoundEffects;




        // Start is called before the first frame update

        private void Awake()
        {
            _instance = this;
        }
        void Start() 
        {
            audioSource = GetComponent<AudioSource>();
        }

        // Update is called once per frame
        void Update()
        {
            
        }




        public void PlaySFX(AudioClip sfx, float volume)
        {
            if (sfx != null)
            {
                audioSource.PlayOneShot(sfx, volume);

            }
            else
            {
                Debug.LogWarning(sfx + " is NULL");
            }
        }

        public void PlaySFX(AudioClip sfx)
        {
            if (sfx != null)
            {
                audioSource.PlayOneShot(sfx);

            }
            else
            {
                Debug.LogWarning(sfx + " is NULL");
            }
        }

        public void PlaySwimSFX()
        {
            int randomIndex = Random.Range(0, swimSoundEffects.Count());
            audioSource.PlayOneShot(swimSoundEffects[randomIndex], 0.25f);

        }


        public void PlayBreathSFX()
        {
            audioSource.clip = breathSFX;
            audioSource.Play();
        }

        public void StopBreathing()
        {
            audioSource.Stop();
            audioSource.clip = null;
        }
    }
}

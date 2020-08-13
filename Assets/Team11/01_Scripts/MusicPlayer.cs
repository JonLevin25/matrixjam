using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team11
{
    public class MusicPlayer : MonoBehaviour
    {

        public AudioClip sunsetMusic, stingerMusic, hardGameLoop, ambienceLoop, normalGameLoop;
        AudioSource _audioSource;


        
        // Start is called before the first frame update
        void Start()
        {
        }
        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();

        }

        // Update is called once per frame
        void Update()
        {
            
        }

       public void PlayRedPill()
        {
            StartCoroutine(StartHardGameLoop());
           
        }

        public void PlayBluePill()
        {
            StartCoroutine(StartNormalGameLoop());
        }

        IEnumerator StartNormalGameLoop()
        {
            _audioSource.Stop();
            _audioSource.PlayOneShot(stingerMusic);
            yield return new WaitForSeconds(stingerMusic.length);
            _audioSource.clip = normalGameLoop;
            _audioSource.Play();
            Debug.Log("Playing " + _audioSource.clip);

        }

        IEnumerator StartHardGameLoop()
        {
            _audioSource.Stop();
            _audioSource.PlayOneShot(stingerMusic);
            yield return new WaitForSeconds(stingerMusic.length);
            _audioSource.clip = hardGameLoop;
            _audioSource.Play();
            Debug.Log("Playing " + _audioSource.clip);

        }

        public void PlayStartSequence()
        {
            _audioSource.Stop();
            _audioSource.PlayOneShot(sunsetMusic);
         //   StartCoroutine(StartSequenceCoroutine());
        }

        IEnumerator StartSequenceCoroutine()
        {
            
           
            yield return new WaitForSeconds(sunsetMusic.length);
           // _audioSource.clip = ambienceLoop;
//         _audioSource.Play();

        }


        
                

            
           
            
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team22
{
    public class BambooSpawner : MonoBehaviour
    {
        public GameObject bamboo, fastBamboo;
        public Transform spawnLocation;
        public AudioSource source;
        public bool playCuesInEditMode = true;

        private void Start()
        {
            GetComponent<Animator>().enabled = true;
        }

        public void SpawnBamboo()
        {
            if (Application.isPlaying)
                Instantiate(bamboo, spawnLocation.position, bamboo.transform.rotation);
        }

        public void SpawnFastBamboo()
        {
            if (Application.isPlaying)
                Instantiate(fastBamboo, spawnLocation.position, fastBamboo.transform.rotation);
        }

        public void PlayCue(AudioClip cueClip)
        {
            if((playCuesInEditMode && !Application.isPlaying) || Application.isPlaying)
                source.PlayOneShot(cueClip);
        }
    }
}

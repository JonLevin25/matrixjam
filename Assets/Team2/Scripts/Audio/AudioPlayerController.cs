using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team2
{
    public class AudioPlayerController : MonoBehaviour
    {
        [SerializeField] private AudioClip[] footsteps;
        [SerializeField] private AudioClip[] jumps;
        [SerializeField] private AudioClip[] gun;
        [SerializeField] private AudioClip[] gunInvalid;
        [SerializeField] private FloopGunController gunController;

        private AudioSource source;
        private Animator playerAnimator;

        private Random random = new Random();

        private void Start()
        {
            playerAnimator = GetComponentInChildren<Animator>();
            source = GetComponent<AudioSource>();
            source.playOnAwake = false;

            if (footsteps[0] != null)
            {
                source.clip = footsteps[0];
            }
        }

        private void Update()
        {
            if (playerAnimator.GetBool("walk"))
            {
                PlayRandomClip(footsteps);
            }

            if (playerAnimator.GetBool("jump"))
            {
                PlayRandomClip(jumps, true);
                playerAnimator.SetBool("jump", false);
            }

            if (gunController.shootState == FloopGunController.ShootState.valid)
            {
                PlayRandomClip(gun, true);
            }

            if (gunController.shootState == FloopGunController.ShootState.invalid)
            {
                PlayRandomClip(gunInvalid, true);
            }
        }


        private void PlayRandomClip(AudioClip[] collection, bool immediate = false)
        {
            if (immediate || !source.isPlaying)
            {
                source.clip = collection[Random.Range(0, collection.Length)];
                source.PlayOneShot(source.clip);
            }
        }
    }

}

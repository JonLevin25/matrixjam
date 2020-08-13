using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team22
{
    public class PlayerController : MonoBehaviour
    {
        public TriggerController triggerController;
        public Animator playerAnimator;
        public float missDelay = 1f;
        public float swingDelay = 0.1f;
        [Header("Audio")]
        public AudioClip[] swordSwings, swordHits;

        private bool canShoot = true;
        private float missTimer = 0;
        private AudioSource source;

        private void Start()
        {
            source = GetComponent<AudioSource>();
        }

        // Update is called once per frame
        void Update()
        {
            if (missTimer > 0)
                missTimer -= Time.deltaTime;
            else
                canShoot = true;

            if(Input.anyKeyDown && !Input.GetKeyDown(KeyCode.Escape))
            {
                if(canShoot && !DialogueManager.instance.IsInDialogue())
                {
                    CheckTrigger();
                }
            }
        }

        private void CheckTrigger()
        {
            bool inTrigger = triggerController.GetTriggerStatus();
            playerAnimator.SetTrigger("Slice");
            source.PlayOneShot(GetRandomClip(swordSwings));

            if (inTrigger)
            {
                // bamboo is inside trigger
                // Debug.LogWarning("HIT!");
                triggerController.DestroyInside();
                source.PlayOneShot(GetRandomClip(swordHits));
                ShakeCam.instance.Shake(0.15f, 0.1f);
                missTimer = swingDelay;
            }
            else
            {
                // missed bamboo
                // Debug.LogWarning("MISS!");
                canShoot = false;
                missTimer = missDelay;
            }
        }

        private AudioClip GetRandomClip(AudioClip[] clips)
        {
            return clips[UnityEngine.Random.Range(0,clips.Length)];
        }
    }
}

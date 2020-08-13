using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team9
{
    public class AudioControl : MonoBehaviour
    {
        private AudioSource MainGameMusic;

        private GameManager GameManageRef;

        private bool _pitchDone;

        // Start is called before the first frame update
        void Start()
        {
            _pitchDone = false;
            GameManageRef = GetComponent<GameManager>();
            MainGameMusic = this.gameObject.GetComponent<AudioSource>();
            MainGameMusic.volume = 0.08f;
            PlayGameMusic();
        }

        public void pitchUp()
        {
            MainGameMusic.pitch += 0.4f;
        }

        /*IEnumerator playwithPitch()
        {
            yield return StartCoroutine(pitchPlayer());
        }

        IEnumerator pitchPlayer()
        {
            while (GameManageRef.ParachuteRef._health >= 75)
            {
                yield return null;
            }

            if (GameManageRef.ParachuteRef._health >= 50 && GameManageRef.ParachuteRef._health < 75)
            {
                MainGameMusic.pitch += 0.5f;
                yield return null;
            }

            else if (GameManageRef.ParachuteRef._health >= 25 && GameManageRef.ParachuteRef._health < 50)
            {
                MainGameMusic.pitch += 0.5f;
                yield return null;
            }

            else if (GameManageRef.ParachuteRef._health >= 0 && GameManageRef.ParachuteRef._health < 25)
            {
                MainGameMusic.pitch += 0.5f;
            }


        }
*/

        private void PlayGameMusic()
        {
            MainGameMusic.Play();
        }

        // Update is called once per frame
        void Update()
        {
        }
    }
}

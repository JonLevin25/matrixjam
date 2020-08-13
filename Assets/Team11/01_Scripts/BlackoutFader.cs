using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MatrixJam.Team11
{
    public class BlackoutFader : MonoBehaviour
    {
        Image image;

        // Start is called before the first frame update
        void OnEnable()
        {
            image = GetComponent<Image>();
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        



        public IEnumerator FadeOut(float fadeOutTime)
        {
            Debug.Log("Should fade out");
            while (image.color.a > 0)
            {
                image.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a - ( fadeOutTime * Time.deltaTime));
                yield return new WaitForSeconds(Time.deltaTime);
            }

            image.color = new Color(0, 0, 0, 0);

        }

        public IEnumerator FadeIn(float fadeInTime, float delayDuringBlack, bool callFadeOut)
        {
            while (image.color.a < 1)
            {
                image.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a + (fadeInTime * Time.deltaTime));
                yield return new WaitForSeconds(Time.deltaTime);
            }
            
            image.color = new Color(0, 0, 0, 1);
            if(callFadeOut)
            {
                yield return new WaitForSeconds(delayDuringBlack);
                StartCoroutine(FadeOut(1f));
            }
          
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MatrixJam.Team11
{
    public class BlackoutFader : MonoBehaviour
    {
        Image image;
        [HideInInspector] public Color FadeColor;

        // Start is called before the first frame update
        void OnEnable()
        {
            image = GetComponent<Image>();
            FadeColor = image.color;
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        public IEnumerator LerpOverTime(float duration, Color src, Color dst)
        {
            float progress = 0f;

            do
            {
                float percentage = progress / duration;
                image.color = Color.Lerp(src, dst, percentage);

                progress += Time.deltaTime;
                yield return null;
            } while (progress < duration);

            image.color = dst;
        }

        public IEnumerator FadeOut(float fadeOutTime)
        {
            Debug.Log("Should fade out");
            /*while (image.color.a > 0)
            {
                image.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a - ( fadeOutTime * Time.deltaTime));
                yield return new WaitForSeconds(Time.deltaTime);
            }

            image.color = new Color(0, 0, 0, 0);
            */
            Color transparent = this.FadeColor;
            transparent.a = 0f;

            yield return StartCoroutine(this.LerpOverTime(fadeOutTime, this.FadeColor, transparent));
        }

        public IEnumerator FadeIn(float fadeInTime, float delayDuringBlack = 0f, bool callFadeOut = false)
        {
            /*while (image.color.a < 1)
            {
                image.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a + (fadeInTime * Time.deltaTime));
                yield return new WaitForSeconds(Time.deltaTime);
            }
            
            image.color = new Color(0, 0, 0, 1);
            */
            Color transparent = this.FadeColor;
            transparent.a = 0f;

            yield return StartCoroutine(this.LerpOverTime(fadeInTime, transparent, this.FadeColor));
            if (callFadeOut)
            {
                yield return new WaitForSeconds(delayDuringBlack);
                StartCoroutine(FadeOut(1f));
            }
          
        }

    }
}

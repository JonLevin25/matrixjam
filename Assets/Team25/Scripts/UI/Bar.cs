using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class Bar : MonoBehaviour
    {
        public Image barImage, textImage;
        public Color barColor;
        public float barWidth, barHeight;
        public float fillRate;
        private float fillAmount;
        private Tween barTween;
        void Start()
        {
            ResetBar();
            textImage.gameObject.SetActive(false);
        }

        public void ResetBar()
        {
            barImage.color = barColor;
            barImage.material.color = Color.white;
            RectTransform rt = GetComponent<RectTransform>();
            rt.rect.Set(rt.rect.x, rt.rect.y, barWidth, barHeight);
            barImage.fillAmount = fillAmount = 0;
        }

        public void OscillateBarFill()
        {
            textImage.gameObject.SetActive(true);
            barTween = barImage.DOFillAmount(1, fillRate).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InCirc);
        }
        
        public void KillBar()
        {
            barTween.Kill();
        }
    }
}

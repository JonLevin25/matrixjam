using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MatrixJam.Team11
{
    public class AirMeter : MonoBehaviour
    {
        [SerializeField] RectTransform[] affectedImages;
        [SerializeField] Image fill;
        [SerializeField] float scaleFactor = 1.2f;
        Slider _slider;
        [SerializeField] float timeForHitFX;
        Color defaultColor;

        // Start is called before the first frame update
        void Start()
        {
            _slider = GetComponent<Slider>();
            defaultColor = fill.color;
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        public void PlayerHit()
        {
            if(this.isActiveAndEnabled)
            {
                StartCoroutine(PlayerHitCoroutine());

            }
        }

        IEnumerator PlayerHitCoroutine()
        {
            SFXPlayer.instance.PlaySFX(SFXPlayer.instance.collisionSFX);
            foreach (var image in affectedImages)
            {
                image.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
            }
            fill.color = Color.blue;
            yield return new WaitForSeconds(timeForHitFX);
            fill.color = defaultColor;
            foreach (var image in affectedImages)
            {
                image.localScale = new Vector3(1, 1, 1);
            }
        }
    }
}

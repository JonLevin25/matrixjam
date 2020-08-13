using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team11
{
    public class TextFader : MonoBehaviour
    {
        SpriteRenderer _spriteRenderer;
        [SerializeField] float fadePerTime = 1f;
      public  bool fading;

       


        // Start is called before the first frame update
        void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        // Update is called once per frame
        void Update()
        {
            if (fading)
            {
                _spriteRenderer.color = new Color(_spriteRenderer.color.r, _spriteRenderer.color.g, _spriteRenderer.color.b, _spriteRenderer.color.a - fadePerTime * Time.deltaTime);
            }
        }

        public void FadeOut()
        {
            fading = true;
           // Destroy(gameObject, 1f);
        }
    }
}

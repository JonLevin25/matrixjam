using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MatrixJam.Team11
{
    public class TextFadeIn : MonoBehaviour
    {
        public bool fadingIn = false;
        Text _text;
        [SerializeField] float fadeInTime = 1f;


        // Start is called before the first frame update
        void Start()
        {
            _text = GetComponent<Text>();
        }

        // Update is called once per frame
        void Update()
        {
            if(fadingIn && _text.color.a < 1)
            {
                _text.color = new Color(_text.color.r, _text.color.g, _text.color.b, _text.color.a + fadeInTime * Time.deltaTime);
                if(_text.color.a >= 1)
                {
                    _text.color = new Color(_text.color.r, _text.color.g, _text.color.b, 1);
                }
            }
        }
    }
}

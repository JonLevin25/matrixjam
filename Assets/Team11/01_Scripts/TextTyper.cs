using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

namespace MatrixJam.Team11
{
    public class TextTyper : MonoBehaviour
    {
        [SerializeField] float timeBetweenLetters;
        [TextArea(5, 5)] [SerializeField] string kingSpeech;
        [SerializeField] float periodBreak = 0.1f;
        [SerializeField] float colonBreak = 0.05f;
         TextFadeIn[] textWhenSpeechIsFinished;
        
        Text _text;
        bool _fastForward = false;


        // Start is called before the first frame update
        void Start()
        {
            _text = GetComponent<Text>();
            _text.text = "";
            textWhenSpeechIsFinished = FindObjectsOfType<TextFadeIn>();
           
            

            StartCoroutine(TypeText());
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.anyKeyDown && Input.GetKeyDown(KeyCode.Space))
            {
                this._fastForward = true;
            }
        }

        IEnumerator TypeText()
        {
            foreach (char c in kingSpeech)
            {
                if (this._fastForward)
                {
                    _text.text = kingSpeech;
                    break;
                }

                _text.text += c;
                if(c == ',')
                {
                    yield return new WaitForSeconds(colonBreak);

                }
                else if (c == '.' || c == '?')
                {
                    yield return new WaitForSeconds(periodBreak);

                }
                yield return new WaitForSeconds(timeBetweenLetters);
            }
            FindObjectOfType<SunsetPlayerController>().canMove = true;
            foreach (var text in textWhenSpeechIsFinished)
            {
                text.fadingIn = true;
            }

        }
        
        
    }
}

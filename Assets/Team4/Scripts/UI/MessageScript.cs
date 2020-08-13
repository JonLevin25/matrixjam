using System;
using UnityEngine;
using UnityEngine.UI;

namespace MatrixJam.Team4
{
    public class MessageScript : MonoBehaviour
    {
        public GameObject HighligtedObject;
        public Text Textbox;
        public String[] LinesOfTexts;
        private Transform _originalParent;
        private int _msgIndex = 0;
        private bool _originalActive;


        private void OnValidate()
        {
            if (LinesOfTexts != null && LinesOfTexts.Length > 0)
            {
                Textbox.text = LinesOfTexts[0];
            }
        }

        public void Awake()
        {
            gameObject.SetActive(false);
        }
        
        public void NextMessage()
        { 
            EventManager.Singleton.OnNextMessage();;
        }

        public void ShowMessage()
        {
            if (HighligtedObject != null)
            {
                UIManager.ShowDarkScreen();
                _originalParent = HighligtedObject.transform.parent;
                _originalActive = HighligtedObject.activeInHierarchy;
                HighligtedObject.transform.SetParent(transform.parent);
                HighligtedObject.SetActive(true);
                transform.SetParent(HighligtedObject.transform);
                

            }
            
            gameObject.SetActive(true);
            

        }
        
        public void HideMessage() {
            if (HighligtedObject != null)
            {
                transform.SetParent(HighligtedObject.transform.parent);
                HighligtedObject.transform.SetParent(_originalParent);
                HighligtedObject.SetActive(_originalActive);
                UIManager.HideDarkScreen();
            }
            gameObject.SetActive(false);
    
        }

        public bool HasNext()
        {
            return _msgIndex < LinesOfTexts.Length - 1;
        }

        public void ShowNextLine()
        {
            _msgIndex++;
            Textbox.text = LinesOfTexts[_msgIndex];
        }
    }
    
    
}

using System.Collections.Generic;

namespace MatrixJam.Team4
{
    public class IntroManager
    {

        private List<MessageScript> _messages;
        private int _msgIndex;
        private static IntroManager _instance;

        public IntroManager(List<MessageScript> introMessages)
        {
            _messages = introMessages;
            _instance = this;
        }

        public void Start()
        {
            if ( _messages.Count == 0)
            {
                EventManager.Singleton.OnIntroDone();
                return;
            }

            _messages[_msgIndex].ShowMessage();
        }

        public void NextMessage()
        {
            if (_messages[_msgIndex].HasNext())
            {
                _messages[_msgIndex].ShowNextLine();
            }
            else
            {
                MoveToTheNextTooltip();
            }                 
        }

        private void MoveToTheNextTooltip()
        {
            _messages[_msgIndex].HideMessage();
            if (_msgIndex < _messages.Count - 1)
            {
                _msgIndex++;
                _messages[_msgIndex].ShowMessage();
            }
            else
            {
                EventManager.Singleton.OnIntroDone();
            }
        }

        public void PreviousMessage()
        {
            if(0 < _msgIndex)
            {
                --_msgIndex;
                _messages[_msgIndex].ShowMessage();
            }
        }

    }
}

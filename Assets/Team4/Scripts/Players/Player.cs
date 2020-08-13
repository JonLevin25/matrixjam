using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MatrixJam.Team4
{
    public abstract class Player : MonoBehaviour
    {
        public PlayerSide playerSide;
        public Text ScoreTextbox;
        public int Score { get => _score; set => _score = value; }
        private int _score;
        private List<Unit> _myUnits;

        public List<Unit> MyUnits { get => _myUnits; set => _myUnits = value; }

        public virtual void YourTurn(TurnData turnData)
        {
            UIManager.ChoiceManager.StartTurn(this, turnData);
        }

        public virtual void EndTurn(TurnObject turnObject)
        {
            ScoreTextbox.text = Score.ToString();//TODO add to UIManager
            EventManager.Singleton.OnPlayerPlayed(turnObject);
        }

        
        protected TurnObject ValidateTurnObject(TurnObject validationObject)
        {

            if (validationObject.ChosenUnit == null)
            {
                return null;
            }
            
            if (validationObject.ChosenUnit.Position == null)
            {
                return null;
            }

            return validationObject;
        }
        
    }
    
    public enum PlayerSide
    {
        Human,
        AI,
        Neutral
    }
}

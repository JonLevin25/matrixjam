using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team4
{
    public class AiPlayer : Player
    {
        
        
        // this is an AI which chooses a random unit and returns a random answer
        public override void YourTurn(TurnData turnData)
        {
            base.YourTurn(turnData);
            var turnObject = GameManager.Brain.MakeDecision(turnData, this);
            StartCoroutine(UIManager.ChoiceManager.HandleAiChoice(ValidateTurnObject(turnObject)));

        }
    }
}

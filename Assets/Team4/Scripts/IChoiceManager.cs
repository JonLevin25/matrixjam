using System.Collections;
using System.Linq;
using MatrixJam.Team4;
using UnityEngine;

namespace MatrixJam.Team
{
    public interface IChoiceManager
    {

        void StartTurn(Player player, TurnData turnData);
        void PickNumber(int value);
        void PickSquare(Vector2 index);
        void PickAttack(AttackDirection attackType);
        IEnumerator HandleAiChoice(TurnObject validateTurnObject);
    }

}
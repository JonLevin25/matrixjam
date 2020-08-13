using UnityEngine;

namespace MatrixJam.Team4
{
    public class RandomAi : AiBrain
    {
        public override TurnObject MakeDecision(TurnData turnData, Player player)
        {
            var myUnits = player.MyUnits;
            var randomIndex = Random.Range(0, myUnits.Count);
            var randomUnit = myUnits[randomIndex];
            var unitOptions = turnData._positionOptions[randomUnit];
            var positionIndex = Random.Range(0, unitOptions.Count);
            var position = unitOptions[positionIndex];

            randomUnit.Position = position;
            var turnObject = new TurnObject();
            turnObject.ChosenUnit = randomUnit;

            randomIndex = Random.Range(0, 2);
            switch (randomIndex)
            {
                case 0:
                    turnObject.AttackDirection = AttackDirection.row;
                    break;
                case 1:
                    turnObject.AttackDirection = AttackDirection.square;
                    break;
                case 2:
                    turnObject.AttackDirection = AttackDirection.colum;
                    break;
            }

            return turnObject;
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team4
{
    public class PickAtYouAI : AiBrain
    {
        // this is an AI which chooses a random unit and returns a random answer

        public override TurnObject MakeDecision(TurnData turnData, Player player)
        {
            
            var randomIndex = UnityEngine.Random.Range(0, player.MyUnits.Count);
            var randomUnit = player.MyUnits[randomIndex];
            var unitOptions = turnData._positionOptions[randomUnit];

            while (unitOptions.Count == 0)
            {
                randomIndex = UnityEngine.Random.Range(0, player.MyUnits.Count);
                randomUnit = player.MyUnits[randomIndex];
                unitOptions = turnData._positionOptions[randomUnit];
            }

            var turnObject = new TurnObject();
            turnObject.ChosenUnit = randomUnit;

            SetHighestPointAttack(turnObject, unitOptions, turnData);
            return turnObject;
        }

        private void SetHighestPointAttack(TurnObject turnObject, List<Position> unitOptions, TurnData turnData)
        {
            var unit = turnObject.ChosenUnit;
            var maxPoints = 0;
            var index = 0;
            var bestDirection = AttackDirection.colum;
            for ( int i=0; i < unitOptions.Count; i++)
            {
                var rowPoints = GetRowPoints(unitOptions[i], unit, turnData.boardData);
                var columnPoints = GetColumnPoints(unitOptions[i], unit, turnData.boardData);
                var squarePoints = GetSquarePoints(unitOptions[i], unit, turnData.boardData);

                if (rowPoints > maxPoints)
                {
                    index = i;
                    maxPoints = rowPoints;
                    bestDirection = AttackDirection.row;
                }

                if (columnPoints > maxPoints)
                {
                    index = i;
                    maxPoints = columnPoints;
                    bestDirection = AttackDirection.colum;
                }

                if (columnPoints > maxPoints)
                {
                    index = i;
                    maxPoints = columnPoints;
                    bestDirection = AttackDirection.square;
                }
            }

            unit.Position = unitOptions[index];
            turnObject.AttackDirection = bestDirection;
        }

        private int GetSquarePoints(Position unitOption,  Unit unit, BoardData boardData)
        {
            var ans = 0;
            var square = new Square(unitOption.GetX(), unitOption.GetY());

            for (int i = square.startX; i < square.startX + 3; i++)
            {
                for (int j = square.startY; j < square.startY + 3; j++)
                {
                    var unitToCheck = boardData._boardData[i, j];

                    if (unitToCheck == null ) //|| player == unitToCheck.Owner)
                    {
                        continue;
                    }

                    var gainedPoints = Mathf.Min(unitToCheck.Value, unit.Value);

                    ans += gainedPoints;
                }
            }

            return ans;
        }

        private int GetColumnPoints(Position unitOption, Unit unit, BoardData boardData)
        {
            int ans = 0;
            for (int i = 0; i < 9; i++)
            {
                var x = unitOption.GetX();
                var unitToCheck = boardData._boardData[x, i];

                if (unitToCheck == null )//|| player == unitToCheck.Owner)
                {
                    continue;
                }

                var gainedPoints = Mathf.Min(unitToCheck.Value, unit.Value);

                ans += gainedPoints;
            }

            return ans;
        }

        private int GetRowPoints(Position unitOption, Unit unit, BoardData boardData)
        {
            int ans = 0;
            for (int i = 0; i < 9; i++)
            {
                var y = unitOption.GetY();
                var unitToCheck = boardData._boardData[i, y];

                if (unitToCheck == null)//|| player == unitToCheck.Owner)
                {
                    continue;
                }

                var gainedPoints = Mathf.Min(unitToCheck.Value, unit.Value);

                ans += gainedPoints;
            }

            return ans;
        }

    }
}

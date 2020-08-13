using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MatrixJam.Team;
using UnityEngine;

namespace MatrixJam.Team4
{
    public class ChoiceManager : IChoiceManager
    {
        private BoardManager _boardManager;
        private TurnData _playerTurnData;
        private Unit _selectedUnit;
        private Position _selectedPosition;
        private List<int> _numberChoices;

        public ChoiceManager(BoardManager boardManager)
        {
            _boardManager = boardManager;
        }

        public void StartTurn(Player player, TurnData turnData)
        {
            ClearData();
            _playerTurnData = turnData;
            _numberChoices = new List<int>();
            foreach (var unit in _playerTurnData._positionOptions.Keys)
            {
                if (_playerTurnData._positionOptions[unit].Count > 0)
                {
                    _numberChoices.Add(unit.Value);
                }
            }
            UIManager.SetPlayerAvailableNumbers(_numberChoices, player.playerSide, true);

        }

        private void ClearData()
        {
            UIManager.ShowSelectablePositions(new Vector2[0]); //clear
            _selectedUnit = null;
            _selectedPosition = null;
        }

        public void PickNumber(int value)
        {
            Debug.Log("Player chose: " + value);
            SoundManager.Instance.PlayPickNumber();
            foreach (var unit in _playerTurnData._positionOptions.Keys)
            {
                if (unit.Value == value)
                {
                    UpdateSelectedUnit(unit);
                    return;
                }
            }
            throw new Exception("Could not find number");
        }

        private void UpdateSelectedUnit(Unit unit)
        {
            _selectedUnit = unit;
            if (unit.Owner.playerSide == PlayerSide.Human)
            {
                var positionOption = _playerTurnData._positionOptions[unit];
                Vector2[] squares = GetSquarsFromAllowedPositions(positionOption);
                UIManager.ShowSelectablePositions(squares);
            }
        }

        private Vector2[] GetSquarsFromAllowedPositions(List<Position> positionOption)
        {
            Vector2[] squares = positionOption.Select(x => new Vector2(x.GetX(), x.GetY())).ToArray();
            return squares;
        }

        public void PickSquare(Vector2 index)
        {
            var positionOption = _playerTurnData._positionOptions[_selectedUnit];
            var position = positionOption.Find(p => p.GetX() == index.x && p.GetY() == index.y);
            if (position != null)
            {
                _selectedPosition = position;
                SoundManager.Instance.PlayPickSquare();
                UIManager.SetNumberOnSquare(index, _selectedUnit.Value, _selectedUnit.Owner.playerSide);
                UIManager.ShowDamageOptions(_selectedUnit.Owner.playerSide == PlayerSide.Human);
            }

        }

        public void PickAttack(AttackDirection attackType)
        {
            if (_selectedUnit == null || _selectedPosition == null)
            {
                return;
            }
            var turnObject = new TurnObject();
            turnObject.ChosenUnit = _selectedUnit;
            turnObject.ChosenUnit.Position = _selectedPosition;
            turnObject.AttackDirection = attackType;
            HideUsedNumber();
            UIManager.ShowDamageOptions(false);
            SoundManager.Instance.PlayAttack();
            _boardManager.ExecuteTurn(turnObject);
        }

        private void HideUsedNumber()
        {
            _numberChoices.Remove(_selectedUnit.Value);
            UIManager.SetPlayerAvailableNumbers(_numberChoices, _selectedUnit.Owner.playerSide, false);
        }

        public IEnumerator HandleAiChoice(TurnObject turnObject)
        {
            yield return new WaitForSeconds(1);
            PickNumber(turnObject.ChosenUnit.Value);
            var position = turnObject.ChosenUnit.Position;
            UIManager.ChoiceManager.PickSquare(new Vector2(position.GetX(), position.GetY()));
            yield return new WaitForSeconds(1);
            UIManager.ChoiceManager.PickAttack(turnObject.AttackDirection);


        }
        
    }
}
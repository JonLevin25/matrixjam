using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team4
{
    public class BoardManager
    {
        private int _size;
        private BoardData _boardData;
        private bool[,,] _possibleUnitsPerCoordinate;

        public BoardManager(int size)
        {
            _size = size;

            InitPossibleUnitsPerCoord(size);
            InitBoardData(size);
            

        }
        
        public void ExecuteTurn(TurnObject turnObject)
        {
            var unit = turnObject.ChosenUnit;
            var player = unit.Owner;
            var attackDirection = turnObject.AttackDirection;
            int points = 0;
            switch (attackDirection)
            {
                case AttackDirection.colum:
                    points = GetColumPoints(player, unit);
                    break;
                case AttackDirection.row:
                    points = GetRowPoints(player, unit);
                    break;
                case AttackDirection.square:
                    points = GetSquarePoints(player, unit);
                    break;
            }

            player.Score += points;
            player.EndTurn(turnObject);
            if ( PlaceUnit(unit, unit.Position.GetX(), unit.Position.GetY()))
            {
                player.MyUnits.Remove(unit);
;            }

            EventManager.Singleton.OnTurnOver();
        }

        private int GetSquarePoints(Player player, Unit unit)
        {
            var ans = 0;
            var square = new Square(unit.Position.GetX(), unit.Position.GetY());

            for (int i = square.startX; i < square.startX + 3; i++)
            {
                for (int j = square.startY; j < square.startY + 3; j++)
                {
                    var unitToCheck = _boardData._boardData[i, j];

                    if (unitToCheck == null || player == unitToCheck.Owner)
                    {
                        continue;
                    }

                    var gainedPoints = Mathf.Min(unitToCheck.Value, unit.Value);

                    ans += gainedPoints;
                }
            }

            return ans;
        }

        private int GetRowPoints(Player player, Unit unit)
        {
            int ans = 0;
            for (int i = 0; i < _size; i++)
            {
                var y = unit.Position.GetY();
                var unitToCheck = _boardData._boardData[i, y];

                if (unitToCheck == null || player == unitToCheck.Owner)
                {
                    continue;
                }

                var gainedPoints = Mathf.Min(unitToCheck.Value, unit.Value);

                ans += gainedPoints;
            }

            return ans;
        }

        private int GetColumPoints(Player player, Unit unit)
        {
            int ans = 0;
            for (int i = 0; i < _size; i++)
            {
                var x = unit.Position.GetX();
                var unitToCheck = _boardData._boardData[x, i];

                if (unitToCheck == null || player == unitToCheck.Owner)
                {
                    continue;
                }

                var gainedPoints = Mathf.Min(unitToCheck.Value, unit.Value);

                ans += gainedPoints;
            }

            return ans;
        }

        public TurnData GetPlayerTurnData(Player _player)
        {
            var boardData = new BoardData(_boardData._boardData);
            var possiblePositionByUnit = GetPlayersUnitsOptions(_player);

            return new TurnData(boardData, possiblePositionByUnit);
        }

        private List<Position> GetUnitsLegalPositions(Unit unit)
        {
            var unitValueOptions = GetUnitOptions(unit);
            var unitOptions = new List<Position>();
            for (int i = 0; i < unitValueOptions.Count; i++)
            {
                var pos = new Position(unitValueOptions[i] / 10, unitValueOptions[i] % 10);
                unitOptions.Add(pos);
            }

            return unitOptions;
        }

        private Dictionary<Unit, List<Position>> GetPlayersUnitsOptions(Player player)
        {
            var options = new Dictionary<Unit, List<Position>>();

            for (int i = 0; i < player.MyUnits.Count; i++)
            {
                var unit = player.MyUnits[i];
                var unitOptions = GetUnitsLegalPositions(unit);

                options.Add(unit, unitOptions);
            }

            return options;
        }

        public void AddRandomUnits(int initialRandomUnits = 30)
        {
            for ( ; initialRandomUnits > 0; initialRandomUnits-- )
            {
                AddRandomUnit();
            }
        }

        private void AddRandomUnit()
        {
            Debug.Log("BoardManager:AddRandomUnit");
            do
            {
                int value = UnityEngine.Random.Range(0, 8);
                var possibeOptions = GetUnitOptions(value);
                if (possibeOptions.Count > 0)
                {
                    var randomIndex = UnityEngine.Random.Range(0, possibeOptions.Count);
                    var indexedCoordinate = possibeOptions[randomIndex];
                    int x = indexedCoordinate / 10;
                    int y = indexedCoordinate % 10;
                    int realValue = value + 1;

                    Unit unit = new Unit(null, realValue);
                    Position position = new Position(x, y);
                    if (PlaceUnit(unit, position))
                    {
                        UIManager.SetNumberOnSquare(new Vector2(x, y), unit.Value, PlayerSide.Neutral);
                        return;
                    }
                }
            }
            while (true);      
        }
        

        private List<int> GetUnitOptions(Unit unit)
        {
            return GetUnitOptions(unit.Value - 1);
        }

        private List<int> GetUnitOptions(int value)
        {
            List<int> ans = new List<int>();

            for (int i = 0; i < _size; i++)
            {
                for (int j = 0; j < _size; j++)
                {
                    if ( _possibleUnitsPerCoordinate[i,j,value] )
                    {
                        ans.Add(i * 10 + j);
                    }
                }
            }       
            return ans;
        }

        private void InitBoardData(int size)
        {
            _boardData = new BoardData(size, size);
        }

        private void InitPossibleUnitsPerCoord(int size)
        {
            _possibleUnitsPerCoordinate = new bool[size, size, size];

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    for (int k = 0; k < 9; k++)
                        _possibleUnitsPerCoordinate[i, j, k] = true;
                }
            }
        }


        private bool PlaceUnit(Unit unit, Position position)
        {
            var x = position.GetX();
            var y = position.GetY();
            return PlaceUnit(unit, x, y);
        }

        private bool PlaceUnit(Unit unit, int x, int y)
        {
            Debug.Log("BoardManager:PlaceUnit");
            var unitIndex = unit.Value - 1;
            Debug.Log("Attempting PlaceUnit position, value:" + x.ToString() + " / " + y.ToString() + ", " + unit.Value.ToString());
            if (!_possibleUnitsPerCoordinate[x, y, unitIndex])
            {
                Debug.Log("Position is taken! Aborting PlaceUnit!");
                return false;
            }

            if (_boardData.PlaceUnit(x, y, unit))
            {
                SetUnitIllegal(x, y, unitIndex);
                Debug.Log("PlaceUnit succeeded - position, value:" + x.ToString() + " / " + y.ToString() + ", " + unit.Value.ToString());

                return true;
            }

            return false;
        }

        private void SetUnitIllegal(int x, int y, int unitIndex)
        {
            IllegalizePosition(x, y);
            IllegalizeLines(x, y, unitIndex);
            IllegalizeSquare(x, y, unitIndex);
        }

        private void IllegalizePosition(int x, int y)
        {
            for(int i = 0; i < 9; i++)
            {
                _possibleUnitsPerCoordinate[x, y, i] = false;
            }
        }

        public void IllegalizeSquare(int x, int y, int unitIndex)
        {
            var square = new Square(x, y);

            for (int i = square.startX; i < square.startX + 3; i++)
            {
                for (int j = square.startY; j < square.startY + 3; j++)
                {
                    _possibleUnitsPerCoordinate[i, j, unitIndex] = false;
                }
            }
        }

        public void IllegalizeLines(int x, int y, int unitIndex)
        {
            for (int i = 0; i < _size; i++)
            {
                _possibleUnitsPerCoordinate[i, y, unitIndex] = false;
                _possibleUnitsPerCoordinate[x, i, unitIndex] = false;
            }

        }
    }

    public class Square
    {
        public int startX;
        public int startY;

        public Square(int x, int y)
        {
            startX = x / 3;
            startX *= 3;

            startY = y / 3;
            startY *= 3;
        }

    }

}
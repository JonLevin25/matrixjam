using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team4
{
    public class BoardData
    {
        public int sizeX;
        public int sizeY;
        public Unit[,] _boardData;

        public BoardData(int x, int y)
        {
            sizeX = x;
            sizeY = y;

            _boardData = new Unit[sizeX, sizeY];
        }

        public BoardData(Unit[,] boardData)
        {
            sizeX = boardData.GetLength(0);
            sizeY = boardData.GetLength(1);

            _boardData = (Unit[,])boardData.Clone();
        }

        public bool PlaceUnit(int x, int y, Unit unit)
        {
            if ( _boardData[x,y] == null )
            {
                _boardData[x, y] = unit;
                return true;
            }

            return false;
        }
    }
}

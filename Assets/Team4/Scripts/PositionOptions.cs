using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team4
{        
    /* a dictionary where the key is a unit, the value is a list with all possible positions 
    for that unit */
    public class PositionOptions 
    { 
        private Dictionary<Unit, List<Position>> _unitToPossiblePositions;

        public PositionOptions(Dictionary<Unit, List<Position>> data)
        {
            _unitToPossiblePositions = data;
        }

        public List<Position> GetPossiblePositions(Unit unit)
        {
            return _unitToPossiblePositions[unit];
        }
    }
}

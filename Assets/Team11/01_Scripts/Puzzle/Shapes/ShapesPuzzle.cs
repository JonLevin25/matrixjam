using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team11.Puzzle
{
    public class ShapesPuzzle : PuzzleManager
    {
        [SerializeReference] protected ShapeSlot.CodeShape[] _code;
        [SerializeReference] protected ShapeSlot[] _slots;

        private void Awake()
        {
            if (this._code.Length != this._slots.Length)
            {
                Debug.LogError($"{this.name}: ShapesPuzzle must have the exact same amount shapes and sltos! (got: {this._code.Length} != {this._slots.Length})");
            }

            if (this._code.Length == 0)
            {
                Debug.LogWarning($"{this.name}: ShapesPuzzle is empty!"); 
            }
        }

        public override bool CheckSolutionCorrect()
        {
            for (int i = 0; i < this._code.Length; i++)
            {
                if (this._code[i] != this._slots[i].Value)
                {
                    return false;
                }
            }

            return true;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MatrixJam.Team11.Puzzle
{
    public abstract class PuzzleManager : MonoBehaviour
    {
        [SerializeField] protected UnityEvent OnSolved;
        
        private bool _isSolved;
        public bool IsSolved { get { return this._isSolved; } }

        protected virtual void Start()
        {
            this._isSolved = false;
        }
        
        public virtual void Solve()
        {
            this._isSolved = true;

            if (this.OnSolved != null)
            {
                //TO_IDO: add puzzle solution SFX.
                SFXPlayer.instance.PlaySFX(SFXPlayer.instance.solvePuzzleSFX2);
                this.OnSolved.Invoke();
            }

            Debug.Log($"{this.name}: Puzzle was solved! :D");
        }

        public virtual void UpdateSolved()
        {
            Debug.Log($"{this.name}: Checking puzzle...");

            if (this.CheckSolutionCorrect())
            {
                Invoke("Solve", 0.5f);
            }
        }

        public abstract bool CheckSolutionCorrect();
    }
}

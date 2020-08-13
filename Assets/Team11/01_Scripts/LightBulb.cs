using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team11
{
    public class LightBulb : MonoBehaviour
    {
        LighbulbPuzzle puzzleObject;
        Animator _animator;
        public bool lit = false;


       


        // Start is called before the first frame update
        void Start()
        {
            puzzleObject = FindObjectOfType<LighbulbPuzzle>();
            _animator = GetComponent<Animator>();
        }

        public void ChangeValue()
        {
            SFXPlayer.instance.PlaySFX(SFXPlayer.instance.lightbulbSFX);
            // TO_IDO: add lightbulb SFX
            lit = !lit;
            _animator.SetBool("lit", lit);
            puzzleObject.CheckForValueChange();
        }



    }
}

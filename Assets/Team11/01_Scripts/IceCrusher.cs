using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team11
{
    public class IceCrusher : MonoBehaviour
    {
        Animator _animator;
        FrozenKey frozenKey;

        // Start is called before the first frame update
        void Start()
        {
            frozenKey = FindObjectOfType<FrozenKey>();
            _animator = GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        public void HammerTime()
        {
            SFXPlayer.instance.PlaySFX(SFXPlayer.instance.hammertimeSFX);
            _animator.SetTrigger("crush");
        }

        void Hit()
        {
            _animator.ResetTrigger("crush");
            frozenKey.CheckForCrush();
        }

        
    }
}

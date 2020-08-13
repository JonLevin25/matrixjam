using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team11
{


    public class Door : MonoBehaviour
    {
        public bool isOpen;
        Animator _animator;
        Collider2D _collider;


        // Start is called before the first frame update
        void Start()
        {
            _animator = GetComponent<Animator>();
            _collider = GetComponent<Collider2D>();
            if(isOpen)
            {
                _collider.enabled = false;
                Open();
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Open()
        {
            _animator.SetBool("doorOpened", true);
            if(!isOpen)
            {
                SFXPlayer.instance.PlaySFX(SFXPlayer.instance.doorOpenSFX);
            }
        }

        public void DoorOpened() // called by animation event
        {
            isOpen = true;
            _collider.enabled = false;
        }


        public void Close()
        {
            if (_collider.enabled == false)
            {
                _animator.SetBool("doorOpened", false);
                _collider.enabled = true;
                isOpen = false;
                SFXPlayer.instance.PlaySFX(SFXPlayer.instance.doorCloseSFX);



            }

        }


    }

}

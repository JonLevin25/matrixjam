using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MatrixJam.Team9
{
    public class HealthBar : MonoBehaviour
    {
        private GameObject bar;

        Animator anim ;

        [SerializeField] private GameObject health1;
        [SerializeField] private GameObject health2;
        [SerializeField] private GameObject health3;
        [SerializeField] private GameObject health4;

         

        // Start is called before the first frame update
        private void Awake()
        {
            bar = GameObject.Find("HealthSpriteBar");
            anim = this.gameObject.GetComponent<Animator>();
        }

        public void ChangeHealth(int changeTo)
        {
            if (changeTo == 2)
            {
                health1.SetActive(false);
                health2.SetActive(true);
            }

            if (changeTo == 3)
            {
                health2.SetActive(false);
                health3.SetActive(true);
            }

            if (changeTo == 4)
            {
                health3.SetActive(false);
                health4.SetActive(true);
            }

            if (changeTo == 0)
            {
                health4.SetActive(false);
            }


        }

        public void showBar()
        {            
            anim.SetBool(("Pop"), true);
        }

        public void HideBar()
        {
            this.gameObject.SetActive(false);
        }

        // Update is called once per frame

        /*        public void SetSize(float sizeNormalized)
                {
                    bar.localScale = new Vector3(sizeNormalized, 1f);
                }
        */
    }
}

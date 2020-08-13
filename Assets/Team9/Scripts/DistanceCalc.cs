using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MatrixJam.Team9
{
    public class DistanceCalc : MonoBehaviour
    {
        public Transform FinalDistanceLength;
        public EggScript eggRef;

        private float _accelaration;
        private Vector3 _eggPosition;


        private Transform distBar;

        public float minimum = 0.01F;
        public float maximum = 1.00F;

        private Animator anim;


        // Start is called before the first frame update
        void Start()
        {
            _accelaration = eggRef.GetComponent<EggScript>().Acceleration;
            _eggPosition = eggRef.transform.position;
            distBar = transform.Find("DistanceBar");
            anim = GetComponent<Animator>();

        }

        // Update is called once per frame
        void Update()
        {

            float finalCalc = Vector3.Distance(FinalDistanceLength.transform.position , _eggPosition);
            float result = (finalCalc / 200);

            if (finalCalc > 0.001f)
            {
                SetSize(result);
            }

        }

        public void showBarDistance()
        {
            anim.SetBool(("Pop"), true);
        }

        public void SetSize(float sizeNormalized)
        {
/*            sizeNormalized = (sizeNormalized * 0.01f) * 0.5f;
*/

/*            sizeNormalized = Mathf.Clamp(sizeNormalized, 0f, 1f);
*/            distBar.localScale = new Vector3(sizeNormalized, 1f);
        }
    }
}

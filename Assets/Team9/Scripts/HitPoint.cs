using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team9
{
    public class HitPoint : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            Debug.Log("hit" + this.gameObject.name);
        }

    }
}

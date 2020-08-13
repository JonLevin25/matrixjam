using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team24
{
    public class Shot : MonoBehaviour
    {
        [SerializeField] float XVel;
        // Update is called once per frame
        void FixedUpdate()
        {
            transform.Translate(XVel, 0, 0);
        }

        private void OnEnable()
        {
            Invoke("EndShot", Stats.shotLifetime);
        }

        private void OnTriggerEnter(Collider other)
        {
            Destroy(gameObject);
        }

        void EndShot()
        {
            Destroy(gameObject);
        }
    }
}

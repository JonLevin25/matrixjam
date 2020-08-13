using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team22
{
    public class BambooBrain : MonoBehaviour
    {
        public GameObject hitSpark, bambooDebris;
        public int debrisToSpawn = 4;
        public void Miss()
        {
            Destroy(gameObject);

            GameManager.instance.UpdateStats(0, 1);
        }

        public void Slice()
        {
            GameManager.instance.UpdateStats(1, 0);
            Instantiate(hitSpark, transform.position, transform.rotation);

            for (int i = 0; i < debrisToSpawn; i++)
            {
                Instantiate(bambooDebris, transform.position, bambooDebris.transform.rotation);
            }

            Destroy(gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team9
{
    public class KillSelfParticle : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(KillMyself());
        }


        IEnumerator KillMyself()
        {
            yield return new WaitForSeconds(2);
            Destroy(this.gameObject);
        }
    }
}

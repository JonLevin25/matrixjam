using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team8
{
    public class PickAGoo : MonoBehaviour
    {

        [SerializeField]
        private Animator pickAGoo;
        private void Awake()
        {
            StartCoroutine("PicksAGoo");
        }
        public void ResetBool()
        {
            pickAGoo.SetBool("Pick", false);
        }
        IEnumerator PicksAGoo()
        {
            while (true)
            {
                pickAGoo.SetBool("Pick", true);
                yield return new WaitForSeconds(Random.Range(30f, 45f));
            }
        }
    }
}

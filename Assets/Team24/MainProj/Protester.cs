using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team24
{
    public class Protester : MonoBehaviour
    {
        int HP;

        bool activated = false;

        public void WalkToEdge()
        {
            if(!activated)
            StartCoroutine(WalkTillEdging());
        }

        IEnumerator WalkTillEdging()
        {
            activated = true;
            GM.instance.playerMech.GainHope(Stats.protesterHope);
            while(transform.position.z < GM.instance.sideWalk.position.z)
            {
                yield return new WaitForSeconds(0.12f);
                transform.Translate(0, 0, Stats.protesterSpeed);
            }
        }
    }
}

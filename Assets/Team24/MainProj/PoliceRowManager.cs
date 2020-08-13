using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team24
{
    public class PoliceRowManager : MonoBehaviour
    {
        public List<Policeman> row = new List<Policeman>();
        public GameObject barricade;

        float timeACorrector = Stats.policeAttackPace;

        public void Init()
        {
            //timeACorrector = Stats.policeAttackPace + (0.5f / row.Count);
            row.Clear();
            foreach (Policeman item in GetComponentsInChildren<Policeman>())
            {
                row.Add(item);
            }
            
            this.enabled = false;
        }

        IEnumerator RandomizeShot()
        {
            while(true)
            {
                yield return new WaitForSeconds(timeACorrector);
                if(row.Count > 1)
                {
                    row[Random.Range(1, row.Count - 1)].Shoot();
                }
                else
                    break;

            }
            GM.instance.RowEnded();
        }

        public void RemoveFromRow(Policeman policeman)
        {
            row.Remove(policeman);
            //timeACorrector = Stats.policeAttackPace;
        }

        public void StartShoot()
        {

            StartCoroutine(RandomizeShot());
        }
    }
}

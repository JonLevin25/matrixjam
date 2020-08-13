using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MatrixJam.Team24
{

    public class GM : MonoBehaviour
    {
        public static GM instance;

        public PlayerMech playerMech;

        public AudioSource camSourceHatikva;
        public AudioSource camSourceJer;
        public AudioClip hatikva;
        //public List<PoliceRowManager> policeRows = new List<PoliceRowManager>();
        public PoliceRowManager[] policeRowsArr;

        public GameObject playerShot;
        public GameObject protesterShot;
        public GameObject policeShot;

        public Transform sideWalk;
        public Image hopeImg;
        #region rows
        public PoliceRowManager RowManager0;
        public PoliceRowManager RowManager1;
        public PoliceRowManager RowManager2;
        public PoliceRowManager RowManager3;
        public PoliceRowManager RowManager4;

        #endregion

        int rowCount = 0;

        private void Start()
        {
            //foreach (GameObject item in GameObject.FindGameObjectsWithTag("Tag6"))
            //{
            //    policeRows.Add(item.GetComponent<PoliceRowManager>());
            //}

            //foreach (PoliceRowManager item in policeRows)
            //{
            //    item.Init();
            //    item.enabled = false;
            //}

            //policeRowsArr = new PoliceRowManager[5] {RowManager0, RowManager1, RowManager2, RowManager3, RowManager4 };

            for (int i = 0; i < policeRowsArr.Length; i++)
            {
                policeRowsArr[i].Init();
            }
            instance = this;

            policeRowsArr[0].enabled = true;
            policeRowsArr[0].StartShoot();
            camSourceHatikva.Play();
            Invoke("ChangeSong", hatikva.length);
        }

        public void RowEnded()
        {
            policeRowsArr[rowCount].enabled = false;
            Destroy( policeRowsArr[rowCount].barricade);
            rowCount++;
            if(rowCount < policeRowsArr.Length)
            {
                Stats.policeAttackPace -= 0.01f;
                policeRowsArr[rowCount].enabled = true;
                policeRowsArr[rowCount].StartShoot();
                playerMech.GainAbility();

            }
        }

        void ChangeSong()
        {
            camSourceHatikva.Stop();
            camSourceJer.Play();
        }
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team9
{
    public class ParalaxController : MonoBehaviour
    {
        private float _position = 0;

        public Transform Building;
        public Transform LayerBGD1;
        public Transform LayerBGD2;
        public Transform Sun;

        public float Position
        {
            get
            {
                return _position;
            }

            set
            {
                Building.transform.position = new Vector3(0, -18f-value /1f, 1);
                LayerBGD1.transform.position = new Vector3(0, -18f-value / 7f, 1);
                LayerBGD2.transform.position = new Vector3(0, -18f-value / 8f, 1);
                Sun.transform.position = new Vector3(0, -18-value / 50f, 1);

                _position = value;
            }
        }
    }
}

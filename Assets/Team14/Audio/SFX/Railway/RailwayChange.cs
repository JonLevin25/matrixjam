using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team14.Team
    {
        namespace MatrixJam.Team14.Team
{
    public class RailwayChange : MonoBehaviour
        {
            // Start is called before the first frame update
            public AudioSource Railway;
            public AudioClip First;
            public AudioClip Second;
            public AudioClip Third;

            private void Awake()
            {
                Railway = GetComponent<AudioSource>();


            }

        }
    }
}

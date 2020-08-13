using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team2
    {
        namespace MatrixJam.Team2
{
    public class SizeFitter : MonoBehaviour
        {
            [SerializeField] private float ratio;

            private Transform parent;

            void Start()
            {
                parent = transform.parent.transform;
                transform.localScale = GetComponentInParent<SpriteRenderer>().size / ratio;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team2
{
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(Collider2D))]
    public class Pickable : MonoBehaviour
    {
        private SpriteRenderer spriteRenderer;
        private Collider2D collider;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            collider = GetComponent<Collider2D>();
        }

        public void Pick()
        {
            spriteRenderer.sortingOrder = 100;
            collider.enabled = false;
        }

        public void Drop()
        {
            spriteRenderer.sortingOrder = 0;
            collider.enabled = true;
        }
    }
}

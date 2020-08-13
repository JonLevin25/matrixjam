using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team22
{
    public class BranchDebris : MonoBehaviour
    {
        public Sprite[] debrisSprite;
        public float force = 1000;

        private Rigidbody2D rb;
        private float randomRot;

        void Start()
        {
            Destroy(gameObject, 2f);

            GetComponent<SpriteRenderer>().sprite = debrisSprite[Random.Range(0, debrisSprite.Length)];
            transform.Rotate(new Vector3(0, 0, Random.Range(-180, 180)));

            rb = GetComponent<Rigidbody2D>();

            Vector3 dir = new Vector2(Random.Range(-1f, 0.25f), Random.Range(0f, 0.5f));
            rb.AddForce(dir * force);
        }

        private void Update()
        {
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team11
{
    public class Spikes : MonoBehaviour
    {
        [SerializeField] Vector2 forceToAdd;
        PlayerController player;
        // Start is called before the first frame update
        void Start()
        {
            player = FindObjectOfType<PlayerController>();
        }

        // Update is called once per frame
        void Update()
        {
            
        }


        private void OnCollisionStay2D(Collision2D collision)
        {
            if (collision.gameObject.GetComponent<PlayerController>() && player.wasHurt == false && player.isDead == false)
            {
                
                    player.HurtPlayer();
                    collision.gameObject.GetComponent<Rigidbody2D>().MovePosition((Vector2)collision.transform.position + forceToAdd);
                    collision.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                
            

            }
        }
    }
}

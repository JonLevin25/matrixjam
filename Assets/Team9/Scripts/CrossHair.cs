using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team9
{
    public class CrossHair : MonoBehaviour
    {


        private AudioSource AudioSourceGun;

        public AudioClip gun1;
        public AudioClip gun2;

        // Start is called before the first frame update
        void Start()
        {
            Cursor.visible = false;
            AudioSourceGun = this.gameObject.GetComponent<AudioSource>();
        }

        // Update is called once per frame
        private void Update()
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 18.0f;
            transform.position = Camera.main.ScreenToWorldPoint(mousePos);

            //I'm clicking and checking if i hit something 
            if (Input.GetMouseButtonDown(0))
            {

                AudioSourceGun.PlayOneShot(gun1, 0.100f);
                //shooting a ray to the mouse point
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
/*                Debug.DrawRay(transform.position, ray.direction, Color.green, 10f);
*/
                //storing the hit object
                RaycastHit2D hit = Physics2D.Raycast(transform.position, ray.direction);
                //checking if it's a birds

                if(hit.collider !=null )
                {
                    if (hit.collider.tag == "Tag1")
                    {
                        AudioSourceGun.PlayOneShot(gun2, 0.100f);
                        /*                    AudioSourceGun.clip = gun2;
                                            AudioSourceGun.Play();
                        */
                        BirdScript hitBird = hit.transform.GetComponent<BirdScript>();

                        //only if we hit something we call birds script
                        hitBird.GetHit();
                    }

                }
                else
                {
/*                    AudioSourceGun.clip = gun1;
                    AudioSourceGun.Play();
*/                }
            }

        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MatrixJam;

namespace MatrixJam.Team24
{
    public class PlayerMech : MonoBehaviour
    {
        float HP;

        public Sprite rightSp;
        public Sprite leftSp;
        public Sprite forwardSp;
        public Sprite backwardSp;

        bool movingRight;
        bool movingLeft;
        bool movingForward;
        bool movingBackward;
        bool pointingRight;

        bool isAnimatingRight;
        bool isAnimatingLeft;

        public float movementSpeed;
        public float minDistFromPlayer;
        public float camOffsetz;

        public GameObject player;
        public static GameObject GO;
        public Camera cam;
        public Collider coll;

        Renderer rnd;

        int advancement;

        private void Start()
        {
            GO = player;
            HP = Stats.playerHP;
            rnd = player.GetComponent<Renderer>();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                movingRight = true;
                pointingRight = true;
                
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                movingLeft = true;
                pointingRight = false;
                
            }
            if (Input.GetKeyDown(KeyCode.UpArrow))
                movingForward = true;
            if (Input.GetKeyDown(KeyCode.DownArrow))
                movingBackward = true;
            if (Input.GetKeyUp(KeyCode.RightArrow))
                movingRight = false;
            if (Input.GetKeyUp(KeyCode.LeftArrow))
                movingLeft = false;
            if (Input.GetKeyUp(KeyCode.UpArrow))
                movingForward = false;
            if (Input.GetKeyUp(KeyCode.DownArrow))
                movingBackward = false;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                RaycastHit hit;
                Ray ray = new Ray(player.transform.position, GetDir());
                Physics.Raycast(ray, out hit, 3);
                //print(hit.collider);
                Attack(hit);
                rnd.material.SetTexture(Shader.PropertyToID("_MainTex"), Anims.instance.protesterFlower.texture);
            }
            if(Input.GetKeyUp(KeyCode.Space))
                rnd.material.SetTexture(Shader.PropertyToID("_MainTex"), Anims.instance.protesterIdle.texture);

            if (Input.GetKeyDown(KeyCode.E))
                    Instantiate(GM.instance.playerShot, new Vector3(transform.position.x + 2f, transform.position.y, transform.position.z), Quaternion.identity);



        }

        void FixedUpdate()
        {
            if (movingForward && !movingBackward)
            {
                transform.Translate(0f, 0f, movementSpeed);
                //maincam.transform.position = new Vector3(Player.transform.position.x, maincam.transform.position.y, Player.transform.position.z - camOffsetz);
            }
            if (movingBackward && !movingForward)
            {
                transform.Translate(0f, 0f, -movementSpeed);
                //maincam.transform.position = new Vector3(Player.transform.position.x, maincam.transform.position.y, Player.transform.position.z - camOffsetz);
            }
            if (movingRight && !movingLeft)
            {
                transform.Translate(movementSpeed, 0f, 0f);
                //rnd.material.SetTexture(Shader.PropertyToID("_MainTex"), rightSp.texture);
                if (player.transform.position.x > cam.transform.position.x)
                    cam.transform.Translate(movementSpeed, 0, 0);
                if (!isAnimatingRight)
                    StartCoroutine(RightMovementAnim());
                //maincam.transform.position = new Vector3(Player.transform.position.x, maincam.transform.position.y, Player.transform.position.z - camOffsetz);
            }
            if (movingLeft && !movingRight)
            {
                transform.Translate(-movementSpeed, 0f, 0f);
                if(!isAnimatingLeft)
                StartCoroutine(LeftMovementAnim());
                //rnd.material.SetTexture(Shader.PropertyToID("_MainTex"), leftSp.texture);
                //maincam.transform.position = new Vector3(Player.transform.position.x, maincam.transform.position.y, Player.transform.position.z - camOffsetz);
            }


        }

        private void OnTriggerEnter(Collider other)
        {
            print(HP);
            if (other.gameObject.CompareTag("Tag1"))
                other.gameObject.GetComponent<Protester>().WalkToEdge();
            if (other.gameObject.CompareTag("Tag5"))
                GetHit(Stats.officerDamage);
        }

        private Vector3 GetDir()
        {
            if (pointingRight)
                return new Vector3(1f, 0, 0);
            else
                return new Vector3(-1f, 0, 0);
        }

       public void Attack(RaycastHit hit)
       {
            if (hit.collider)
            {
                hit.collider.gameObject.GetComponent<Policeman>().GetHit(Stats.playerDamage);
            }
       }

        public void GetHit(int dmg)
        {
            HP -= dmg;
            GM.instance.hopeImg.fillAmount = HP / Stats.playerHP;
            if(HP < 0)
            {
                MatrixJam.LevelHolder.Level.Restart();
            }
        }

        public void GainHope(int dmg)
        {
            HP += dmg;
        }
        
        public void GainAbility()
        {
            advancement++;
        }

        IEnumerator RightMovementAnim()
        {
            isAnimatingRight = true;
                foreach (Sprite frame in Anims.instance.playerAnimRight)
                {
                    if (movingRight && !movingLeft)
                    {
                        rnd.material.SetTexture(Shader.PropertyToID("_MainTex"), frame.texture);
                        yield return new WaitForSeconds(0.2f);
                    }
                    else
                    {
                        print("RightMove ended");
                        rnd.material.SetTexture(Shader.PropertyToID("_MainTex"), rightSp.texture);
                        break;
                    }
                }
            isAnimatingRight = false;
        }
        IEnumerator LeftMovementAnim()
        {
            isAnimatingLeft = true;
            foreach (Sprite frame in Anims.instance.playerAnimLeft)
            {
                if (!movingRight && movingLeft)
                {
                    rnd.material.SetTexture(Shader.PropertyToID("_MainTex"), frame.texture);
                    yield return new WaitForSeconds(0.2f);
                }
                else
                {
                    print("LeftMove ended");
                    rnd.material.SetTexture(Shader.PropertyToID("_MainTex"), leftSp.texture);
                    break;
                }
            }
            isAnimatingLeft = false;
        }

    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

namespace MatrixJam.Team24
{
    public class Policeman : MonoBehaviour/*, IBehavior*/
    {

        // System.Object enemy;
        int HP;
        //List<Protester> pros = new List<Protester>();
        bool isShooting;
        bool isDead;
        PoliceRowManager rowManager;
        //private bool playerExists;
        public VideoPlayer vp;

        Renderer rnd;

        public void GetHit(int damage)
        {
            HP -= damage;
            if (HP <= 0)
            {
                isDead = true;
                rowManager.RemoveFromRow(this);
                print("charModule died " + rowManager + " " + rowManager.row.Count);
                rnd.material.SetTexture(Shader.PropertyToID("_MainTex"), Anims.instance.unicorn.texture);
            }
        }

        //IEnumerator AttackPlayer(PlayerMech player)
        //{
        //    while (playerExists)
        //    {
        //        yield return new WaitForSeconds(Stats.policeAttackPace);
        //        player.GetHit(Stats.officerDamage);
        //        print("police attacks player");
        //    }
        //}

        //IEnumerator AttackProtesters()
        //{
        //    while (pros.Count > 0 && !playerExists)
        //    {
        //        yield return new WaitForSeconds(Stats.policeAttackPace);
        //        pros[0].GetHit(Stats.officerDamage);
        //        print("police attacks protester");
        //    }

        //}

        //private void OnTriggerEnter(Collider other)
        //{
        //    //if(other.gameObject.TryGetComponent<Protester>(out var enemy))
        //    //{
        //    //    enemy.SendMessage("GetHit");
        //    //}
        //    //else if(other.gameObject.TryGetComponent<PlayerMech>(out var enemy))
        //    //{

        //    //}

        //    if(other.gameObject.CompareTag("Tag0"))
        //    {
        //        //if(pros.Count == 0)
        //        //{
        //            StartCoroutine(AttackPlayer(other.gameObject.GetComponent<PlayerMech>()));
        //            playerExists = true;
        //        //}
        //    }
        //    else if (other.gameObject.CompareTag("Tag1"))
        //    {
        //        pros.Add(other.gameObject.GetComponent<Protester>());
        //        if(!playerExists && pros.Count == 1)
        //        StartCoroutine(AttackProtesters());
        //    }

        //}

        //private void OnTriggerExit(Collider other)
        //{
        //    if (other.gameObject.CompareTag("Tag0"))
        //    {
        //        playerExists = false;
        //        if(pros.Count > 0)
        //        {
        //            StartCoroutine(AttackProtesters());
        //        }
        //    }
        //    else if(other.gameObject.CompareTag("Tag1"))
        //    {
        //        pros.Remove(other.gameObject.GetComponent<Protester>());
        //    }
        //}

        private void Start()
        {
            HP = Stats.officerHP;
            rowManager = GetComponentInParent<PoliceRowManager>();
            vp = GetComponent<VideoPlayer>();
            rnd = GetComponent<Renderer>();
        }

        

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Tag3"))
            {
                GetHit(Stats.playerDamage);
                Destroy(other.gameObject);
            }
            else if (other.gameObject.CompareTag("Tag4"))
            {
                GetHit(Stats.protesterDamage);
                Destroy(other.gameObject);
            }
        }

        public void Shoot()
        {
            Instantiate(GM.instance.policeShot, new Vector3( transform.position.x - 2f, transform.position.y, transform.position.z), Quaternion.identity);
            if(!isShooting)
                StartCoroutine(ShootAnim());
        }

        IEnumerator ShootAnim()
        {
            isShooting = true;
            foreach (Sprite frame in Anims.instance.mgvAnim)
            {
                rnd.material.SetTexture(Shader.PropertyToID("_MainTex"), frame.texture);
                yield return new WaitForSeconds(0.08f);
                if (isDead)
                {
                    rnd.material.SetTexture(Shader.PropertyToID("_MainTex"), Anims.instance.unicorn.texture);
                    break;
                }
            }
            isShooting = false;
        }

    }
}

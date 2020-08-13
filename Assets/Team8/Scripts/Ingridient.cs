using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team8
{
    public enum IngridientType
    {
        Fish,
        Carrot,
        Tomato,
        Onion,
        Bread,
        Meat,
        Lettuce,
        Rice
    }
    public class Ingridient : MonoBehaviour
    {
        public IngridientType Type;
        public bool Collected = false;
        public bool Splatter = false;
        public GameObject ShadowReference;
        public Color DominentColor;
        [SerializeField]
        private ParticleSystem splatterParticles;

        [SerializeField]
        private GameObject[] splatPrefab;

        [SerializeField]
        private Transform ShadowParent;

        [SerializeField]
        private Color splatterColor;
        //helpers
        private GameObject splatPlaceHolder;
        private bool splattedAlready = false;
        private bool initialized = false;
        private void Start()
        {
            ShadowReference = GameManager.Instance.CreateShadow(ShadowParent, transform,0.6f,true);
        }
        private void OnCollisionEnter(Collision collision)
        {
            if ((collision.collider.tag != "Player" && collision.collider.tag != "Tag0" && collision.collider.tag != "Tag9") && Splatter && !splattedAlready)
            {
                GameManager.Instance.soundManager.PlaySound(
                    GameManager.Instance.soundManager.squishSounds[Random.Range(0, GameManager.Instance.soundManager.squishSounds.Length)], 0.5f);
                splattedAlready = true;
                splatterParticles.Play();
                GetComponent<MeshRenderer>().enabled = false;
                GetComponent<Rigidbody>().velocity = Vector3.zero;
                Vector3 newPos = collision.contacts[0].normal * 2f;
                splatPlaceHolder = Instantiate(splatPrefab[Random.Range(0,splatPrefab.Length)], collision.contacts[0].point + collision.contacts[0].normal * 0.01f, Quaternion.identity);
                splatPlaceHolder.transform.rotation = Quaternion.LookRotation(newPos);
                splatPlaceHolder.transform.eulerAngles = new Vector3(splatPlaceHolder.transform.eulerAngles.x, splatPlaceHolder.transform.eulerAngles.y,
                    Random.Range(0f, 360f));
                splatPlaceHolder.transform.localScale *= Random.Range(1f, 2f);
                splatPlaceHolder.GetComponentInChildren<MeshRenderer>().material.SetColor("_Color", splatterColor);
                Destroy(ShadowReference);
                Destroy(gameObject,1f);
            }
            else if(collision.collider.tag == "Tag7" && !initialized)
            {
                initialized = true;
                GetComponent<Rigidbody>().velocity *= 0.2f;
            }
            else if(collision.collider.tag == "Tag9")
            {
                Destroy(ShadowReference);
                Destroy(gameObject, 0.1f);
            }
        }
    }
}

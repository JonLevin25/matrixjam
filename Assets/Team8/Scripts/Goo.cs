using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team8
{
    public class Goo : MonoBehaviour
    {
        public bool Squashable = false;

        [SerializeField]
        private float movementSpeed = 10f;

        [SerializeField]
        private float rotationSpeed = 10f;

        [SerializeField]
        private float timeBetweenMovement = 2f;
        
        [SerializeField]
        private float checkIngridientsRadius = 1f;
        [SerializeField]
        private LayerMask ingridientLayer;
        [SerializeField]
        private LayerMask gooLayer;
        [SerializeField]
        private LayerMask groundLayer;

        [SerializeField]
        private Material eyes;

        [SerializeField]
        private GameObject[] gooSplatters;

        [SerializeField]
        private ParticleSystem splatterParticles;

        [SerializeField]
        private Renderer[] meshesToKill;

        [SerializeField]
        private Animator gooAnimator;

        [SerializeField]
        private ParticleSystem foodChump;
        //helpers
        private Rigidbody rigidbody;
        private GameObject splatPlaceHolder;
        private Transform cacheTransform;
        private Collider[] ingridientsCloseBy;
        private Collider closestIngridient;
        private Color initialColor;
        private Vector3 direction;
        private Vector3 newPos;
        private Vector3 ingridientCheck;
        private Ray ray;
        private RaycastHit hit;
        private int randomDirection = 0;
        private float distanceToClosestIngridient = 0f;
        private float counter = 0f;
        private float timeBeforeBlink = 0f;
        private bool walkAgain = true;
        public void InitializeSlime()
        {
            GameManager.Instance.CreateShadow(splatterParticles.transform, cacheTransform, 0.6f, true);
            gooAnimator = GetComponent<Animator>();
            rigidbody = GetComponent<Rigidbody>();
            cacheTransform = transform;
            StartCoroutine("Blink");   
            StartCoroutine("MoveGoo");
        }

        IEnumerator MoveGoo()
        {
            Squashable = true;
            while (true)
            {
                //if (walkAgain)
                //{
                //walkAgain = false;
                direction = GetRandomDirection();

                if (direction != Vector3.zero)
                {
                    gooAnimator.SetBool("Walk", true);
                       ray = new Ray(cacheTransform.position + Vector3.up * 3f, Vector3.down);
                    newPos = (direction - cacheTransform.position).normalized;
                    newPos = new Vector3(newPos.x * movementSpeed,rigidbody.velocity.y,newPos.z * movementSpeed);
                    rigidbody.velocity = Vector3.Lerp(rigidbody.velocity, newPos, 0.3f);

                    cacheTransform.rotation = Quaternion.Slerp(cacheTransform.rotation, Quaternion.LookRotation(rigidbody.velocity), rotationSpeed * Time.deltaTime);

                }
                else
                {
                    gooAnimator.SetBool("Walk", false);
                }
                yield return new WaitForFixedUpdate();
                //}
                //else
                //{
                //    if (rigidbody.velocity.y > 0f)
                //    {
                //        rigidbody.velocity = new Vector3(rigidbody.velocity.x * 0.95f, 0, rigidbody.velocity.z * 0.95f);
                //    }
                //    else
                //    {
                //        rigidbody.velocity = new Vector3(rigidbody.velocity.x * 0.95f, rigidbody.velocity.y, rigidbody.velocity.z * 0.95f);
                //    }

                //    if(rigidbody.velocity.magnitude < 0.05f)
                //    {
                //        walkAgain = true;
                //    }
                //}
                //yield return null;
            }
        }

        private Vector3 GetRandomDirection()
        {
            ingridientCheck = CheckForIngridients();
            if(ingridientCheck != Vector3.zero)
            {
                closestIngridient = null;
                return ingridientCheck;
            }
            return Vector3.zero;
        }

        private Vector3 CheckForIngridients()
        {
            ingridientsCloseBy = Physics.OverlapSphere(cacheTransform.position, checkIngridientsRadius, ingridientLayer);
            if (ingridientsCloseBy.Length > 1)
            {
                foreach (Collider ingridient in ingridientsCloseBy)
                {
                    if (ingridient.name == "Collider")
                    {
                        if (!ingridient.GetComponentInParent<Ingridient>().Collected)
                        {
                            if (closestIngridient)
                            {
                                if (Vector3.Distance(ingridient.transform.position, cacheTransform.position) < distanceToClosestIngridient)
                                {
                                    closestIngridient = ingridient;
                                    distanceToClosestIngridient = Vector3.Distance(cacheTransform.position, closestIngridient.transform.position);
                                }
                            }
                            else
                            {
                                closestIngridient = ingridient;
                                distanceToClosestIngridient = Vector3.Distance(cacheTransform.position, closestIngridient.transform.position);
                            }
                        }
                    }
                    else
                    {
                        if (!ingridient.GetComponent<Ingridient>().Collected)
                        {
                            if (closestIngridient)
                            {
                                if (Vector3.Distance(ingridient.transform.position, cacheTransform.position) < distanceToClosestIngridient)
                                {
                                    closestIngridient = ingridient;
                                    distanceToClosestIngridient = Vector3.Distance(cacheTransform.position, closestIngridient.transform.position);
                                }
                            }
                            else
                            {
                                closestIngridient = ingridient;
                                distanceToClosestIngridient = Vector3.Distance(cacheTransform.position, closestIngridient.transform.position);
                            }
                        }
                    }
                    
                }
                if (closestIngridient)
                {
                    return closestIngridient.transform.position;
                }
                else
                {
                    return Vector3.zero;
                }
            }
            else if (ingridientsCloseBy.Length == 1)
            {
                if (ingridientsCloseBy[0].name == "Collider")
                {
                    if (!ingridientsCloseBy[0].GetComponentInParent<Ingridient>().Collected)
                    {
                        return ingridientsCloseBy[0].transform.position;
                    }
                }
                else
                {
                    if (!ingridientsCloseBy[0].GetComponent<Ingridient>().Collected)
                    {
                        return ingridientsCloseBy[0].transform.position;
                    }
                }
            }


            return Vector3.zero;
        }
       public void Squashed()
        {

            Squashable = false;
            splatterParticles.Play();
            gooAnimator.enabled = false;
            foreach (Renderer mesh in meshesToKill)
            {
                mesh.enabled = false;
            }
            splatPlaceHolder = Instantiate(gooSplatters[Random.Range(0, gooSplatters.Length)], new Vector3(cacheTransform.position.x,0.1f,cacheTransform.position.z), Quaternion.Euler(-90,Random.Range(0,360),0));
           // splatPlaceHolder.transform.rotation = Quaternion.LookRotation(newPos);
           // splatPlaceHolder.transform.eulerAngles = new Vector3(splatPlaceHolder.transform.eulerAngles.x, splatPlaceHolder.transform.eulerAngles.y,
             //   Random.Range(0f, 360f));
            Destroy(gameObject, 1f);
        }
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.black;
            Gizmos.DrawWireSphere(transform.position, checkIngridientsRadius);
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Tag0")
            {
                if(other.name == "Collider")
                {
                    if (!other.GetComponentInParent<Ingridient>().Collected)
                    {
                        foodChump.Play();
                        Destroy(other.GetComponentInParent<Ingridient>().ShadowReference);
                        Destroy(other.transform.parent.gameObject);
                    }
                }
                else
                {
                    if (!other.GetComponent<Ingridient>().Collected)
                    {
                        Destroy(other.GetComponent<Ingridient>().ShadowReference);
                        Destroy(other.gameObject);
                    }
                }
            }
            else if(other.tag == "Tag9")
            {
                Destroy(gameObject);
            }
        }

        IEnumerator Blink()
        {
            while (true)
            {
                timeBeforeBlink = Random.Range(1f, 3f);

                eyes.SetColor("_Color", Color.black);
                yield return new WaitForSeconds(0.1f);
                eyes.SetColor("_Color", Color.white);
                yield return new WaitForSeconds(timeBeforeBlink);
            }
        }
    }
}

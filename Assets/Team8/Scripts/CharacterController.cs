using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team8
{
    public class CharacterController : MonoBehaviour
    {
        [HideInInspector]
        public List<Ingridient> CurrentlyHeldIngridients = new List<Ingridient>();

        [SerializeField]
        private float movementSpeed = 1f;

        [SerializeField]
        private float rotationSpeed = 1f;

        [SerializeField]
        private Transform firstIngridientPosition, secondIngridientPosition, thirdIngridientPosition,fourthIngridientPosition;

        [SerializeField]
        private Transform ShadowParent;

        [SerializeField]
        private Cooker cooker;

        [SerializeField]
        private Transform[] respawnLocations;

        [SerializeField]
        private AudioClip slip;
        [SerializeField]
        private AudioClip gooDeath;
        [SerializeField]
        private AudioClip throwing;
        [SerializeField]
        private AudioClip pickUp;
        [SerializeField]
        private AudioClip impact;


        [SerializeField]
        private LayerMask ground;
        //helpers
        private Rigidbody rigidbody;
        private Transform cacheTransform;
        private Transform closestLocation;
        private Animator animator;
        private GameObject product;
        private Vector3 inputValidation;
        private Vector3 direction;
        private Vector3 randomDirectionForIngridients;
        private Vector3 throwTheBunnyDaFuckAwayDirection;
        private Ray ray;
        private RaycastHit hit;
        private float xMovement = 0f;
        private float yMovement = 0f;
        private float movementMultiplierWithIngridients = 1f;
        private float timeBeforeFalling = 1f;
        private bool gotProduct = false;
        private bool fallen = false;
        private bool alreadyThrownIngridients = false;
        private bool respawning = false;
        private bool canMove = true;
        private void Start()
        {
            animator = GetComponentInChildren<Animator>();
            rigidbody = GetComponent<Rigidbody>();
            cacheTransform = transform;
            GameManager.Instance.CreateShadow(ShadowParent, cacheTransform,0.8f,false);
        }
        private void Update()
        {
            if (!fallen && !alreadyThrownIngridients && canMove)
            {
                CheckInput();
            }
        }
        private void FixedUpdate()
        {
           
                MoveCharacter();
        }

        private void CheckInput()
        {
            xMovement = Input.GetAxis("Horizontal");
            yMovement = Input.GetAxis("Vertical");
            ray = new Ray(cacheTransform.position, Vector3.down);
            Debug.DrawRay(cacheTransform.position, Vector3.down);
            if (Physics.Raycast(ray, out hit, 0.1f, ground))
            {
                animator.SetBool("Falling", false);
            }
            else
            {
                animator.SetBool("Falling", true);
            }

            if(CurrentlyHeldIngridients.Count <= 0 && !gotProduct)
            {
                animator.SetBool("HaveIngridients", false);
            }

        }

        private void MoveCharacter()
        {
            if (inputValidation.magnitude > 0.01f  && !alreadyThrownIngridients && !fallen || !canMove)
            {
                cacheTransform.rotation = Quaternion.Slerp(cacheTransform.rotation, Quaternion.LookRotation(rigidbody.velocity), rotationSpeed * Time.deltaTime);

                ray = new Ray(cacheTransform.position, Vector3.down);
                Debug.DrawRay(cacheTransform.position, Vector3.down);
                if (Physics.Raycast(ray, out hit, 0.1f, ground))
                {
                    cacheTransform.eulerAngles = new Vector3(0, cacheTransform.eulerAngles.y, 0);
                }
             }
            if (!fallen && !alreadyThrownIngridients && canMove)
            {
                inputValidation = new Vector3(xMovement, 0, yMovement);
                inputValidation = Vector3.ClampMagnitude(inputValidation, 1f);
                if (inputValidation.magnitude > 0.01f)
                {
                    animator.SetBool("Walk", true);
                    if (respawning)
                    {
                        respawning = false;
                    }
                    inputValidation = new Vector3(-inputValidation.z * movementSpeed * movementMultiplierWithIngridients
                        , rigidbody.velocity.y, inputValidation.x * movementSpeed * movementMultiplierWithIngridients);
                    rigidbody.velocity = Vector3.Lerp(rigidbody.velocity, inputValidation, 0.3f);

                    if (CurrentlyHeldIngridients.Count == 4 && !fallen)
                    {
                        timeBeforeFalling -= Time.deltaTime;
                        if (timeBeforeFalling <= 0)
                        {
                            FallOver();
                        }
                    }
                }
                else
                {
                    animator.SetBool("Walk", false);
                }
            }
        }

        private void CollectIngridient(Ingridient ingridient)
        {
            if (ingridient.Type == IngridientType.Meat)
            {
                ingridient.transform.GetChild(2).GetComponent<Collider>().isTrigger = true;
            }
            else
            {
                ingridient.GetComponent<Collider>().isTrigger = true;
            }
            animator.SetBool("HaveIngridients", true);
            GameManager.Instance.soundManager.PlaySound(pickUp, 0.5f);
            ingridient.Collected = true;
            ingridient.GetComponent<Rigidbody>().isKinematic = true;
            CurrentlyHeldIngridients.Add(ingridient);
            switch (CurrentlyHeldIngridients.Count)
            {
                case 1:
                    ingridient.transform.position = firstIngridientPosition.position;
                    ingridient.transform.parent = firstIngridientPosition;
                    movementMultiplierWithIngridients = 0.90f;
                    break;
                case 2:
                    ingridient.transform.position = secondIngridientPosition.position;
                    ingridient.transform.parent = secondIngridientPosition;
                    ingridient.ShadowReference.SetActive(false);
                    movementMultiplierWithIngridients = 0.80f;
                    break;
                case 3:
                    ingridient.transform.position = thirdIngridientPosition.position;
                    ingridient.transform.parent = thirdIngridientPosition;
                    ingridient.ShadowReference.SetActive(false);
                    movementMultiplierWithIngridients = 0.70f;
                    break;
                case 4:
                    ingridient.transform.position = fourthIngridientPosition.position;
                    ingridient.transform.parent = fourthIngridientPosition;
                    ingridient.ShadowReference.SetActive(false);
                    movementMultiplierWithIngridients = 0.5f;
                    timeBeforeFalling = Random.Range(1f, 2f);
                    break;
            }
        }

        public void ClearIngridients()
        {
            animator.SetBool("HaveIngridients", false);
            for (int i = 0; i < CurrentlyHeldIngridients.Count; i++)
            {
                CurrentlyHeldIngridients[i].ShadowReference.SetActive(true);
                Destroy(CurrentlyHeldIngridients[i].gameObject);
            }
            alreadyThrownIngridients = false;
            CurrentlyHeldIngridients.Clear();
            movementMultiplierWithIngridients = 1f;
        }

        public void ClearProduct()
        {
            animator.SetBool("HaveIngridients", false);
            Destroy(product);
            alreadyThrownIngridients = false;
            gotProduct = false;
            movementMultiplierWithIngridients = 1f;
        }
        public void GiveProduct()
        {
            animator.SetBool("HaveIngridients", false);
            alreadyThrownIngridients = false;
            gotProduct = false;
            movementMultiplierWithIngridients = 1f;
        }
        private void FallOver()
        {
            animator.SetBool("Falling", true);
            GameManager.Instance.soundManager.PlaySound(slip, 0.3f);
            fallen = true;
            for (int i = 0; i < CurrentlyHeldIngridients.Count; i++)
            {
                CurrentlyHeldIngridients[i].transform.parent = null;
                if (CurrentlyHeldIngridients[i].Type == IngridientType.Meat)
                {
                    CurrentlyHeldIngridients[i].transform.GetChild(2).GetComponent<Collider>().isTrigger = false;
                }
                else
                {
                    CurrentlyHeldIngridients[i].GetComponent<Collider>().isTrigger = false;
                }
                CurrentlyHeldIngridients[i].GetComponent<Rigidbody>().isKinematic = false;
                randomDirectionForIngridients = new Vector3(Random.Range(0f, 1f), 0, Random.Range(0f, 1f));
                CurrentlyHeldIngridients[i].GetComponent<Rigidbody>().AddForce(
                    (cacheTransform.forward.normalized + randomDirectionForIngridients) * Random.Range(5f,10f) , ForceMode.Impulse);
                CurrentlyHeldIngridients[i].GetComponent<Ingridient>().Splatter = true;
            }
            CurrentlyHeldIngridients.Clear();

            if (gotProduct)
            {
                ClearProduct();
            }

            rigidbody.AddForce(cacheTransform.forward.normalized * 3f, ForceMode.Impulse);
            movementMultiplierWithIngridients = 1f;
            Invoke("ResetFallen", 0.5f);
        }

        private void ResetFallen()
        {
            fallen = false;
        }
        private void DelayCooking()
        {
            alreadyThrownIngridients = false;
            cooker.CookIngridients(CurrentlyHeldIngridients);
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Tag0" && !gotProduct && !fallen && CurrentlyHeldIngridients.Count < 4)
            {
                if(other.name == "Collider")
                {
                    if (!other.GetComponentInParent<Ingridient>().Collected && !respawning)
                    {
                        CollectIngridient(other.GetComponentInParent<Ingridient>());
                    }
                }
                else
                {
                    if (!other.GetComponent<Ingridient>().Collected && !respawning)
                    {
                        CollectIngridient(other.GetComponent<Ingridient>());
                    }
                }
            }
            else if(other.tag == "Tag1")
            {
                if (CurrentlyHeldIngridients.Count == 3 && !other.GetComponent<Cooker>().CurrentlyCooking && !other.GetComponent<Cooker>().FinishedCooking && !alreadyThrownIngridients)
                {
                    alreadyThrownIngridients = true;
                    StartCoroutine(ThrowIngridients((cacheTransform.forward + Vector3.up),false,true,2f));
                    Invoke("DelayCooking",0.7f);
                }
                else if (other.GetComponent<Cooker>().FinishedCooking && CurrentlyHeldIngridients.Count == 0)
                {
                    gotProduct = true;
                        GameManager.Instance.soundManager.PlaySound(pickUp, 0.5f);
                        product = other.GetComponent<Cooker>().GiveProduct();

                    animator.SetBool("HaveIngridients", true);
                    product.GetComponent<Rigidbody>().isKinematic = true;
                        product.transform.position = firstIngridientPosition.position;
                        product.transform.parent = firstIngridientPosition;
                        movementMultiplierWithIngridients = 0.90f;
                }
            }
            else if(other.tag == "Tag2" && gotProduct)
            {
                other.GetComponent<Customer>().TakeProduct(product);
            }
            else if(other.tag == "Tag3")
            {
                if (other.GetComponent<Goo>().Squashable)
                {

                    GameManager.Instance.soundManager.PlaySound(gooDeath, 1f);
                    Invoke("FallOver", 0.1f);
                    other.GetComponent<Goo>().Squashed();
                }
            }
            else if(other.tag == "Tag4")
            {
                if (gotProduct)
                {
                    GameManager.Instance.soundManager.PlaySound(throwing, 0.5f);
                    rigidbody.velocity = Vector3.zero;
                    product.GetComponent<Rigidbody>().isKinematic = false;
                    alreadyThrownIngridients = true;
                    product.transform.parent = null;
                    product.GetComponent<Rigidbody>().AddForce((other.transform.forward + Vector3.up) * 2f, ForceMode.Impulse);
                    Invoke("ClearProduct",1f);
                }
                else if(CurrentlyHeldIngridients.Count > 0 && !alreadyThrownIngridients)
                {
                    alreadyThrownIngridients = true;
                    StartCoroutine(ThrowIngridients((other.transform.forward + Vector3.up),true,true,4f));
                }
            }
            else if(other.tag == "Tag9" && canMove)
            {
                canMove = false;
                respawning = true;
                RespawnPlayer();
            }
            else if(other.tag == "Tag10")
            {
                GameManager.Instance.StartGame();
                Destroy(other.gameObject);
            }
        }
        private void RespawnPlayer()
        {
            foreach (Transform respawnLocation in respawnLocations)
            {
                if (closestLocation)
                {
                    if(Vector3.Distance(cacheTransform.position,respawnLocation.position) < Vector3.Distance(cacheTransform.position, closestLocation.position))
                    {
                        closestLocation = respawnLocation;
                    }
                }
                else
                {
                    closestLocation = respawnLocation;
                }
            }
            cacheTransform.position = closestLocation.position;
            rigidbody.velocity = Vector3.zero;
            throwTheBunnyDaFuckAwayDirection = closestLocation.forward + Vector3.up * 2f;
            rigidbody.AddForce(throwTheBunnyDaFuckAwayDirection * 8f, ForceMode.Impulse);
            if (gotProduct)
            {
                ClearProduct();
            }
            else if(CurrentlyHeldIngridients.Count > 0)
            {
                ClearIngridients();
            }
            Invoke("ResetRespawning", 2.4f);
        }
        private void ResetRespawning()
        {
            GameManager.Instance.soundManager.PlaySound(impact, 0.7f);
            cacheTransform.rotation = Quaternion.Slerp(cacheTransform.rotation, Quaternion.LookRotation(rigidbody.velocity), rotationSpeed * Time.deltaTime);
            cacheTransform.eulerAngles = new Vector3(0, cacheTransform.eulerAngles.y, 0);
            canMove = true;
        }
        IEnumerator ThrowIngridients(Vector3 direction, bool clear, bool inverse, float multiplier)
        {
            rigidbody.velocity = Vector3.zero;
            if (inverse)
            {
                for (int i = CurrentlyHeldIngridients.Count - 1; i >= 0; i--)
                {
                    GameManager.Instance.soundManager.PlaySound(throwing, 0.5f);
                    CurrentlyHeldIngridients[i].GetComponent<Rigidbody>().isKinematic = false;
                    CurrentlyHeldIngridients[i].transform.parent = null;
                    CurrentlyHeldIngridients[i].GetComponent<Rigidbody>().AddForce(direction * multiplier, ForceMode.Impulse);
                    yield return new WaitForSeconds(0.3f);
                }
            }
            else
            {
                for (int i = 0; i < CurrentlyHeldIngridients.Count; i++)
                {
                    GameManager.Instance.soundManager.PlaySound(throwing, 0.5f);
                    CurrentlyHeldIngridients[i].GetComponent<Rigidbody>().isKinematic = false;
                    CurrentlyHeldIngridients[i].transform.parent = null;
                    CurrentlyHeldIngridients[i].GetComponent<Rigidbody>().AddForce(direction * multiplier, ForceMode.Impulse);
                    yield return new WaitForSeconds(0.3f);
                }
            }
            if (clear)
            {
                Invoke("ClearIngridients", 1f);
            }
        }
    }

}

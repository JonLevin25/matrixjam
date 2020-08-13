using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team8
{
    public class IngridientSpawner : MonoBehaviour
    {
        public bool StartSpawningIngridients = false;
        [SerializeField]
        private GameObject[] ingridients;

        [SerializeField]
        private float maxTimeBetweenIngridients = 2f;

        [SerializeField]
        private float throwingForce = 2f;

        [SerializeField]
        private int maxNumberOfBeforeRecipeIngridient = 3;

        [SerializeField]
        private Transform[] spawners;

        [SerializeField]
        private Customer drake, josh;

        [SerializeField]
        private CharacterController player;

        [SerializeField]
        private Transform[] roofTarget;
        [SerializeField]
        private Transform ingridientLocation;
        [SerializeField]
        private Transform shadowParent;
        [SerializeField]
        private Animator animator;
        //helpers
        private Transform cacheTransform;
        private Transform selectedTarget;
        private GameObject ingridientToSpawn;
        private GameObject ingridientPlaceHolder;
        private Vector3 newRotationDirection;
        private Vector3 ingridientDirection;
        private float timeToSpawnNextComponent = 0f;
        private float counter = 0f;
        private float timeToCompleteSpin = 0.25f;
        private float spinTimeCounter = 0f;
        private float distanceMultiplier = 1f;
        private float counterForAnimation = 0f;
        private float maxTimeToLookAround = 1f;
        private int ingridientCounter = 0;
        private int randomCustomer = 0;
        private bool currentlySpawningIngridient = false;
        private bool finishedRotating = false;
        private bool followingDrake = false;

        public void Start()
        {
            cacheTransform = transform;
            GameManager.Instance.CreateShadow(shadowParent, transform, 0.8f, false);
        }
        private void Update()
        {
            if (!currentlySpawningIngridient && StartSpawningIngridients)
            {
                counter += Time.deltaTime;
                if (counter >= timeToSpawnNextComponent)
                {
                    counter = 0f;
                    if (ingridientCounter >= maxNumberOfBeforeRecipeIngridient)
                    {
                        ingridientCounter = 0;
                        SpawnIngridient(ChooseRecipeIngridient());
                    }
                    else
                    {
                        ingridientCounter++;
                        SpawnIngridient(ingridients[Random.Range(0, ingridients.Length)]);
                    }
                    
                    timeToSpawnNextComponent = Random.Range(maxTimeBetweenIngridients / 2f, maxTimeBetweenIngridients);
                }
            }
            else
            {
                counterForAnimation += Time.deltaTime;
                if(counterForAnimation >= maxTimeToLookAround)
                {
                    counterForAnimation = 0f;
                    maxTimeToLookAround = Random.Range(10f, 15f);
                    animator.SetBool("Look Around", true);
                    Invoke("ResetLook", 1f);
                }
            }
        }

        private void SpawnIngridient(GameObject ingridient)
        {
            currentlySpawningIngridient = true;
            ingridientToSpawn = ingridient;
            StartCoroutine("RotateCharacterToTakeIngridient");
        }
        IEnumerator RotateCharacterToTakeIngridient()
        {
            spinTimeCounter = 0f;
            newRotationDirection = (spawners[Random.Range(0, spawners.Length)].position - cacheTransform.position).normalized;
            while (spinTimeCounter <= timeToCompleteSpin)
            {
                spinTimeCounter += Time.deltaTime;
                cacheTransform.rotation = Quaternion.Slerp(cacheTransform.rotation, Quaternion.LookRotation(newRotationDirection), spinTimeCounter / timeToCompleteSpin);
                cacheTransform.eulerAngles = new Vector3(0, cacheTransform.eulerAngles.y, 0);
                yield return null;
            }
            animator.SetBool("Take Ingridient", true);
        }
        IEnumerator RotateCharacterToThrowIngridient()
        {
            spinTimeCounter = 0f;
            selectedTarget = roofTarget[Random.Range(0, roofTarget.Length)];
            newRotationDirection = (selectedTarget.position - cacheTransform.position).normalized;
            
            while (spinTimeCounter <= timeToCompleteSpin)
            {
                spinTimeCounter += Time.deltaTime;
                cacheTransform.rotation = Quaternion.Slerp(cacheTransform.rotation, Quaternion.LookRotation(newRotationDirection), spinTimeCounter / timeToCompleteSpin);
                cacheTransform.eulerAngles = new Vector3(0, cacheTransform.eulerAngles.y, 0);
                yield return null;
            }
            animator.SetBool("Throw Ingridient", true);
        }
        public void ThrowIngridient()
        {
            ingridientPlaceHolder.GetComponent<Rigidbody>().isKinematic = false;
            ingridientPlaceHolder.transform.parent = null;
            ingridientDirection = transform.forward + transform.up;

            distanceMultiplier = Mathf.Clamp(Vector3.Distance(cacheTransform.position, selectedTarget.position), 2.5f, 3f) - 2f;
            ingridientPlaceHolder.GetComponent<Rigidbody>().AddForce(ingridientDirection * (throwingForce * distanceMultiplier), ForceMode.Impulse);
            currentlySpawningIngridient = false;
            Invoke("ResetThrow", 1f);
        }
        public void TakeIngridient()
        {
            ingridientPlaceHolder = Instantiate(ingridientToSpawn, spawners[Random.Range(0, spawners.Length)].position, Quaternion.identity);
            ingridientPlaceHolder.GetComponent<Rigidbody>().isKinematic = true;
            ingridientPlaceHolder.transform.position = ingridientLocation.position;
            ingridientPlaceHolder.transform.parent = ingridientLocation;

            StartCoroutine("RotateCharacterToThrowIngridient");
            Invoke("ResetTake", 1f);
        }
        private void ResetTake()
        {
            animator.SetBool("Take Ingridient", false);
        }
        private void ResetThrow()
        {
            animator.SetBool("Throw Ingridient", false);
        }
        public void ResetLook()
        {
            animator.SetBool("Look Around", false);
        }
        private GameObject ChooseRecipeIngridient()
        {
            randomCustomer = Random.Range(0, 2);

            if (player.CurrentlyHeldIngridients.Count > 0)
            {

                if (player.CurrentlyHeldIngridients.Count == 1)
                {
                    if(drake.CurrentlyChosenRecipe.IngridientOrder[0] == player.CurrentlyHeldIngridients[0].Type)
                    {
                        followingDrake = true;
                        foreach (GameObject ingridient in ingridients)
                        {
                           if(ingridient.GetComponent<Ingridient>().Type == drake.CurrentlyChosenRecipe.IngridientOrder[1])
                            {
                                return ingridient;
                            }
                        }
                    }
                    else
                    {
                        foreach (GameObject ingridient in ingridients)
                        {
                            if (ingridient.GetComponent<Ingridient>().Type == josh.CurrentlyChosenRecipe.IngridientOrder[1])
                            {
                                return ingridient;
                            }
                        }
                    }
                }
                else if(player.CurrentlyHeldIngridients.Count == 2)
                {
                    if (followingDrake)
                    {
                        foreach (GameObject ingridient in ingridients)
                        {
                            if (ingridient.GetComponent<Ingridient>().Type == drake.CurrentlyChosenRecipe.IngridientOrder[2])
                            {
                                return ingridient;
                            }
                        }

                    }
                    else
                    {
                        foreach (GameObject ingridient in ingridients)
                        {
                            if (ingridient.GetComponent<Ingridient>().Type == josh.CurrentlyChosenRecipe.IngridientOrder[2])
                            {
                                return ingridient;
                            }
                        }
                    }
                }
                else
                {
                    followingDrake = false;
                }
            }
            else
            {
                if (randomCustomer == 0)
                {
                    foreach (IngridientType ingridientType in drake.CurrentlyChosenRecipe.IngridientOrder)
                    {
                        foreach (GameObject ingridient in ingridients)
                        {
                            if (DoesPlayerDontHaveIngridient(ingridient.GetComponent<Ingridient>().Type, ingridientType))
                            {
                                return ingridient;
                            }
                        }
                    }
                }
                else
                {

                    foreach (IngridientType ingridientType in josh.CurrentlyChosenRecipe.IngridientOrder)
                    {
                        foreach (GameObject ingridient in ingridients)
                        {
                            if (DoesPlayerDontHaveIngridient(ingridient.GetComponent<Ingridient>().Type, ingridientType))
                            {

                                return ingridient;
                            }
                        }
                    }
                }
            }

            return ingridients[Random.Range(0, ingridients.Length)];
        }

        private bool DoesPlayerDontHaveIngridient(IngridientType ingridient1, IngridientType ingridient2)
        {
            if(player.CurrentlyHeldIngridients.Count > 0)
            {
                foreach (Ingridient heldIngridient in player.CurrentlyHeldIngridients)
                {
                    if(ingridient1 == heldIngridient.Type)
                    {
                        return false;
                    }
                }
            }

            return ingridient1 == ingridient2;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MatrixJam.Team8
{
    public class Customer : MonoBehaviour
    {
        public Recipe CurrentlyChosenRecipe;
        

        [SerializeReference]
        private CharacterController player;
        [SerializeField]
        private float startingYOffset = 1f;
        [SerializeField]
        private float timeBetweenBites = 0.4f;
        [SerializeField]
        private bool inverse = false;
        [SerializeField]
        private Transform shadowParent;
        [SerializeField]
        private Transform eatingLocation;
        [SerializeField]
        private RawImage FirstIngridientImage, SecondIngridientImage, ThirdIngridientImage,productImage;
        [SerializeField]
        private AudioClip eat, burp, takeIt, fall,shock,impact,crack,breakIt;
        [SerializeField]
        private RectTransform banner;
        [SerializeField]
        private RectTransform face;

        [SerializeField]
        private GameObject floorNoAnimator;

        [SerializeField]
        private GameObject floorWithAnimator;

        [SerializeField]
        private Collider tableCollider;
        [SerializeField]
        private Transform shadowOwner;
        [SerializeField]
        private Animator animator;
        [SerializeField]
        private CameraShake cameraShake;
        
        //helpers
        private float bannerScale = 0f;
        private float faceScale = 0f;
        private float headRotation = 0f;
        private int timeYouAte = 0;
        private int currentPosition = 0;
        private int currentPositionBeforeChecking = 0;
        private bool left = false;
        private bool eating = false;
        private bool inverseEating = true;
        private bool firstTime = true;
        private bool finishedGame = false;
        private Vector3 stepPlaceHolder;
        private Color newColor = new Color(0,0,0,0);
        private List<Material> materialsToEat = new List<Material>();
        private List<ParticleSystem> foodParticles = new List<ParticleSystem>();
        private Animator floorAnimator;
        private GameObject productCurrentlyHolding;
        private GameObject shadowReference;
        public void Start()
        {
            shadowReference = GameManager.Instance.CreateShadow(shadowParent, shadowOwner? shadowOwner : transform, 0.8f,false);
        }
        public void Initialize()
        {
            StartCoroutine("PullBanner");
        }
        private void GetNewRecipe()
        {
            do
            {
                CurrentlyChosenRecipe = GameManager.Instance.Recipes[Random.Range(0, GameManager.Instance.Recipes.Length)];
            } while (CurrentlyChosenRecipe == GameManager.Instance.Recipes[GameManager.Instance.Recipes.Length - 1]);
            UpdateIcons();
        }

        public void TakeProduct(GameObject product)
        {
            if (product.name == CurrentlyChosenRecipe.ProductPrefab.name + "(Clone)" && !eating && !finishedGame)
            {
                eating = true;
                FirstIngridientImage.color = newColor;
                SecondIngridientImage.color = newColor;
                ThirdIngridientImage.color = newColor;
                productImage.color = newColor;
                StartCoroutine("LiftBanner");
                GameManager.Instance.soundManager.PlaySound(takeIt, 0.7f);
                player.GiveProduct();
                productCurrentlyHolding = product;
                product.GetComponent<Rigidbody>().isKinematic = true;
                product.transform.position = eatingLocation.position;
                product.transform.parent = eatingLocation;
                StartCoroutine("Eat");
            }
        } 

        private void UpdateIcons()
        {
            foreach (IngridientType type in CurrentlyChosenRecipe.IngridientOrder)
            {
                foreach (Texture ingridientIcon in GameManager.Instance.IngridientsIcon)
                {
                    currentPositionBeforeChecking = currentPosition;
                    if (ingridientIcon.name == type.ToString())
                    {
                        currentPosition++;
                        switch (currentPosition)
                        {
                            case 1:
                                FirstIngridientImage.texture = ingridientIcon;
                                FirstIngridientImage.color = Color.white;
                                break;
                            case 2:
                                SecondIngridientImage.texture = ingridientIcon;
                                SecondIngridientImage.color = Color.white;
                                break;
                            case 3:
                                ThirdIngridientImage.texture = ingridientIcon;
                                ThirdIngridientImage.color = Color.white;
                                break;
                        }
                    }
                    if(currentPosition != currentPositionBeforeChecking)
                    {
                        break;
                    }
                }

                if(currentPosition >= 3)
                {
                    break;
                }
            }
            productImage.texture = CurrentlyChosenRecipe.RecipeIcon;
            productImage.color = Color.white;
        }
        IEnumerator Eat()
        {
            foreach (Transform child in productCurrentlyHolding.transform.GetChild(0))
            {
                foodParticles.Add(child.GetComponent<ParticleSystem>());
            }
            stepPlaceHolder = CurrentlyChosenRecipe.Steps;
            if (stepPlaceHolder.x < 0)
            {
                inverseEating = true;   
            }
            if(CurrentlyChosenRecipe.Axis.x == 1 && gameObject.name == "Panda")
            {
                stepPlaceHolder += Vector3.one * 6;
            }
            foreach (Material material in productCurrentlyHolding.GetComponent<MeshRenderer>().materials)
            {
                if (material.shader.name == "Team8/CutMe")
                {
                    materialsToEat.Add(material);
                }
            }
            yield return new WaitForSeconds(timeBetweenBites);
            foreach (Material material in materialsToEat)
            {
                material.SetVector("_Axis", CurrentlyChosenRecipe.Axis);
                
                if (inverseEating)
                {
                    material.SetFloat("_WorldY", stepPlaceHolder.z);
                }
                else
                {
                    material.SetFloat("_WorldY", stepPlaceHolder.x);
                }
            }
            GameManager.Instance.soundManager.PlaySound(eat, 0.7f);
            foreach (ParticleSystem particle in foodParticles)
            {
                particle.Play();
            }
            yield return new WaitForSeconds(timeBetweenBites);
            foreach (Material material in materialsToEat)
            {
                material.SetFloat("_WorldY", stepPlaceHolder.y);
            }
            GameManager.Instance.soundManager.PlaySound(eat, 0.7f);
            foreach (ParticleSystem particle in foodParticles)
            {
                particle.Play();
            }
            yield return new WaitForSeconds(timeBetweenBites);
            foreach (Material material in materialsToEat)
            {
                if (inverseEating)
                {
                    material.SetFloat("_WorldY", stepPlaceHolder.x);
                }
                else
                {
                    material.SetFloat("_WorldY", stepPlaceHolder.y);
                }
            }
            GameManager.Instance.soundManager.PlaySound(eat, 0.7f);
            foreach (ParticleSystem particle in foodParticles)
            {
                particle.Play();
            }
            yield return new WaitForSeconds(timeBetweenBites);
            foreach (Material material in materialsToEat)
            {
                material.SetVector("_Axis", Vector3.up);
                material.SetFloat("_WorldY", 10f);
            }
            GameManager.Instance.soundManager.PlaySound(burp, 0.7f);
            Destroy(productCurrentlyHolding);
            materialsToEat.Clear();
            foodParticles.Clear();
            inverseEating = false;
            FinishedEating();

        }
        private void FinishedEating()
        {
            eating = false;
            currentPosition = 0;
            timeYouAte++;
            animator.SetInteger("Fatness", timeYouAte);
            if (timeYouAte == 2)
            {
                floorNoAnimator.SetActive(false);
                floorWithAnimator.SetActive(true);
                floorAnimator = floorWithAnimator.GetComponentInParent<Animator>();
            }
            if (timeYouAte >= 4)
            {
                GameManager.Instance.EndGame();
                GameManager.Instance.soundManager.PlaySound(crack, 0.7f);
                cameraShake.shakeAmount = 1f;
                cameraShake.shakeDuration = 2f;
                Invoke("DelayPoint", 3f);
                GameManager.Instance.EndGame();
                floorWithAnimator.GetComponent<BoxCollider>().enabled = false;
                tableCollider.enabled = false;
            }
            else
            {
                StartCoroutine("PullBanner");
            }
        }
        private void Impact()
        {
            GameManager.Instance.soundManager.PlaySound(impact, 0.7f);
            cameraShake.shakeDuration = 0.5f;
            cameraShake.shakeAmount = 5f;
            Destroy(gameObject, 3f);
        }
        private void DelayPoint()
        {
            if (gameObject.name == "Panda")
            {
                floorAnimator.SetBool("BrakeJosh", true);
                
            }
            else
            {
                floorAnimator.SetBool("BrakeDrake", true);
            }
            timeYouAte++;
            GameManager.Instance.soundManager.PlaySound(breakIt, 0.7f);
            animator.SetInteger("Fatness", timeYouAte);
            Invoke("Impact", 4f);
            Invoke("Falling", 0.6f);
        }
        private void Falling()
        {
            GameManager.Instance.soundManager.PlaySound(fall, 0.7f);
        }
        public void FinishedGameBanner()
        {
            finishedGame = true;
            FirstIngridientImage.color = new Color(0, 0, 0, 0);
            SecondIngridientImage.color = new Color(0, 0, 0, 0);
            ThirdIngridientImage.color = new Color(0, 0, 0, 0);
            productImage.color = new Color(0, 0, 0, 0);
            face.GetComponent<RawImage>().color = new Color(0, 0, 0, 0);
            StartCoroutine("LiftBanner");
        }
        IEnumerator ShowFace()
        {
            StartCoroutine("MoveHead");
            faceScale = 1f;
            while (face.localScale.y < 1f)
            {
                face.localScale += Vector3.one * (faceScale * Time.deltaTime / 1f);
                yield return null;
            }
            face.localScale = new Vector3(1f, 1f, 1f);
            GetNewRecipe();
        }
        IEnumerator MoveHead()
        {
            left = (Random.Range(0, 2) == 0);
            headRotation = 0f;
            Quaternion from = Quaternion.Euler(0f, 0f, 15f);
            Quaternion to = Quaternion.Euler(0f, 0f, -15f);
            while (true)
            {
                float lerp = 0.5F * (1.0F + Mathf.Sin(Mathf.PI * Time.realtimeSinceStartup * 0.2f * (inverse? 1:-1)));
                face.localRotation = Quaternion.Lerp(from, to, lerp);
                yield return null;
            }
        }
        IEnumerator PullBanner()
        {
            bannerScale = 1f;
            while (banner.localScale.y < 1f)
            {
                banner.localScale = new Vector3(banner.localScale.x, banner.localScale.y + (bannerScale * Time.deltaTime / 1f), banner.localScale.z);
                yield return null;
            }
            banner.localScale = new Vector3(1f, 1f, 1f);
            if (firstTime)
            {
                StartCoroutine("ShowFace");
            }
            else
            {
                GetNewRecipe();
            }
        }

        IEnumerator LiftBanner()
        {
            bannerScale = 1f;
            while (banner.localScale.y > 0f)
            {
                banner.localScale = new Vector3(banner.localScale.x, banner.localScale.y - (bannerScale * Time.deltaTime / 1f), banner.localScale.z);
                yield return null;
            }
            banner.localScale = new Vector3(1f, 0f, 1f);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team8
{
    public class Cooker : MonoBehaviour
    {
        public bool CurrentlyCooking = false;
        public bool FinishedCooking = false;

        [SerializeField]
        private Recipe guu;

        [SerializeField]
        private Recipe onionGoo;
        [SerializeField]
        private Transform releasePosition;

        [SerializeField]
        private CharacterController player;

        [SerializeField]
        private float timeBeforeRelease = 1f;

        [SerializeField]
        private ParticleSystem[] particleSystemsToActivate;

        [SerializeField]
        private AudioClip cooking;

        [SerializeField]
        private AudioClip[] goolaugh;

        [SerializeField]
        private ParticleSystem[] foodChumps;
        //helpers
        private Recipe chosenRecipe;
        private GameObject product;
        private ParticleSystem.MainModule placeHolder;
        private Vector3 direction;
        public void CookIngridients(List<Ingridient> ingridients)
        {

            for (int i = 0; i < ingridients.Count; i++)
            {
                placeHolder = foodChumps[i].main;
                placeHolder.startColor = ingridients[i].DominentColor;
            }
            CurrentlyCooking = true;
            chosenRecipe = CheckRecipe(ingridients);
            foreach (ParticleSystem particle in particleSystemsToActivate)
            {
                particle.Play();
            }
            player.ClearIngridients();
            GameManager.Instance.soundManager.PlaySound(cooking, 0.6f);
            Invoke("ReleaseProduct",timeBeforeRelease);
        }

        private void ReleaseProduct()
        {
            product = Instantiate(chosenRecipe.ProductPrefab, releasePosition.position, Quaternion.identity);
            FinishedCooking = true;
            CurrentlyCooking = false;
            foreach (ParticleSystem particle in particleSystemsToActivate)
            {
                particle.Stop();
            }
            if(chosenRecipe == guu || chosenRecipe == onionGoo)
            {
                FinishedCooking = false;
                Invoke("FuckGooUp",1f);
            }
        }

        private void FuckGooUp()
        {
            GameManager.Instance.soundManager.PlaySound(goolaugh[Random.Range(0, goolaugh.Length)], 0.7f);
            direction = (Vector3.up + Vector3.left + Vector3.back) * Random.Range(3f, 5f);
            product.GetComponent<Rigidbody>().AddForce(direction, ForceMode.Impulse);
            product.transform.rotation = Quaternion.LookRotation(direction);
            product.transform.eulerAngles = new Vector3(0, product.transform.eulerAngles.y, 0);
            product.GetComponent<Goo>().Invoke("InitializeSlime", 2f);
        }
        public Recipe CheckRecipe(List<Ingridient> ingridients)
        {
            foreach (Recipe recipe in GameManager.Instance.Recipes)
            {
                if (IngridientMatch(recipe.IngridientOrder[0], ingridients[0].Type))
                {
                    if(IngridientMatch(recipe.IngridientOrder[1], ingridients[1].Type))
                    {
                        if(IngridientMatch(recipe.IngridientOrder[2], ingridients[2].Type))
                        {
                            return recipe;
                        }
                    }
                }
            }

            return guu;
        }

        public GameObject GiveProduct()
        {
            FinishedCooking = false;
            return product;
        }
        private bool IngridientMatch(IngridientType ingridient1, IngridientType ingridient2)
        {
            return ingridient1 == ingridient2;
        }
    }
}

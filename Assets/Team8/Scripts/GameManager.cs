using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team8
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;
        public Recipe[] Recipes;
        public Texture[] IngridientsIcon;
        public SoundManager soundManager;
        [SerializeField]
        private GameObject shadowPrefab;

        [SerializeField]
        private IngridientSpawner shopkeeper;

        [SerializeField]
        private Customer drake, josh;

        [SerializeField]
        private AudioSource music;

        [SerializeField]
        private AudioClip musicAudioClip;

        [SerializeField]
        private AudioClip gong;
        
        //helpers
        private GameObject shadowPlaceHolder;
        private float startingVolume = 0f;
        private void Awake()
        {
            Instance = this;
        }

        public void DelayStart()
        {
            music.clip = musicAudioClip;
            music.volume = 0.2f;
            music.Play();
            drake.Initialize();
            josh.Initialize();
            Invoke("StartSpawningIngridients", 2f);
        }
        private void StartSpawningIngridients()
        {
            shopkeeper.StartSpawningIngridients = true;
            shopkeeper.ResetLook();
        }
        public void StartGame()
        {
            StartCoroutine("FadeOutChimes");
        }
        public GameObject CreateShadow(Transform shadowParent,Transform shadowHolder,float sizeMultiplier,bool goingHigh)
        {
            shadowPlaceHolder = Instantiate(shadowPrefab, shadowParent.position, Quaternion.Euler(-90,0,0));
            shadowPlaceHolder.GetComponent<ShadowOnGround>().Parent = shadowParent;
            shadowPlaceHolder.GetComponent<ShadowOnGround>().ShadowOwner = shadowHolder;
            shadowPlaceHolder.GetComponent<ShadowOnGround>().GoingHigh = goingHigh;
            shadowPlaceHolder.transform.localScale *= sizeMultiplier;
            return shadowPlaceHolder;
        }

        public void EndGame()
        {
            StartCoroutine("FadeOutMusic");
            shopkeeper.StartSpawningIngridients = false;
            drake.FinishedGameBanner();
            josh.FinishedGameBanner();
        }
        IEnumerator FadeOutChimes()
        {
            startingVolume = music.volume;
            while (music.volume > 0f)
            {
                music.volume -= startingVolume * Time.deltaTime / 1.5f;
                yield return null;
            }
            yield return new WaitForSeconds(0.5f);
            soundManager.PlaySound(gong, 0.8f);
            Invoke("DelayStart", 2f);
        }
        IEnumerator FadeOutMusic()
        {
            startingVolume = music.volume;
            while (music.volume > 0f)
            {
                music.volume -= startingVolume * Time.deltaTime / 1f;
                yield return null;
            }
        }
    }
}

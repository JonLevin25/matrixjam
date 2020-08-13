using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team8
{
    public class SplaterFade : MonoBehaviour
    {
        [SerializeField]
        private MeshRenderer[] meshesToTakeMaterialsFrom;

        [SerializeField]
        private float timeToFade = 1f;

        [SerializeField]
        private bool gooOne = false;

        [SerializeField]
        private bool oger = false;
        [SerializeField]
        private Material eyes;
        //helpers
        private List<Material> mainMaterials = new List<Material>();
        private float counter = 0f;
        private Color zeroAlpha;
        private void OnEnable()
        {
            foreach (MeshRenderer mesh in meshesToTakeMaterialsFrom)
            {
                foreach (Material material in mesh.materials)
                {
                    mainMaterials.Add(material);
                }
            }
            if (gooOne)
            {
                if (!oger)
                {
                    eyes = mainMaterials[1];
                }
                else
                {
                    eyes = mainMaterials[2];
                }
                StartCoroutine("Blink");
            }

            else
            {
                Invoke("StartFadeOut", Random.Range(1f, 3f));
            }
        }
        
        private void StartFadeOut()
        {
            StartCoroutine("FadeOut");
        }
        IEnumerator Blink()
        {
            yield return new WaitForSeconds(1f);
            eyes.SetColor("_Color", Color.black);
            yield return new WaitForSeconds(0.1f);
            eyes.SetColor("_Color", Color.white);
            yield return new WaitForSeconds(1f);
            eyes.SetColor("_Color", Color.black);
            yield return new WaitForSeconds(0.1f);
            eyes.SetColor("_Color", Color.white);
            yield return new WaitForSeconds(1f);
            StartFadeOut();
        }
        IEnumerator FadeOut()
        {
            counter = 1f;
            while (mainMaterials[mainMaterials.Count-1].color.a > 0)
            {
                foreach (Material material in mainMaterials)
                {
                    material.color = new Color(material.color.r, material.color.g, material.color.b, material.color.a - (counter * Time.deltaTime / timeToFade));
                }
                yield return null;
            }
            Destroy(transform.parent.gameObject);
        }
    }
}

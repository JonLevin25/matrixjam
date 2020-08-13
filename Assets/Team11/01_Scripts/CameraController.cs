using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MatrixJam.Team11
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] GameObject gameCamera;
        [SerializeField] GameObject mapCamera;
        [SerializeField] TextFader text;
        [SerializeField] Image[] sliderImages;
        [SerializeField] float fadedAlpha = 0.1f;

        bool affectsPlayer = false;
        // Start is called before the first frame update
        void Start()
        {
            Invoke("EnableCameraControls" ,1f);
        }

        void EnableCameraControls()
        {
            affectsPlayer = true;
        }


        // Update is called once per frame
        void Update()
        {
            if(!affectsPlayer)
            {
                return;
            }
            if(Input.GetKey(KeyCode.M) && mapCamera.activeSelf == false)
            {
                mapCamera.SetActive(true);
                GetComponent<PlayerController>().canMove = false;
                foreach (var image in sliderImages)
                {
                    image.color = new Color(image.color.r, image.color.g, image.color.b, fadedAlpha);
                }


                if(text != null)
                {
                    text.FadeOut();
                }
            }
            else if(mapCamera.activeSelf && !Input.GetKey(KeyCode.M))
            {
                mapCamera.SetActive(false);
                GetComponent<PlayerController>().canMove = true;
                foreach (var image in sliderImages)
                {
                    image.color = new Color(image.color.r, image.color.g, image.color.b, 1f);
                }
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team11
{
    public class FrozenKey : MonoBehaviour
    {
        bool wasTriggered = false;
        [SerializeField] GameObject blueKey;
        [SerializeField] GameObject iceCube;
        [SerializeField] float maxDistance;
        [SerializeField] Transform crushingTransform;
        Animator _animator;

        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
            
        }
        public void CheckForCrush()
        {
            if(!wasTriggered)
            {
                if (Vector2.Distance(iceCube.transform.position, crushingTransform.position) < maxDistance)
                {
                    Crush();
                }
            }
          
        }

        void Crush()
        {
            wasTriggered = true;
            iceCube.GetComponent<Animator>().SetTrigger("Crush");
            //TO_IDO: add ice shatter SFX
            SFXPlayer.instance.PlaySFX(SFXPlayer.instance.iceCrushSFX,0.8f);
          //  FindObjectOfType<PlayerController>().IceCubeCrushed(iceCube.gameObject);
            Destroy(iceCube.GetComponent<LiftableObject>());
            Destroy(iceCube.GetComponent<Collider2D>());
            blueKey.transform.position = iceCube.transform.position;
            blueKey.SetActive(true);
            Invoke("TeleportIceCubeAway", 0.2f);
           // Destroy(iceCube, 0.5f);

        }

        void TeleportIceCubeAway()
        {
            iceCube.transform.position = new Vector3(100, 100, 100);
        }
    }
}

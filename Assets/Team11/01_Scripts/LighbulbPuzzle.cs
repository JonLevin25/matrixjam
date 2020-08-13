using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MatrixJam.Team11
{
    public class LighbulbPuzzle : MonoBehaviour
    {
        [SerializeField] Door door;
        LightBulb[] lights;
        
        
        
        // Start is called before the first frame update
        void Start()
        {
            lights = FindObjectsOfType<LightBulb>();
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        public void CheckForValueChange()
        {
            StartCoroutine(ShortDelay());
        }

        void OpenDoor()
        {
            door.Open(); // TO_IDO: make a SFX for the golden doors and implement here. might be able to reuse the pressure plates SFX.
            SFXPlayer.instance.PlaySFX(SFXPlayer.instance.solvePuzzle1SFX);
            InteractableItem[] buttons = GetComponentsInChildren<InteractableItem>();
            foreach (var button in buttons)
            {
                button.canInteract = false;
                Debug.Log(button.canInteract);
            }
        }
        IEnumerator ShortDelay()
        {
            yield return new WaitForSeconds(0.05f);
            var litLights = lights.Where(light => light.lit == true);
            Debug.Log(litLights.Count());
            if (litLights.Count() == lights.Count())
            {
                Invoke("OpenDoor", 0.3f);
                //TO_IDO: play puzzle solved SFX
                SFXPlayer.instance.PlaySFX(SFXPlayer.instance.solvePuzzle1SFX);
            }
        }


    }
}

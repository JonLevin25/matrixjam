using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace MatrixJam.Team11
{
    public class PressurePlatePuzzleManager : MonoBehaviour
    {
        PressurePlate[] plates;
        [SerializeField] Door door;

        // Start is called before the first frame update
        void Start()
        {
            plates = FindObjectsOfType<PressurePlate>();

        }

        // Update is called once per frame
        void Update()
        {
            
        }

        public void CheckForValueChange()
        {
            StartCoroutine(ShortDelay());
        }
        IEnumerator ShortDelay()
        {
            yield return new WaitForSeconds(0.05f);
            var activePlates = plates.Where(plate => plate.isPressed == true);
            Debug.Log(activePlates.Count());
            if (activePlates.Count() == plates.Count())
            {

                // TO_IDO: Add Door SFX (needs a different sound than the "locked doors SFX" because it's one of the golden doors, not the colored with keys). the pressure plate SFX might work?
              //SFXPlayer.instance.PlaySFX(SFXPlayer.instance.solvePuzzleSFX3);
                door.Open();
            }
            else if(door.isOpen)
            {
                door.Close();
                // TO_IDO: Add Door closing SFX

            }
        }
            
            

    }
        
}

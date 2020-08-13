using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team2
{
    public class FlipPlayer : MonoBehaviour
    {
        // Please don't judge me.
        // Seriously.
        // Please.
        // I'm tired.
        // Nothing worked.
        // I swear, I tried.
        // I'll give you some free lines to spare me your kindness.
        // 
        //
        //
        //
        //
        //
        //


        [SerializeField] private Transform player;
        [SerializeField] private Transform firePoint;
        [SerializeField] private Transform shoulderB;
        [SerializeField] private TurretStupid gunRotationController;

        private bool isFlipped = false;
        private float gunRotationCorrectionDefault;

        void Start()
        {
            gunRotationCorrectionDefault = gunRotationController.correction;
        }

        void Update()
        {
            Vector3 mousePos = Input.mousePosition;
            Vector3 objectPos = Camera.main.WorldToScreenPoint(transform.position);

            if (mousePos.x < objectPos.x && !isFlipped ||
                mousePos.x >= objectPos.x && isFlipped)
            {
                isFlipped = mousePos.x < objectPos.x;
                Flip();
            }
        }

        private void Flip()
        {

            player.localScale = new Vector3(isFlipped ? -0.15f : 0.15f, 0.15f, 0.15f);
            firePoint.Rotate(0, 180, 0);
            gunRotationController.correction = gunRotationCorrectionDefault;
            if (isFlipped)
            {
                gunRotationController.correction -= 90;
            }
        }
    }
}

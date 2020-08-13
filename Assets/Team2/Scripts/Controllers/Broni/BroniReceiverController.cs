using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MatrixJam.Team2
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class BroniReceiverController : MonoBehaviour
    {
        [SerializeField] private float deactivationTime;
        [SerializeField] private UnityEvent onActived;
        [SerializeField] private UnityEvent onDeactived;
        [SerializeField] private Color deactivatedColor;
        [SerializeField] private Color activatedColor;

        private SpriteRenderer spriteRenderer;
        private bool isActive = false;
        private float deactivationTimer;

        void Start()
        {
            deactivationTimer = deactivationTime;
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        void Update()
        {
            // Deactivation countdown
            if (!isActive) return;
            deactivationTimer -= Time.deltaTime;
            if (deactivationTimer <= 0)
            {
                Deactivate();
            }
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            deactivationTimer = deactivationTime;
            if (other.TryGetComponent(out BulletController bulletController))
            {
                // TODO: Activate unity particle and/or change sprite.
                //if (!isActive)
                //{
                    Activate();
                //}
                Destroy(bulletController.gameObject);
            }
        }

        void Activate()
        {
            spriteRenderer.color = activatedColor;
            isActive = true;
            onActived.Invoke();
        }

        void Deactivate()
        {
            spriteRenderer.color = deactivatedColor;
            isActive = false;
            onDeactived.Invoke();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team2
{
    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(ReflectiveController))]
    [RequireComponent(typeof(GoThroughController))]
    public class Floopable : MonoBehaviour
    {
        [SerializeField] SpriteRenderer spriteRenderer;
        [SerializeField] FloopableMaterialTypes currentMaterial;
        [SerializeField] Sprite reflectiveSprite;
        [SerializeField] Sprite opaqueSprite;
        [SerializeField] Sprite goThroughSprite;

        private BoxCollider2D boxCollider;
        private ReflectiveController reflectiveController;
        private GoThroughController goThroughController;

        void Start()
        {
            boxCollider = GetComponent<BoxCollider2D>();
            reflectiveController = GetComponent<ReflectiveController>();
            goThroughController = GetComponent<GoThroughController>();
            UpdateMaterial();
        }

        public void ChangeMaterial(FloopableMaterialTypes newMaterial)
        {
            currentMaterial = newMaterial;
            UpdateMaterial();
        }

        public void ChangeBulletTrajectory(Transform bullet, ContactPoint2D contactPoint)
        {
            switch (currentMaterial)
            {
                case FloopableMaterialTypes.Reflective:
                    reflectiveController.ReflectLaser(bullet, contactPoint);
                    break;
                case FloopableMaterialTypes.Opaque:
                    Destroy(bullet.gameObject);
                    break;
            }
        }

        private void UpdateMaterial()
        {
            switch (currentMaterial)
            {
                // Using boxCollider.isTrigger so we can still detect it via raycast
                case FloopableMaterialTypes.Reflective:
                    boxCollider.isTrigger = false;
                    reflectiveController.enabled = true;
                    goThroughController.enabled = false;
                    spriteRenderer.sprite = reflectiveSprite;
                    break;
                case FloopableMaterialTypes.Opaque:
                    boxCollider.isTrigger = false;
                    reflectiveController.enabled = false;
                    goThroughController.enabled = false;
                    spriteRenderer.sprite = opaqueSprite;
                    break;
                case FloopableMaterialTypes.GoThrough:
                    boxCollider.isTrigger = true;
                    reflectiveController.enabled = false;
                    goThroughController.enabled = true;
                    spriteRenderer.sprite = goThroughSprite;
                    break;
            }
        }
    }
}

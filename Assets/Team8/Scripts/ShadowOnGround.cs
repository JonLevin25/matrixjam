using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team8
{
    public class ShadowOnGround : MonoBehaviour
    {
        public Transform Parent;
        public Transform ShadowOwner;
        public bool GoingHigh = false;

        [SerializeField]
        private Vector3 parentOffset = new Vector3(0f, 0.01f, 0f);

        [SerializeField]
        private LayerMask groundLayerMask;

        //helpers
        private Transform cacheTransform;
        private Ray ray;
        private RaycastHit hitInfo;
        private Vector3 initialScale;
        private bool firstTime = true;
        private float distanceFromPoint;
        private float initialDistanceFromPoint = 0.25f;
        private void Start()
        {
            cacheTransform = transform;
        }


        void Update()
        {
            if (!ShadowOwner)
            {
                Destroy(gameObject);
            }
            else
            {
                ray = new Ray(Parent.position + Vector3.up * 10, -Vector3.up);

                Parent.position = ShadowOwner.position;
                if (Physics.Raycast(ray, out hitInfo, 100f, groundLayerMask))
                {
                    if (firstTime)
                    {
                        firstTime = false;
                        initialScale = cacheTransform.localScale;
                        if (!GoingHigh)
                        {
                            initialDistanceFromPoint = Vector3.Distance(Parent.position, hitInfo.point) + Vector3.Distance(Parent.position, hitInfo.point) / 8;
                        }
                    }
                    // Positio
                    cacheTransform.position = new Vector3(Parent.position.x, hitInfo.point.y + parentOffset.y, Parent.position.z);
                    Vector3 newPos = hitInfo.normal * 2f;
                    distanceFromPoint = Vector3.Distance(Parent.position, hitInfo.point);
                    if(distanceFromPoint > initialDistanceFromPoint)
                    {
                        cacheTransform.localScale = initialScale - initialScale / 2 * (Mathf.Clamp(distanceFromPoint,0,2));
                    }
                    else
                    {
                        cacheTransform.localScale = initialScale;
                    }
                    cacheTransform.rotation = Quaternion.LookRotation(newPos);
                }
                else
                {
                    // If raycast not hitting (air beneath feet), position it far away
                    cacheTransform.position = new Vector3(0f, 110f, 0f);
                }
            }
        }
    }
}

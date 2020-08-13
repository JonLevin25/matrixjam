using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team2
{
    public class CameraLimit : MonoBehaviour
    {
        [SerializeField] private Camera cam;
        [SerializeField] private Transform target;
        [SerializeField, Range(0, 100)] private float xBoundSizePercentage = 90;
        [SerializeField, Range(0, 100)] private float yBoundSizePercentage = 80;
        [SerializeField] private int boundaryLayerMask = 8;
        [SerializeField] private float smoothTime = 0.125f;

        private Vector3 camOffset;
        private float camHalfHeight;
        private float camHalfWidth;
        private Vector2 camVelocity;


        void OnValidate()
        {
            UpdateCamSize();
        }

        private void OnDrawGizmos()
        {
            Vector3 position = transform.position;
            position.z = 0;
            Gizmos.color = Color.red;
            Gizmos.DrawLine(position, new Vector2(position.x - camHalfWidth, position.y));
            Gizmos.DrawLine(position, new Vector2(position.x + camHalfWidth, position.y));
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(position, new Vector2(position.x, position.y - camHalfHeight));
            Gizmos.DrawLine(position, new Vector2(position.x, position.y + camHalfHeight));
        }

        void Start()
        {
            camOffset = new Vector3(0, 0, -10);
            UpdateCamSize();
        }

        void LateUpdate()
        {
            Vector2 newCamPos = target.position;
            SetNewCameraPosInBound(ref newCamPos);
            transform.position = Vector2.SmoothDamp(transform.position, newCamPos, ref camVelocity, smoothTime);
            transform.position += camOffset;
        }

        private void UpdateCamSize()
        {
            camHalfHeight = cam.orthographicSize * yBoundSizePercentage / 100;
            // the width recalculates the height (cam.orthographicSize) because it's boundary shouldn't be effected by the height boundary.
            camHalfWidth = cam.aspect * cam.orthographicSize * xBoundSizePercentage / 100;
        }

        private void SetNewCameraPosInBound(ref Vector2 newCamPos)
        {
            newCamPos.x = GetNewCamPosInBoundX(newCamPos);
            newCamPos.y = GetNewCamPosInBoundY(newCamPos);
        }

        private float GetNewCamPosInBoundX(Vector2 newCamPos)
        {
            RaycastHit2D leftBoundHit = Physics2D.Raycast(newCamPos, Vector2.left, camHalfWidth, 1 << boundaryLayerMask);
            RaycastHit2D rightBoundHit = Physics2D.Raycast(newCamPos, Vector2.right, camHalfWidth, 1 << boundaryLayerMask);

            if (IsHit(leftBoundHit) && IsHit(rightBoundHit))
            {
                // Middle of both
                return leftBoundHit.point.x + ((rightBoundHit.point.x - leftBoundHit.point.x) / 2);
            }
            if (IsHit(leftBoundHit))
            {
                return leftBoundHit.point.x + camHalfWidth;
            }
            if (IsHit(rightBoundHit))
            {
                return rightBoundHit.point.x - camHalfWidth;
            }

            // Unchanged pos
            return newCamPos.x;
        }

        private float GetNewCamPosInBoundY(Vector2 newCamPos)
        {
            RaycastHit2D downBoundHit = Physics2D.Raycast(newCamPos, Vector2.down, camHalfHeight, 1 << boundaryLayerMask);
            if (IsHit(downBoundHit))
            {
                return downBoundHit.point.y + camHalfHeight;
            }

            RaycastHit2D upBoundHit = Physics2D.Raycast(newCamPos, Vector2.up, camHalfHeight, 1 << boundaryLayerMask);
            if (IsHit(upBoundHit))
            {
                return upBoundHit.point.y - camHalfHeight;
            }



            // Unchanged pos
            return newCamPos.y;
        }

        private bool IsHit(RaycastHit2D ray)
        {
            return ray.collider != null;
        }
    }
}
using System;
using MatrixJam.Team25.Scripts.Managers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MatrixJam.Team25.Scripts.Jar
{
    public class DragMove : MonoBehaviour, IDragHandler, IPointerDownHandler
    {
        public GameObject movable;
        public bool switchAxis;
        public GameObject arrows;
        private Vector3 mOffset;
        private float mZcoord;
        private Camera mainCam;
        private DataManager dataManager;

        private void Awake()
        {
            dataManager = FindObjectOfType<DataManager>();
            if (dataManager.round != 0 && arrows != null)
            {
                arrows.SetActive(false);
            }
        }

        private void OnEnable()
        {
            mainCam = Camera.main;
            if (movable == null)
                movable = gameObject;
        }

        private Vector3 GetMouseWorldPos()
        {
            Vector3 mousePoint = Input.mousePosition;
            mousePoint.z = mZcoord;
            return mainCam.ScreenToWorldPoint(mousePoint);
        }

        public void OnDrag(PointerEventData eventData)
        {
            Vector3 pos = GetMouseWorldPos() + mOffset;
            if (switchAxis)
            {
                pos.z = pos.y;
                pos.y = transform.position.y;
            }
            else
            {
                pos.z = transform.parent.position.z;
            }
            transform.position = pos;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            mZcoord = mainCam.WorldToScreenPoint(transform.position).z;
            mOffset = gameObject.transform.position - GetMouseWorldPos();
        }
    }
}
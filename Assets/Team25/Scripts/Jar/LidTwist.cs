using System;
using MatrixJam.Team25.Scripts.Managers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MatrixJam.Team25.Scripts.Jar
{
    public class LidTwist : MonoBehaviour, IDragHandler, IEndDragHandler, IPointerDownHandler
    {
        public float maxRotations;
        public float yPositionReductionRatio;
        public float yPosOffset;
        public GameObject arrows, lidTwistTutorial;
        private float maxRotationsAngle;
        private bool holding;
        private float totalYRotation;
        private float previousYRotation;
        private bool turn;
        [HideInInspector] public bool jarClosed;
        private bool rotationsReached;
        private Vector3 pos;
        private DataManager dataManager;
        private GameManager gameManager;

        private void Awake()
        {
            dataManager = FindObjectOfType<DataManager>();
            gameManager = FindObjectOfType<GameManager>();
        }

        private void OnEnable()
        {
            if (dataManager.round == 0)
            {
                lidTwistTutorial.SetActive(true);
                arrows.SetActive(true);
            }
            maxRotationsAngle = maxRotations * 360;
            totalYRotation = transform.rotation.eulerAngles.y;
        }

        public void OnDrag(PointerEventData eventData)
        {
            turn = true;
            previousYRotation = transform.rotation.eulerAngles.y;
            var rot = Quaternion.Euler(Quaternion.identity.eulerAngles.x, -eventData.position.x,
                Quaternion.identity.eulerAngles.z);
            var dir = Mathf.Sign(eventData.delta.x);
            if (dir < 0)
            {
                if (!rotationsReached && rot.eulerAngles.y - previousYRotation >= 0)
                {
                    totalYRotation += rot.eulerAngles.y - previousYRotation;
                    pos = transform.position;
                    pos.x = 0;
                    pos.z = 0;
                    pos.y = ((maxRotationsAngle - totalYRotation) / yPositionReductionRatio) + yPosOffset;
                }

                if (totalYRotation >= maxRotationsAngle)
                {
                    turn = false;
                    rotationsReached = true;
                }
            }
            else
            {
                turn = false;
            }

            if (turn)
            {
                transform.localPosition = pos;
                transform.localRotation = rot;
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
        }
        

        public void OnEndDrag(PointerEventData eventData)
        {
            if (rotationsReached)
            {
                transform.rotation = Quaternion.Euler(Quaternion.identity.eulerAngles.x, maxRotationsAngle,
                    Quaternion.identity.eulerAngles.z);
                if (dataManager.round == 0)
                {
                    lidTwistTutorial.SetActive(false);
                    arrows.SetActive(false);
                }
                jarClosed = true;
            }
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team20
{
    public class MikeSplitterComponent : MonoBehaviour
    {
        public GameObject leftMike;
        public GameObject rightMike;
        public DoorPlaceComponent doorPlace;
        bool needToSplit = false;
        public Vector3 splitVector;
        public float duration = 2f;
        float splitTimer = 0f;
        Vector3 originalLeft;
        Vector3 originalRight;
        DoorComponent placedDoor;
        

        // Start is called before the first frame update
        void Start()
        {
            originalLeft = leftMike.transform.position;
            originalRight = rightMike.transform.position;
        }

        void LerpMike(Transform halfTransform, Vector3 origin, Vector3 dstPos)
        {
            halfTransform.position = Vector3.Lerp(origin, dstPos, splitTimer / duration);
        }

        // Update is called once per frame
        void Update()
        {
            bool enabled = doorPlace.placedDoor;
            leftMike.GetComponent<SpriteRenderer>().enabled = enabled;
            rightMike.GetComponent<SpriteRenderer>().enabled = enabled;

            if (doorPlace.placedDoor && !placedDoor)
            {
                placedDoor = doorPlace.placedDoor;
                placedDoor.GetComponent<SpriteRenderer>().enabled = false;
            }

            if (placedDoor && !doorPlace.placedDoor)
            {
                placedDoor.GetComponent<SpriteRenderer>().enabled = true;
                placedDoor = null;
            }

            if(enabled)
            {
                splitTimer += Time.deltaTime;
                splitTimer = Mathf.Min(splitTimer, duration);
            }


            if (needToSplit && enabled)
            {
                LerpMike(leftMike.transform, originalLeft, originalLeft - splitVector);
                LerpMike(rightMike.transform, originalRight, originalRight + splitVector);
            }
            else
            {
                LerpMike(leftMike.transform, originalLeft - splitVector, originalLeft);
                LerpMike(rightMike.transform, originalRight + splitVector, originalRight);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.GetComponent<PlayerComponent>())
                return;

            needToSplit = true;
            if (splitTimer < 0f)
                splitTimer = duration - splitTimer;
            else
                splitTimer = 0f;
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (!collision.GetComponent<PlayerComponent>())
                return;

            needToSplit = false;
            if (splitTimer < 0f)
                splitTimer = duration - splitTimer;
            else
                splitTimer = 0f;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team20
{
    public class UpdateDirectionArrowComponent : MonoBehaviour
    {
        SpriteRenderer arrowRenderer;
        SpriteRenderer doorRenderer;
        DoorComponent door;
        // Start is called before the first frame update
        void Start()
        {
            door = GetComponentInParent<DoorComponent>();
            doorRenderer = GetComponentInParent<SpriteRenderer>();
            arrowRenderer = GetComponent<SpriteRenderer>();
        }

        // Update is called once per frame
        void Update()
        {
            var doorDirection = door.ArrowDirection();
            transform.right = doorDirection;
            arrowRenderer.flipX = Mathf.Abs( doorDirection.y) > 0.99;


            //arrowRenderer.enabled = door.Connected();
            // = door.Flipped();
        }
    }
}

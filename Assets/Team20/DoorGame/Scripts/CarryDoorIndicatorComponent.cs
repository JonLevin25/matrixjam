using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team20
{
    public class CarryDoorIndicatorComponent : MonoBehaviour
    {
        SpriteRenderer renderer;
        DoorConnectorComponent doorConnector;

        // Start is called before the first frame update
        void Start()
        {
            renderer = GetComponent<SpriteRenderer>();
            doorConnector = GetComponentInParent<DoorConnectorComponent>();
        }

        // Update is called once per frame
        void Update()
        {
            renderer.enabled = doorConnector.HasDoor();
        }
    }
}

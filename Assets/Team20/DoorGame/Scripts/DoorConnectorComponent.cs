using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team20
{
    public class DoorConnectorComponent : MonoBehaviour
    {
        PlayerComponent _playerComponent;
        Connection connection = new Connection();
        // Start is called before the first frame update
        void Start()
        {
            _playerComponent = gameObject.GetComponent<PlayerComponent>();
        }

        DoorPlaceComponent ClosestDoorPlace()
        {
            return this.GetComponent<CurrentDoorPlaceComponent>().GetDoorPlace();
        }

        DoorComponent ClosestDoor()
        {
            var doorPlace = ClosestDoorPlace();
            if (!doorPlace)
                return null;

            return doorPlace.placedDoor;
        }

        T FindClosest<T>() where T: UnityEngine.MonoBehaviour
        {
            var minDist = float.MaxValue;
            T bestDoor = null;
            foreach (var door in Object.FindObjectsOfType<T>())
            {
                if (!door.enabled)
                    continue;
                var dist = Vector3.Distance(door.transform.position, this.transform.position);
                if (dist < minDist)
                {
                    minDist = dist;
                    bestDoor = door;
                }
            }

            return bestDoor;
        }

        void ConnectDoor()
        {
            var door = ClosestDoor();
            if (door != null)
            {
                if (door.Connected())
                {
                    _playerComponent.PlayDetach();
                    door.Disconnect();
                }
                else
                {
                    _playerComponent.PlayDetach();
                    connection.AddDoor(door);
                }
            }
        }

        void FlipConnectionDirection()
        {
            var door = ClosestDoor();
            if (door)
            {
                door.FlipDirection();
                _playerComponent.PlayFlip();
                var warp = GetComponent<WarpComponent>();
                if(warp)
                {
                    warp.ResetIsRight();
                }
            }
                
        }

        DoorComponent currentDoor;

        public bool HasDoor() { return currentDoor; }

        void PickPlaceDoor()
        {
            var doorPlace = ClosestDoorPlace();
            if (!doorPlace)
                return;

            if(!currentDoor)
            {
                _playerComponent.PlayDetach();
                currentDoor = doorPlace.PickDoor();
            }
            else
            {
                if(!doorPlace.placedDoor)
                {
                    _playerComponent.PlayDetach();
                    doorPlace.PlaceDoor(currentDoor);
                    currentDoor = null;
                }
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyUp(KeyCode.J))
            {
                PickPlaceDoor();

            }

            if (Input.GetKeyUp(KeyCode.K))
            {
                ConnectDoor();

            }

            if (Input.GetKeyUp(KeyCode.L))
            {
                FlipConnectionDirection();
            }
        }
    }
}

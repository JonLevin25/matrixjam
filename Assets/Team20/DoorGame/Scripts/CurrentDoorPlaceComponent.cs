using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team20
{
    public class CurrentDoorPlaceComponent : MonoBehaviour
    {
        private HashSet<DoorPlaceComponent> doorPlaces = new HashSet<DoorPlaceComponent>();

        public void LeaveDoorPlace(DoorPlaceComponent place)
        {
            doorPlaces.Remove(place);
        }

        public void EnterDoorPlace(DoorPlaceComponent place)
        {
            doorPlaces.Add(place);
        }

        public DoorPlaceComponent GetDoorPlace()
        {
            DoorPlaceComponent result = null;
            float minDistSqr = float.MaxValue;
            foreach (var doorPlace in doorPlaces)
            {
                var sqrDist = Vector2.SqrMagnitude(doorPlace.transform.position - transform.position);
                if (sqrDist < minDistSqr)
                {
                    minDistSqr = sqrDist;
                    result = doorPlace;
                }
            }

            return result;
        }

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}

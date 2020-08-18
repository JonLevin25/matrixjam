using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team20
{
    public class WarpComponent : MonoBehaviour
    {
        DoorPlaceComponent currentDoorPlace;
        bool isRightToDoor;

        DoorComponent currentDoor
        {
            get
            {
                return currentDoorPlace ? currentDoorPlace.placedDoor : null;
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            if (!currentDoor)
                return;

            if (IsRightToDoor() == isRightToDoor)
                return;

            if (currentDoor.Connected())
            {
                Warp();
            }
            else
            {
                TryGoNextLevel();
            }
        }

        void TryGoNextLevel()
        {
            if (!GetComponent<PlayerComponent>())
                return;

            if (currentDoor.currentPlace.gameExit)
            {
                currentDoor.currentPlace.gameExit.EndLevel();
            }

            if (!currentDoor.currentPlace.goToNextLevel)
                return;

            SceneManagerComponent.instance.GoToNextLevel();
        }

        static void SetPosition2D(Transform dst, Vector2 src)
        {
            dst.position = new Vector3(src.x, src.y, dst.position.z);
        }

        void Warp()
        {
            var doorToWarp = currentDoor.ConnectedDoor();
            if (!doorToWarp.currentPlace)
                return;
            var doorToSelf = DoorToSelf();
            var angle = Vector2.Angle(currentDoor.Direction(), doorToWarp.Direction());

            var playerComponent = GetComponent<PlayerComponent>();
            var movementComponent = GetComponent<MovementComponent>();
            var enemyComponent = GetComponent<EnemyControllerComponent>();

            if(movementComponent)
            {
                var pos2D = (Vector2)doorToWarp.transform.position + new Vector2(doorToSelf.x, doorToSelf.y);
                SetPosition2D(transform, pos2D);
                var originalVelocity = new Vector2(movementComponent.velocity.x, movementComponent.velocity.y);
                movementComponent.velocity = Quaternion.AngleAxis(angle, Vector3.forward) * movementComponent.velocity;
                //this.transform.Translate(Time.deltaTime * movementComponent.velocity);

                var outDirection = doorToWarp.Direction();
                if(isRightToDoor)
                {
                    outDirection = -outDirection;
                }

                if (doorToWarp.Direction().y * currentDoor.Direction().y < -0.9)
                {
                    if(outDirection.y > 0.9)
                    {
                        movementComponent.velocity = new Vector2(0f, movementComponent.velocity.y);
                    }
                    movementComponent.velocity.x = originalVelocity.x;
                    movementComponent.transform.Translate(outDirection * 0.25f);
                }

                if(playerComponent)
                {
                    if (doorToWarp.Direction().x * currentDoor.Direction().x < -0.9)
                        playerComponent.resetHorizontal = true;
                }

                if(angle >= 170 && enemyComponent)
                {
                    enemyComponent.movementX *= -1f;
                }
            }

            currentDoorPlace = null;
        }

        Vector2 DoorToSelf()
        {
            Vector2 doorPosition = currentDoorPlace.transform.position;
            Vector2 selfPosition = transform.position;
            return selfPosition - doorPosition;
        }

        bool IsRightToDoor()
        {
            return Vector2.Dot(DoorToSelf(), currentDoor.Direction()) > 0;
        }

        public void OnEnterDoor(DoorPlaceComponent door)
        {
            currentDoorPlace = door;
            isRightToDoor = IsRightToDoor();
        }

        public void ResetIsRight()
        {
            if (!currentDoorPlace)
                return;
            isRightToDoor = IsRightToDoor();
        }

        public void OnExitDoor(DoorPlaceComponent door)
        {
            if (currentDoorPlace == door)
            {
                currentDoorPlace = null;
            }
        }
    }
}

using UnityEngine;

namespace MatrixJam.Team17
{
    public class NinjaRopeHook : Projectile
    {
        //[ReadOnly]
        public bool hookInFlight = true;

        void OnDrawGizmos()
        {
            if (Owner == null)
            {
                return;
            }

            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, Owner.transform.position);
        }

        void Expire()
        {
            // TODO: Should probably retract when max length is reached without attaching to anything.
            if (hookInFlight)
            {
                Destroy();
            }
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }
    }
}
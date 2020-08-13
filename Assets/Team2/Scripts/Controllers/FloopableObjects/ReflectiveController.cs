using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team2
{
    public class ReflectiveController : MonoBehaviour
    {
        public void ReflectLaser(Transform laser, ContactPoint2D contactPoint)
        {
            // reflect our old velocity off the contact point's normal vector
            Vector3 reflectedVelocity = Vector3.Reflect(laser.right, contactPoint.normal);

            // assign the reflected velocity back to the rigidbody
            // rigidbody.velocity = reflectedVelocity;

            // rotate the object by the same ammount we changed its velocity
            Quaternion rotation = Quaternion.FromToRotation(laser.right, reflectedVelocity);
            laser.rotation = rotation * laser.rotation;
        }
    }
}

using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team17
{
    [RequireComponent(typeof(Collider))]
    public class Sensor : MonoBehaviour
    {
        public bool IsDetected => detected.Count > 0;

        public List<Collider> detected = new List<Collider>();

        new Collider collider;
        public Collider Collider => collider;

        void Awake()
        {
            collider = GetComponent<Collider>();
        }

        void OnEnable()
        {
            detected.Clear();
        }

        void OnCollisionEnter(Collision collision)
        {
            detected.Add(collision.collider);
        }

        void OnCollisionExit(Collision collision)
        {
            detected.Remove(collision.collider);
        }

        void OnTriggerEnter(Collider other)
        {
            detected.Add(other);
        }

        void OnTriggerExit(Collider other)
        {
            detected.Remove(other);
        }
    }
}

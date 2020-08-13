using UnityEngine;

namespace MatrixJam.Team25.Scripts.Jar
{
    public class LidTrigger : MonoBehaviour
    {
        public float proximityThreshold = 0.5f;
        public bool lidOn = false;
        private float distance;

        private void OnTriggerStay(Collider other)
        {
            if (!other.CompareTag("Tag0")) return;
            distance = Vector3.Distance(transform.position, other.transform.parent.position);
            if (distance > proximityThreshold) return;
            other.transform.parent.localPosition = new Vector3(0, transform.localPosition.y, 0);
            GetComponent<Collider>().enabled = false;
            lidOn = true;
        }
    }
}
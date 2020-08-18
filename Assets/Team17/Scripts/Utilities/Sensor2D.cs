using UnityEngine;

namespace MatrixJam.Team17
{
    [RequireComponent(typeof(Collider2D))]
    public class Sensor2D : MonoBehaviour
    {
        public LayerMask layers;

        //[ReadOnly]
        [SerializeField]
        private bool isDetected;
        public bool IsDetected => isDetected;

        new Collider2D collider;
        public Collider2D Collider => collider;

        public bool IsTouchingLayers(LayerMask touchLayers)
        {
            return collider.IsTouchingLayers(touchLayers);
        }

        void Awake()
        {
            collider = GetComponent<Collider2D>();
        }

        void OnEnable()
        {
            isDetected = false;
        }

        void FixedUpdate()
        {
            isDetected = collider.IsTouchingLayers(layers);
        }
    }
}

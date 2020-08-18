using UnityEngine;

namespace MatrixJam.Team17
{
    //[RequireComponent(typeof(PlayerInputListener))]
    [RequireComponent(typeof(Rigidbody))]
    public class Player : MonoBehaviour
    {
        public PlayerConfig playerConfig;

        [Space]
        
        public Sensor head;
        public Sensor feet;

        Rigidbody body;
        float lastGroundedTime;

        public PlayerInputData Input => Game.Input; // inputListener.data;
        public Rigidbody Body => body;

        public bool IsGrounded => feet.IsDetected;
        public bool IsGroundedFuzzy => (Time.time - lastGroundedTime) < playerConfig.groundedFuzzy;
        public bool IsJumpingFuzzy => Input.jumpReady && (Time.time - Input.jumpTime) < playerConfig.jumpFuzzy;
        public bool IsHunched => head.IsDetected;
        //public bool IsCrouching => playerConfig.crouchEnabled && (Input.crouch || (IsHunched && IsGrounded));

        void Awake()
        {
            //inputListener = GetComponent<PlayerInputListener>();
            body = GetComponent<Rigidbody>();
        }

        void OnEnable()
        {
            Input.Reset();

            //head.layers = playerConfig.groundLayers;
            //feet.layers = playerConfig.groundLayers;
        }

        void Update()
        {
            if (IsGrounded)
                lastGroundedTime = Time.time;
        }
    }
}

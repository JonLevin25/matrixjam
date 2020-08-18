using UnityEngine;

namespace MatrixJam.Team17
{
    [CreateAssetMenu(menuName = "TheFlyingDragons/PlayerConfig")]
    public class PlayerConfig : ScriptableObject
    {
        public LayerMask groundLayers;
        public float groundedFuzzy = 0.2f;
        public bool moveEnabled = true;
        public Vector2 moveSpeed = new Vector2(1f, 0);
        public float movementSmoothing = 0.05f;
        public bool airControl = true;
        [Tooltip("Air speed scalar")]
        public Vector2 airSpeed = new Vector2(0.36f, 0);
        public bool crouchEnabled = true;
        [Tooltip("Crouch speed scalar")]
        public Vector2 crouchSpeed = new Vector2(0.36f, 0);
        public bool jumpEnabled = true;
        public bool jumpForward = false;
        public float jumpFuzzy = 0.2f;
        public float jumpCooldown = 0.5f;
        public float jumpForce = 10f;
    }
}

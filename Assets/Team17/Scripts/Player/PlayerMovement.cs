using UnityEngine;

namespace MatrixJam.Team17
{
    [RequireComponent(typeof(Player))]
    public class PlayerMovement : MonoBehaviour
    {
        Player player;
        Vector3 velocity;
		float lastJump;

		public PlayerConfig PlayerConfig => player.playerConfig;
		public PlayerInputData Input => player.Input;
		public Rigidbody Body => player.Body;
		
		void Awake()
        {
            player = GetComponent<Player>();
		}

		void FixedUpdate()
        {
			if (player.IsGrounded || PlayerConfig.airControl)
			{
				float horizontalMove = Input.move.x * PlayerConfig.moveSpeed.x * Time.fixedDeltaTime;
				float verticalMove = Input.move.y * PlayerConfig.moveSpeed.y * Time.fixedDeltaTime;

				if(!player.IsGrounded) // The player is in the air and airControl is enabled
				{
					horizontalMove *= PlayerConfig.airSpeed.x;
					verticalMove *= PlayerConfig.airSpeed.y;
				}

				/*if (player.IsCrouching)
				{
					horizontalMove *= PlayerConfig.crouchSpeed.x;
					player.head.Collider.isTrigger = true;
				}
				else // Not crouching
				{
					player.head.Collider.isTrigger = false;
				}*/

				if (PlayerConfig.moveEnabled)
				{
					Vector3 targetVelocity = transform.forward * verticalMove + transform.right * horizontalMove;
					
					Body.velocity = Vector3.SmoothDamp(Body.velocity, targetVelocity, ref velocity, PlayerConfig.movementSmoothing);
				}
			}

			bool groundedFuzzy = player.IsGrounded || player.IsGroundedFuzzy;
			bool isJumping = Input.jump || player.IsJumpingFuzzy;
			if (PlayerConfig.jumpEnabled && isJumping && groundedFuzzy)
			{
				if ((Time.time - lastJump) > PlayerConfig.jumpCooldown)
				{
					lastJump = Time.time;
					Body.AddForce((PlayerConfig.jumpForward ? transform.forward : transform.up) * PlayerConfig.jumpForce);
				}
				Input.jumpReady = false;
				Input.jump = false;
			}
		}

	}
}

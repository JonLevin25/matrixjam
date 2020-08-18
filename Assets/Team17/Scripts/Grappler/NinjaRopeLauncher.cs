using UnityEngine;

namespace MatrixJam.Team17
{
    public class NinjaRopeLauncher : Grappler
    {   
        public NinjaRopeHookConfig hookConfig;
        public Transform hookSpawnPoint;
        
        [Space]
        
        //[ReadOnly]
        public float ropeLength;
        
        //[ReadOnly]
        public bool ropeActive;

        MeshRenderer ropeMeshRenderer;
        RaycastHit2D hookRaycast;
        bool isTriggerDown;

        public override void ChangeLength(float delta)
        {
            //if (IsRopeActive)
            {
                ropeLength = 0f;// rope.restLength;
                ropeLength += delta * grapplerConfig.changeLengthSpeed * Time.deltaTime;
                ropeLength = Mathf.Clamp(ropeLength, hookConfig.ropeMinLength, hookConfig.ropeMaxLength);
                //cursor.ChangeLength(ropeLength);
            }
        }

        public override void TriggerDown()
        {
            if (isTriggerDown)
            {
                return;
            }
            
            isTriggerDown = true;
            
            if (ropeActive)
            {
                Detach();
                return;
            }
            
            if (Raycast(Mouse.GetWorldPosition()))
            {
                //Debug.Log("Rope raycast hit " + hookRaycast.collider.name);
                //ObiColliderBase targetObiCollider = hookRaycast.collider.GetComponent<ObiColliderBase>();
                //if (targetObiCollider != null)
                {
                   // LaunchHook(hookRaycast.point, targetObiCollider);
                }
            }
            else
            {
                Debug.Log("Rope miss");
            }
        }

        public override void TriggerUp()
        {
            isTriggerDown = false;
        }

        bool Raycast(Vector3 direction)
        {
            hookRaycast = Physics2D.Raycast(hookSpawnPoint.position, direction, 10000f/*ownerToRing.magnitude*/, hookConfig.raycastLayer, -1000f);//, QueryTriggerInteraction.Collide);
            return hookRaycast.collider != null;
        }

        public void LaunchHook(Vector3 targetPosition, Collider targetCollider)
        {
            //Debug.Log("Launching hook at: " + targetCollider);
            //StartCoroutine(AttachHook(targetPosition, targetCollider));

            // Add to the solver and show
            ropeActive = true;
            //ropeMeshRenderer.enabled = true;
        }

        public override void Detach()
        {
            //Debug.Log("Detach Hook");
            //StartCoroutine(ClearHook());
            //if (rope != null)
            //    GameObject.Destroy(rope.gameObject);
            ropeActive = false;
        }

        /*public override Projectile SpawnProjectile()
        {
            Projectile projectile = base.SpawnProjectile();
            NinjaRopeHook hook = (NinjaRopeHook)projectile;
        
            return projectile;
        }*/

    }
}

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace MatrixJam.Team11
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] Canvas gameOverCanvas;
        [SerializeField] float movementSpeed;
        [SerializeField] Transform handTransform;
        [SerializeField] Slider airMeter;
        [SerializeField] float maxAir = 100;
        [SerializeField] float airDrainRate = 1;
        /* #scan
        [SerializeField] float pickUpRadius = 0.3f;
        [SerializeField] float interactRadius = 0.1f;
        */
        [SerializeField] float airLostOnHurt = 10f;
        [SerializeField] Collider2D bubbleCollider;
        [SerializeField] Vector2 liftingColliderOffset;
        [SerializeField] Vector2 liftingColliderSize;
        //public Collider2D regularCollider;
        CapsuleCollider2D _collider;
        // [SerializeField] Collider2D liftingObjectCollider;
        /*  #scan
        LiftableObject[] liftableObjectsInScene;
        InteractableItem[] interactableItemsInScene;
        InteractableItem currentButton;
        */
        GameObject objectInHands;
        //Rigidbody2D objectInHandsRB;
        Animator _animator;
        ParticleSystem _particleSystem;

        Rigidbody2D _rigidbody;
        float horizontalInput;
        float verticalInput;
        float currentAir;
        public bool hasObjectInHands = false;
        public bool isDead = false;
        public bool wasHurt = false;
        public bool canMove = false;
     //   public bool canMoveVertically;

        Vector2 defaultColliderOffset;
        Vector2 defaultColliderSize;

        public GameObject FocusedObject { get; protected set; }
        /* #outline
        private Outlineotron _focusedObjectOutline;
        private bool _isFocusOutlined;
        */

        // Start is called before the first frame update
        void Start()
        {
            _particleSystem = GetComponentInChildren<ParticleSystem>();
            _particleSystem.Stop();
            gameOverCanvas.gameObject.SetActive(false);
            _animator = GetComponent<Animator>();
            currentAir = maxAir/2;
            airMeter.value = currentAir;
            _collider = GetComponent<CapsuleCollider2D>();
            defaultColliderOffset = _collider.offset;
            defaultColliderSize = _collider.size;
                
           // liftingObjectCollider.enabled = false;

            /*  #scan
            liftableObjectsInScene = FindObjectsOfType<LiftableObject>();
            interactableItemsInScene = FindObjectsOfType<InteractableItem>();*/
            _rigidbody = GetComponent<Rigidbody2D>();
            FocusedObject = null;
            /* #outline
            _focusedObjectOutline = null;
            _isFocusOutlined = false;
            */
        }

        private void Awake()
        {
            canMove = false;
            Invoke("DisableMovementLock", 1f); // stops player from moving during fade out so he won't miss the bubble tutorial message.
        }

        // Update is called once per frame
        void Update()
        {
            if(isDead)
            {
                return;
            }

            if(!_collider.IsTouching(bubbleCollider))
            {
                currentAir -= airDrainRate * Time.deltaTime;
                if(currentAir <=0)
                {
                    StartCoroutine(Die());
                }
                else
                {
                    airMeter.value = currentAir;
                }
            }
            else
            {
                currentAir += airDrainRate * 50 * Time.deltaTime;
                if (currentAir >= maxAir)
                {
                    currentAir = maxAir;
                }
                airMeter.value = currentAir;
            }

            if(canMove)
            {
                horizontalInput = Input.GetAxis("Horizontal");
                verticalInput = Input.GetAxis("Vertical");
                /*if (canMoveVertically)
                {
                    verticalInput = Input.GetAxis("Vertical");

                }*/
        }
            else
            {
                horizontalInput = 0;
                verticalInput = 0;
            }
           

            if(horizontalInput != 0)
            {
                transform.localScale = new Vector3(Mathf.Sign(horizontalInput), 1, 1);

            }
            _rigidbody.velocity = new Vector2(horizontalInput * movementSpeed, verticalInput * movementSpeed);

            if(hasObjectInHands)
            {
                CarryObject();
            }
            /*else if (Input.GetKey(KeyCode.Space) && !hasObjectInHands)
            {
                TryPickUpItem();
            }*/
            else if ((this.FocusedObject != null) && Input.GetKey(KeyCode.Space))
            {
                if (this.FocusedObject.TryGetComponent<InteractableItem>(out InteractableItem interactable))
                {
                    if (interactable.canInteract)
                    {
                        interactable.Interact();
                    }
                }
                else if (this.FocusedObject.TryGetComponent<LiftableObject>(out LiftableObject liftable))
                {
                    this.TryPickUpItem(liftable);
                }
                else
                {
                    Debug.LogError($"{name}: the focued object is rrelevant! ({FocusedObject})");
                }
                this.SetFocusOn(null);
                /* #outline
                this.SetFocuedOutline(false);
                */
            }

            /* #scan
            if(Input.GetKeyDown(KeyCode.Space) && !hasObjectInHands)
            {
                foreach (var button in interactableItemsInScene)
                {
                    if (Vector2.Distance(transform.position, button.transform.position) < interactRadius || _collider.IsTouching(button.GetComponent<Collider2D>()))
                    {
                        currentButton = button;
                        currentButton.Interact(); // TODO: <this
                        break;
                    }
                }
                
                currentButton = null;
            }
            */
            _animator.SetFloat("horizontalInput", Mathf.Abs(horizontalInput));
            _animator.SetFloat("verticalInput", Mathf.Abs(verticalInput));

            if((Mathf.Abs(horizontalInput) > 0.1f || Mathf.Abs(verticalInput) > 0.1f) && !_particleSystem.isPlaying)
            {
                _particleSystem.Play();
            }
            else if(_particleSystem.isPlaying && !((Mathf.Abs(horizontalInput) > 0.1f || Mathf.Abs(verticalInput) > 0.1f)))
            {
                _particleSystem.Stop();
               
            }
        }

        /* #scan
        private void OnCollisionStay2D(Collision2D collision)
        {
            if(collision.gameObject.GetComponent<LiftableObject>() && Input.GetKey(KeyCode.Space) && !hasObjectInHands)
            {
                TryPickUpItem(collision.gameObject.GetComponent<LiftableObject>()); // TODO: <this
            }
        }
        */

        private void SetOutline(GameObject obj, bool isEnabled)
        {
            if ((obj != null) &&
                obj.TryGetComponent<Outlineotron>(out Outlineotron outline))
            {
                outline.enabled = isEnabled;
            }
        }

        private void SetFocusOn(GameObject obj)
        {
            this.SetOutline(this.FocusedObject, false);

            this.FocusedObject = obj;

            this.SetOutline(this.FocusedObject, true);

            if (obj != null)
            {
                Debug.DrawLine(this.transform.position, obj.transform.position, Color.white, 1f);
            }
        }

        private bool IsRelevant(GameObject obj)
        {
            InteractableItem interactable = obj.GetComponent<InteractableItem>();
            if (interactable != null)
            {
                return interactable.canInteract;
            }
            else
            {
                return (obj.GetComponent<LiftableObject>() != null);
            }
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (this.hasObjectInHands || !this.IsRelevant(collision.gameObject))
            {
                return;
            }
            Debug.DrawLine(this.transform.position, collision.transform.position, Color.red, 1f);

            if (this.FocusedObject == null)
            {
                this.SetFocusOn(collision.gameObject);
            }
            else if (!Object.ReferenceEquals(this.FocusedObject, collision.gameObject))
            {
                float currentFocusDistance = Vector2.Distance(
                    this.transform.position,
                    this.FocusedObject.transform.position
                    );
                float potantialFocusDistance = Vector2.Distance(
                    this.transform.position,
                    collision.transform.position
                    );

                //Debug.Log($"{this.name}: checking distances: {potantialFocusDistance} ?<? {currentFocusDistance}");

                if (potantialFocusDistance < currentFocusDistance)
                {
                    this.SetFocusOn(collision.gameObject);
                }
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (Object.ReferenceEquals(collision.gameObject, this.FocusedObject))
            {
                this.SetFocusOn(null);
            }
        }

        private void OnDrawGizmos()
        {
            if (this.FocusedObject != null)
            {
                Gizmos.DrawWireCube(this.FocusedObject.transform.position, new Vector3(1f, 1f, 0));
            }
        }

        void TryPickUpItem(LiftableObject item)
        {
            objectInHands = item.gameObject;
           // objectInHandsRB = item.GetComponent<Rigidbody2D>();
            objectInHands.GetComponent<LiftableObject>().isLifted = true;
            SFXPlayer.instance.PlaySFX(SFXPlayer.instance.pickUpSFX, 0.8f);

            hasObjectInHands = true;
            _collider.size = liftingColliderSize;
            _collider.offset = liftingColliderOffset;
            _animator.SetBool("hasObjectInHands", true);



            Physics2D.IgnoreCollision(_collider, objectInHands.GetComponent<Collider2D>(), true);
            /* #scan
            /*foreach (var item in liftableObjectsInScene)
            {
                
                /*if (Vector2.Distance(item.transform.position, transform.position) < pickUpRadius || _collider.IsTouching(item.GetComponent<Collider2D>()))   // Picks up Item
                {
                    objectInHands = item.gameObject;
                    objectInHandsRB = item.GetComponent<Rigidbody2D>();
                    objectInHands.GetComponent<LiftableObject>().isLifted = true;
                    //TO_IDO: add pick up SFX.
                    SFXPlayer.instance.PlaySFX(SFXPlayer.instance.pickUpSFX,0.8f);

                    hasObjectInHands = true;
                    // liftingObjectCollider.enabled = true;
                    _collider.size = liftingColliderSize;
                    _collider.offset = liftingColliderOffset;
                    _animator.SetBool("hasObjectInHands", true);



                    Physics2D.IgnoreCollision(_collider, objectInHands.GetComponent<Collider2D>(), true);
                   // Physics2D.IgnoreCollision(liftingObjectCollider, objectInHands.GetComponent<Collider2D>(), true);

                }
            }*/
        //*/
    }




    void CarryObject()
        {
            if(!Input.GetKey(KeyCode.Space))
            {
               // liftingObjectCollider.enabled = false;           // Drop item
                objectInHands.GetComponent<LiftableObject>().isLifted = false;
                Physics2D.IgnoreCollision(_collider, objectInHands.GetComponent<Collider2D>(), false);
                _collider.offset = defaultColliderOffset;
                _collider.size = defaultColliderSize;
               // Physics2D.IgnoreCollision(liftingObjectCollider, objectInHands.GetComponent<Collider2D>(), false);
                _animator.SetBool("hasObjectInHands", false);

                objectInHands = null;
               // objectInHandsRB = null;
                hasObjectInHands = false;
            }
            else
            {
                objectInHands.transform.position = handTransform.position;
                if (horizontalInput != 0)
                {
                  objectInHands.transform.localScale = new Vector3(Mathf.Sign(horizontalInput), 1, 1);


                }
            }

        }

        IEnumerator Die()
        {
            SFXPlayer.instance.PlaySFX(SFXPlayer.instance.drownSFX);
            _rigidbody.gravityScale = -0.1f;
            _animator.SetFloat("verticalInput", 0);
            _animator.SetFloat("horizontalInput", 0);
            _animator.SetBool("hasObjectInHands", false);
            _animator.SetBool("playerHurt", false);

            _animator.SetBool("playerDrown", true);
            isDead = true;

           // liftingObjectCollider.enabled = false;
            airMeter.gameObject.SetActive(false);
            this.enabled = false;
           
            yield return new WaitForSeconds(1f);
            gameOverCanvas.gameObject.SetActive(true);
            _particleSystem.Stop();

        }

        void DisableMovementLock()
        {
            canMove = true;
        }


        public void HurtPlayer()
        {
            
            _animator.SetBool("playerHurt", true);
            wasHurt = true;
            Invoke("CanBeHurtAgain", 0.4f);
            currentAir -= airLostOnHurt;
            airMeter.value = currentAir;
            airMeter.GetComponent<AirMeter>().PlayerHit();
            
            if (currentAir <= 0)
            {
                StartCoroutine(Die());
            }
           
        }

        public void HurtAnimationFinished() // called by animation event.
        {
            _animator.SetBool("playerHurt", false);
        }
        void CanBeHurtAgain()
        {
            wasHurt = false;
            _animator.SetBool("playerHurt", false);
        }


        void PlaySwimSound() // called by animation event
        {
            SFXPlayer.instance.PlaySwimSFX();
        }




    }
}

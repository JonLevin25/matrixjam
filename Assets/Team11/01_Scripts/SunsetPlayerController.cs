using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team11
{
    public class SunsetPlayerController : MonoBehaviour
    {
        /* #scene 
        [SerializeField] GameObject normalLevel;
        [SerializeField] GameObject hardLevel;
        [SerializeField] GameObject sunsetScene;
        */

        [SerializeField] float movementSpeed;
        [SerializeField] float timeUntilLevelSpawn;
        Rigidbody2D _rigidbody;
        float horizontalInput;
        Animator _animator;
        public bool canMove = false;

        // Start is called before the first frame update
        void Start()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
            FindObjectOfType<MusicPlayer>().PlayStartSequence();
        }

        // Update is called once per frame
        void Update()
        {
            if (!canMove)
            {
                return;
            }
            horizontalInput = Input.GetAxis("Horizontal");
            if (horizontalInput != 0)
            {
                transform.localScale = new Vector3(Mathf.Sign(horizontalInput), 1, 1);

            }
            _rigidbody.velocity = new Vector2(horizontalInput * movementSpeed, 0);
            _animator.SetFloat("horizontalInput", Mathf.Abs(horizontalInput));
        }

        void PlaySwimSound() // called by animation event
        {
            SFXPlayer.instance.PlaySwimSFX();
        }

        /* #scene
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.tag.Equals("Tag0"))
            {
                var fader = FindObjectOfType<BlackoutFader>();
                fader.StartCoroutine(fader.FadeIn(1f, timeUntilLevelSpawn, true));
                Destroy(collision.gameObject);
                
                Invoke("StartNormalLevel", timeUntilLevelSpawn + 1f);
            }
        }

        void StartNormalLevel()
        {
            sunsetScene.SetActive(false);
            normalLevel.SetActive(true);
            Destroy(gameObject);
        }
        */
    }
}

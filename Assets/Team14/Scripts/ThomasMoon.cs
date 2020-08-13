using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MatrixJam.Team14
{
    [Serializable]
    public class AnimateThomasAfter
    {
        public bool resetEveryRestart;
        public float secsDelay;

        private float _currSecs;

        public void OnUpdate()
        {
            _currSecs += Time.deltaTime;
        }


        public void OnRestart()
        {
            if (resetEveryRestart) _currSecs = 0f;
        }

        public bool ShouldAnimate => _currSecs >= secsDelay;
    }
    
    public class ThomasMoon : MonoBehaviour
    {
        public enum LookAt
        {
            None,
            Cursor,
            NextObstacleOrTarget,
            Target
        }

        [Header("Config")]
        [SerializeField] private bool eyebrowsAnim = true;
        [SerializeField] private bool weirdFaceAnim = true;
        [SerializeField] private bool happyAnim = true;
        [SerializeField] private Transform lookaheadPos; // Used for eyes look

        [Space]
        [Range(0.1f, 1f)]
        [SerializeField] private float lookLerpSpeed = 0.6f;
        [SerializeField] private AnimateThomasAfter animationDelay; // Hold off animations so player isnt distracted
        [SerializeField] private Vector2 eyebrowsRandomDelta;
        [SerializeField] private Camera cam;
        [SerializeField] private float localEyeRadius;
        [SerializeField] private float scalePerSec;

        [SerializeField] private Animator anim;
        [SerializeField] private SpriteRenderer faceSprite;
        [SerializeField] private SpriteRenderer leftEye;
        [SerializeField] private SpriteRenderer rightEye;
        [SerializeField] private SpriteRenderer leftEyebrow;
        [SerializeField] private SpriteRenderer rightEyebrow;
        [SerializeField] private LookAt lookMode;
        [SerializeField] private Transform target;

        public float Z => faceSprite.transform.position.z;

        private bool _ignoreEyes;
        private const string TrigWeird = "weird";
        private const string TrigEyebrows = "eyebrows";
        private const string TrigHappy = "happy";
        private const string TrigReset = "reset";
        private float _timeSinceRestart = 0f;

        private IEnumerable<string> AllTriggers
        {
            get
            {
                yield return TrigWeird;
                yield return TrigEyebrows;
                yield return TrigHappy;
                yield return TrigReset;
            }
        }

        public LookAt LookMode
        {
            get => lookMode;
            set => lookMode = value;
        }

        public Transform Target
        {
            get => target;
            set => target = value;
        }

        private Vector3? LookAtPos
        {
            get
            {
                if (_ignoreEyes) return null;
                
                switch (LookMode)
                {
                    case LookAt.None:
                        return null;
                    case LookAt.Cursor:
                        var screenPos = Input.mousePosition;
                        screenPos.z = Z;
                        return cam.ScreenToWorldPoint(screenPos);
                    case LookAt.NextObstacleOrTarget:
                        return GetPOsNextObstacleOrTarget(); 
                    case LookAt.Target:
                        return Target.position;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private Vector3 GetPOsNextObstacleOrTarget()
        {
            var pos = GetReferencePos();
            var nextObstacle = Obstacle.GetNextObstacle(pos);
            if (!nextObstacle) return target.position;
            
            var nextObstaclePos = nextObstacle.transform.position;
            var targetPos = target.position;

            var isTargetCloser = Vector3.Distance(pos, targetPos) 
                                 < Vector3.Distance(pos, nextObstaclePos);
            
            return isTargetCloser ? targetPos : nextObstaclePos;
        }

        private Vector3 GetReferencePos()
        {
            if (lookaheadPos != null) return lookaheadPos.position;
            return transform.position;
        }

        private Vector3 startScale;

        private IEnumerator Start()
        {
            GameManager.ResetEvent += OnRestart;
            GameManager.GameFinishedEvent += OnGameFinish;
            
            if (eyebrowsAnim)
            {
                startScale = transform.localScale;
                StartCoroutine(RandomEyebrows(eyebrowsRandomDelta[0], eyebrowsRandomDelta[1]));
                yield return new WaitForSeconds(1);
            }
            
            // HappyAnim(3f);
        }

        private void OnDestroy()
        {
            GameManager.ResetEvent -= OnRestart;
            GameManager.GameFinishedEvent -= OnGameFinish;
        }

        private void Update()
        {
            animationDelay.OnUpdate();
            transform.localScale += Vector3.one * Time.deltaTime * scalePerSec;
            HandleEyesLookAt();
        }

        private void OnGameFinish(bool obj)
        {
            HappyAnim(999);
        }

        public void WeirdAnim(float time)
        {
            if (!weirdFaceAnim) return;
            if (!animationDelay.ShouldAnimate) return;

            StartCoroutine(WierdRoutine());
            IEnumerator WierdRoutine()
            {
                StartAnim(TrigWeird, false);
                yield return new WaitForSeconds(time);
                EndAnim(TrigWeird);
            }
        }

        public void HappyAnim(float time)
        {
            if (!happyAnim) return;
            if (!animationDelay.ShouldAnimate) return;

            StartCoroutine(HappyRoutine());
            IEnumerator HappyRoutine()
            {
                StartAnim(TrigHappy, false);
                yield return new WaitForSeconds(time);
                EndAnim(TrigHappy);
            }
        }

        public void EyebrowsAnim()
        {
            if (!eyebrowsAnim) return;
            if (!animationDelay.ShouldAnimate) return;

            const float dur1 = 0.4f;
            const float delay = 1f;
            const float dur2 = 0.6f;

            var oldLeft = leftEye.transform.localPosition;
            var oldRight = rightEye.transform.localPosition;
            var leftTween1 = leftEye.transform.DOLocalMove(Vector3.zero, dur1);
            var rightTween1 = rightEye.transform.DOLocalMove(Vector3.zero, dur1);
            
            var leftTween2 = leftEye.transform.DOLocalMove(oldLeft, dur2);
            var rightTween2 = rightEye.transform.DOLocalMove(oldRight, dur2);
            

            var seq = DOTween.Sequence()
                .Append(leftTween1)
                .Join(rightTween1)
                .AppendInterval(delay)
                .Append(leftTween2)
                .Join(rightTween2);
            
            seq.onComplete = () => EndAnim(TrigEyebrows);

            Debug.Log("EYEBROWS!");
            StartAnim(TrigEyebrows, true);
            seq.Play();
        }

        private void OnRestart()
        {
            animationDelay.OnRestart();
            StopAllCoroutines();

            ResetAnimations();
        }

        private void ResetAnimations()
        {
            foreach (var trig in AllTriggers)
                anim.SetBool(trig, false);

            anim.SetTrigger(TrigReset);
        }

        private void StartAnim(string trig, bool ignoreEyes)
        {
            _ignoreEyes = ignoreEyes;
            anim.SetBool(trig, true);
        }

        private void EndAnim(string trig)
        {
            _ignoreEyes = false;
            anim.SetBool(trig, false);
        }

        private IEnumerator RandomEyebrows(float minTime, float maxTime, bool startImmediate = false)
         {
             if (startImmediate) EyebrowsAnim();
             while (true)
             {
                 var randTime = Random.Range(minTime, maxTime); 
                 yield return new WaitForSeconds(randTime);
                 EyebrowsAnim();
             }
         }

        private Vector3 _refPos;
        private Vector3 _localLookAt;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(_refPos, 0.3f);
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(_localLookAt, 0.3f);
        }

        private bool HandleEyesLookAt()
        {
            var lookAtNullable = LookAtPos;

            if (!lookAtNullable.HasValue) return false;
            var lookAt = lookAtNullable.Value;

            
            if (lookaheadPos)
            {
                var localLookahead = lookaheadPos.InverseTransformPoint(lookAt);
                lookAt = transform.TransformPoint(localLookahead);
                _localLookAt = lookAt;
            }
            
            // _refPos = refPos;
            // var localLookAt = lookAt - refPos + transform.position;
            // _localLookAt = localLookAt;
            
            
            EyeLookAt(leftEye, lookAt);
            EyeLookAt(rightEye, lookAt);
            
            return true;
        }

        public void RestartSize()
        {
            transform.localScale = startScale;
        }

        private void EyeLookAt(SpriteRenderer eyeSprite, Vector3 target)
        {
            var delta = transform.InverseTransformDirection(target - eyeSprite.transform.position);

            var oldPos = eyeSprite.transform.localPosition;
            var targetPos = delta.normalized * localEyeRadius;

            eyeSprite.transform.localPosition = Vector3.Lerp(oldPos, targetPos, lookLerpSpeed);
        }
    }
}

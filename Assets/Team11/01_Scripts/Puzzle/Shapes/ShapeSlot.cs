using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MatrixJam.Team11.Puzzle
{
    public class ShapeSlot : MonoBehaviour
    {
        public static readonly int ANIM_TOGGLE = Animator.StringToHash("value");

        public enum CodeShape : int
        {
            Square = 1,
            Triangle = 2,
            Circle = 3,
        }
        public static readonly int SHAPES_AMOUNT = System.Enum.GetValues(typeof(CodeShape)).Length;

        public static CodeShape CalcNextShape(CodeShape current)
        {
            return (CodeShape)(1 + ((int)current % SHAPES_AMOUNT));
        }

        [SerializeField] protected CodeShape _value = CodeShape.Square;
        public CodeShape Value
        {
            get { return this._value; }
            set {
                this._value = value;

                this.NotifyAnimator();

                Debug.Log($"{this.name}: Value set to {value}");
            }
        }

        [SerializeField] protected UnityEvent OnTrigger;
        [SerializeReference] protected Animator _animator;

        public void Trigg()
        {
            Debug.Log($"{this.name}: Trigged");
            // note: the animation is updated within the setter of `ShapeSlot.Value`.
            this.Value = ShapeSlot.CalcNextShape(this.Value);
            this.InvokeOnTrigger();

            //TO_IDO: Add Key code SFX
            SFXPlayer.instance.PlaySFX(SFXPlayer.instance.keyCodeSFX);
        }

        public void InvokeOnTrigger()
        {
            if (this.OnTrigger != null)
            {
                this.OnTrigger.Invoke();
            }
        }

        protected void NotifyAnimator()
        {
            if (this._animator != null)
            {
                this._animator.SetInteger(ANIM_TOGGLE, (int)this.Value);
            }
        }

        private void OnEnable()
        {
            this.NotifyAnimator();
        }
    }
}

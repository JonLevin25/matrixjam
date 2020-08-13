using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MatrixJam.Team11
{
    public class Outlineotron : MonoBehaviour
    {
        public static readonly Vector2[] DEFAULT_OUTLINE_PARTS_OFFSETS = new Vector2[]
        {
            Vector2.up, Vector2.down, Vector2.left, Vector2.right
        };

        [SerializeReference] public SpriteRenderer OutlinedRenderer;
        [SerializeField] public Color OutlineColor = Color.black;

        [Tooltip("in pixels")]
        [SerializeField] protected Vector2[] _outlinePartsOffsets = (Vector2[])DEFAULT_OUTLINE_PARTS_OFFSETS.Clone();

        public Vector2[] OutlinePartsOffsets { get { return this._outlinePartsOffsets; } }

        protected SpriteTracker _tracker;

        public bool IsReady { get; protected set; } = false;

        void Awake()
        {
            this._tracker = this.gameObject.AddComponent<SpriteTracker>();
            this._tracker.Source = this.OutlinedRenderer;

            float offsetScale = 1f / this.OutlinedRenderer.sprite.pixelsPerUnit;

            this._tracker.Targets = new SpriteRenderer[this.OutlinePartsOffsets.Length];

            for (int i = 0; i < this.OutlinePartsOffsets.Length; i++)
            {
                GameObject partGameObject = new GameObject($"OutlinePart{this.OutlinePartsOffsets[i]}");
                SpriteRenderer part = partGameObject.AddComponent<SpriteRenderer>();

                partGameObject.transform.SetParent(this.OutlinedRenderer.transform, false);
                partGameObject.transform.localPosition = (Vector3)(this.OutlinePartsOffsets[i] * offsetScale);
                partGameObject.SetActive(this.enabled);

                this._tracker.Targets[i] = part;
            }

            this.enabled = false;  // by default the outline is off, it will be toggled by Player Controller script.
            this.IsReady = true;

            this.UpdateOutlineParts();

            // TODO: make the texture white, so outline is not affected by colors.
            // may be we can skip the color after all?
            // https://docs.unity3d.com/ScriptReference/TextureFormat.Alpha8.html
        }

        public void UpdateOutlineParts()
        {
            if (this.IsReady == false)
            {
                Debug.LogWarning($"{this.name}: UpdateOutlineParts must be called on a ready outlineotron!");
                return;  // the update will be performed when becoming ready anyway.
            }

            foreach (var part in this._tracker.Targets)
            {
                part.color = this.OutlineColor;
                part.sortingOrder = this.OutlinedRenderer.sortingOrder - 1;
            }
        }

        private void SetEnabled(bool isEnabled)
        {
            foreach (var part in this._tracker.Targets)
            {
                part.gameObject.SetActive(isEnabled);
            }
            this._tracker.enabled = isEnabled;
        }

        private void OnEnable()
        {
            this.SetEnabled(true);
            this.UpdateOutlineParts();
        }

        private void OnDisable()
        {
            this.SetEnabled(false);
        }
    }
}

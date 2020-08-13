using System;
using UnityEngine;

namespace MatrixJam.Team14
{
#if UNITY_EDITOR
    using UnityEditor;

    [CustomEditor(typeof(SetScreenPos))]
    public class SetScreenPosEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var script = (SetScreenPos) target;

            if (GUILayout.Button("Update"))
            {
                script.SetPos();
            }
        }
    }
#endif

    public class SetScreenPos : MonoBehaviour
    {
        [SerializeField] private Camera cam;
        [SerializeField] private Vector3 viewportPos;
        [SerializeField] private bool setOnStart = true;
        [SerializeField] private bool setOnUpdate;

        private void OnValidate()
        {
            SetPos();
        }

        private void Start()
        {
            if (setOnStart)
                SetPos();
        }

        private void Update()
        {
            if (setOnUpdate)
                SetPos();
        }

        public void SetPos()
        {
            var target = cam.ViewportToWorldPoint(viewportPos);
            transform.position = target;
        }
    }
}

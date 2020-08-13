using UnityEngine;

namespace MatrixJam.Team14
{
#if UNITY_EDITOR
    using UnityEditor;

    [CustomEditor(typeof(UpdateChildScreenPositions))]
    public class UpdateChildScreenPositionsEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var script = (UpdateChildScreenPositions) target;

            if (GUILayout.Button("Update"))
            {
                script.Update();
            }
        }
    }
#endif
    public class UpdateChildScreenPositions : MonoBehaviour
    {
        public void Update()
        {
            foreach (var setter in GetComponentsInChildren<SetScreenPos>())
            {
                setter.SetPos();
            }
        }
    }
}
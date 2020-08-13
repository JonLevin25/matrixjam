using System.Linq;
using UnityEngine;

namespace MatrixJam.Team14
{
#if UNITY_EDITOR
    using UnityEditor;

    [CustomEditor(typeof(GenerateInChildren))]
    public class GenerateTracksEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var script = target as GenerateInChildren;
            if (GUILayout.Button("Generate"))
            {
                script.Generate();
            }
        }
    }
#endif
    
    public class GenerateInChildren : MonoBehaviour
    {
        [SerializeField] private GameObject prefab;
        [SerializeField] private int amount;
        [SerializeField] private Vector3 initOffset;
        [SerializeField] private Vector3 delta;

        public void Generate()
        {
#if UNITY_EDITOR
            var userConfirm = EditorUtility.DisplayDialog("Confirm Delete",
                $"Generating will destory current children of {name}", "Continue", "Cancel");

            if (!userConfirm) return;

            var children = transform.Cast<Transform>().ToArray();
            foreach (var child in children)
                DestroyImmediate(child.gameObject);
            
            for (var i = 0; i < amount; i++)
            {
                var pos = initOffset + transform.position + (i * delta);
                var instance = (GameObject) PrefabUtility.InstantiatePrefab(prefab, transform);
                instance.transform.position = pos;
            }
#endif
        }
    }
}

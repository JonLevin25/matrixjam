#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;

namespace MatrixJam.Team17
{
    [CustomEditor(typeof(GameConfig))]
    public class GameConfigEditor : Editor
    {
        [MenuItem("TheFlyingDragons/View Game config %#g")]
        static void ViewGameConfig()
        {
            try
            {
                string[] assetsFound = AssetDatabase.FindAssets("TheFlyingDragons");
                string firstAssetGUID = assetsFound[0];
                string assetPath = AssetDatabase.GUIDToAssetPath(firstAssetGUID);
                GameConfig gameConfig = AssetDatabase.LoadAssetAtPath(assetPath, typeof(GameConfig)) as GameConfig;
                Selection.activeObject = gameConfig;
                Debug.Log("Selected: " + gameConfig);
            }
            catch (System.Exception)
            {
                Debug.LogError("Game config file: TheFlyingDragons is missing");
            }
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            GameConfig config = target as GameConfig;

            /*GUILayout.Space(10f);
            if (GUILayout.Button("Init player settings"))
                InitializePlayerSettings(config);*/

            GUILayout.Space(10f);
            if (GUILayout.Button("Import stuff"))
                Import(config);
        }
        /*
        void InitializePlayerSettings(GameConfig config)
        {
            Debug.Log("Initializing " + config.name);
            string gameId = "com.TheFlyingDragons.MatrixJam1";

            // PlayerSettings
            PlayerSettings.companyName = "TheFlyingDragons";
            string bundleVersion = config.version + config.buildNumber;
            PlayerSettings.bundleVersion = bundleVersion;
            PlayerSettings.productName = "TheFlyingDragons ";// v" + bundleVersion;
            // Android
            PlayerSettings.Android.bundleVersionCode = config.buildNumber;
            PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android, gameId);
            // iOS
            PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.iOS, gameId);
            // Standalone
            PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Standalone, gameId);
        }
        */
        void Import(GameConfig game)
        {
            // Grab spreadsheet data from googdle docs and what not

            //Game gameApp = GameObject.FindObjectOfType<Game>();
            // Update stuff
         
            // Done
            Debug.Log("Import of game stuff done");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        public static void WriteTextFile(string filePath, bool append, string text)
        {
            StreamWriter writer = new StreamWriter(filePath, append);
            writer.Write(text);
            writer.Close();
        }
    }
}
#endif
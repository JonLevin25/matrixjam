using System;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEditor;

namespace MatrixJam.TeamMeta
{
    [InitializeOnLoad]
    public class GameJamSetup : EditorWindow
    {
        private const string ExcludeTemplate =
@"#MATRIX_JAM_START
/Assets/*
/ProjectSettings/*
/Packages/*
!Assets/Team{0}
!Assets/Team{0}.meta
#MATRIX_JAM_END";
        private static string _teamNumberString = "";

        private static string _teamNumberFilePath =
            Path.Combine(Path.Combine(Path.Combine(Directory.GetCurrentDirectory(), "Assets"),"TeamMeta"), "teamNumber.txt");
        public static string TeamName 
        {
            get
            {
                return string.Format("Team{0}", _teamNumberString);;
            }            
        }

        static GameJamSetup()
        {
            if (File.Exists(_teamNumberFilePath))
            {
                _teamNumberString = File.ReadAllText(_teamNumberFilePath);
                if (_teamNumberString != "Meta")
                {
                    GameJamData.TeamNumber = Int32.Parse(_teamNumberString);
                }
            }
            else
            {
                Debug.LogError("No team file found, please run setup (MatrixGameJam > Setup) before continuing");
            }
        }
        
        [MenuItem("MatrixGameJam/Setup")]
        public static void RunSetup()
        {
            var window = ScriptableObject.CreateInstance<GameJamSetup>();
            window.ShowModalUtility();
        }

        [MenuItem("MatrixGameJam/CleanExclude")]
        public static void CleanExcludeMenuItem()
        {
            DeleteGitExclude();
            EditorUtility.DisplayDialog("Done", "removed local git ignore", "ok");
        }
        
        [MenuItem("MatrixGameJam/CleanExclude",true)]
        public static bool CleanExcludeMenuItemValidate()
        {
            return TeamName == "TeamMeta";
        }

        private static void CreateGitExclude()
        {
            var excludePath = Path.Combine(Path.Combine(Path.Combine(Directory.GetCurrentDirectory(), ".git"), "info"), "exclude");
            string excludeContent;
            if (File.Exists(excludePath))
            {
                excludeContent = File.ReadAllText(excludePath);

                if (Regex.IsMatch(excludeContent, "#MATRIX_JAM_START"))
                {
                    excludeContent = Regex.Replace(excludeContent, "#MATRIX_JAM_START.*#MATRIX_JAM_END",
                        string.Format(ExcludeTemplate, _teamNumberString), RegexOptions.Singleline);
                }
                else
                {
                    excludeContent += string.Format(ExcludeTemplate, _teamNumberString);    
                }
            }
            else
            {
                excludeContent = string.Format(ExcludeTemplate, _teamNumberString);
            }

            File.WriteAllText(excludePath, excludeContent);
        }

        private static void DeleteGitExclude()
        {
            var excludePath = Path.Combine(Path.Combine(Path.Combine(Directory.GetCurrentDirectory(), ".git"), "info"), "exclude");
            if (File.Exists(excludePath))
            {
                File.Delete(excludePath);
            }
        }

        private void OnGUI()
        {
            GUILayout.Label("Welcome to Matrix Game Jam");
            GUILayout.Label("Please input your team number and click select");
            GUILayout.Label("<Meta team members, please input Meta>");
            
            GUILayout.Space(10f);

            _teamNumberString = EditorGUILayout.TextField("Team Number", _teamNumberString);

            if (GUILayout.Button("Select"))
            {
                int teamNumber = 0;
                if (_teamNumberString == "Meta" || Int32.TryParse(_teamNumberString, out teamNumber))
                {
                    GameJamData.TeamNumber = teamNumber;
                    var teamRootPath = Path.Combine(Path.Combine(Directory.GetCurrentDirectory(), "Assets"),
                        TeamName);

                    if (_teamNumberString != "Meta")
                    {
                        Directory.CreateDirectory(teamRootPath);
                        CreateGitExclude();
                    }
                    else
                    {
                        DeleteGitExclude();
                    }

                    File.WriteAllText(_teamNumberFilePath,_teamNumberString);
                    
                    AssetDatabase.Refresh();
                    EditorUtility.DisplayDialog("Ready", "All set", "jam time");
                    Close();
                }
                else
                {
                    EditorUtility.DisplayDialog("Error",
                        @"Please input a number in the team number field", "ok");
                }
            }
        }
    }
}
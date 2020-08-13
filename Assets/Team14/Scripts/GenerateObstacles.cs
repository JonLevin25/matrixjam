using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MatrixJam.Team14
{
#if UNITY_EDITOR
    using UnityEditor;

    [CustomEditor(typeof(GenerateObstacles))]
    public class GenerateObstaclesEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var script = target as GenerateObstacles;
            if (GUILayout.Button("Generate"))
            {
                script.Generate();
            }
        }
    }
#endif
    
    [Serializable]
    public class ObstacleData
    {
        public readonly TrainMove Move;
        public readonly int Beat;

        public ObstacleData(TrainMove move, int beat)
        {
            this.Move = move;
            this.Beat = beat;
        }
    }
    
    public class GenerateObstacles : MonoBehaviour
    {
        [SerializeField] private TrackList trackList;
        [SerializeField] private Transform startAndDirection;
        
        [TextArea]
        [SerializeField] private string inputStr;
        [SerializeField] private GameObject jumpObstaclePrefab;
        [SerializeField] private GameObject duckObstaclePrefab;
        [SerializeField] private GameObject honkObstaclePrefab;

        public void Generate()
        {
#if UNITY_EDITOR
            var obstacleDatas = Parse(inputStr);

            foreach (var child in transform.Cast<Transform>().ToArray())
                DestroyImmediate(child.gameObject);
            
            foreach (var obstacleData in obstacleDatas)
            {
                var prefab = GetObstaclePrefab(obstacleData.Move);
                var pos = trackList.GetBeatPositionWithGlobalBeat(startAndDirection, obstacleData.Beat);
                var go = (GameObject)PrefabUtility.InstantiatePrefab(prefab, transform);
                go.transform.position = pos;

                var obstacleGen = go.GetComponent<SetObstacleParams>();

                var trackIdx = trackList.GetTrackIdxByGlobalBeat(obstacleData.Beat);
                var track = trackList[trackIdx];
                obstacleGen.SetParams(track);
            }
            
#endif
        }
        
        private static IEnumerable<ObstacleData> Parse(string str) 
            => str.Split('\n').Select(ParseLine).Where(data => data != null);

        private GameObject GetObstaclePrefab(TrainMove move)
        {
            switch (move)
            {
                case TrainMove.Jump:
                    return jumpObstaclePrefab;
                case TrainMove.Duck:
                    return duckObstaclePrefab;
                case TrainMove.Honk:
                    return honkObstaclePrefab;
                default:
                    throw new ArgumentOutOfRangeException(nameof(move), move, null);
            }
        }

        private static ObstacleData ParseLine(string line)
        {
            var parts = line.Split('\t');
            if (parts.Length < 2)
            {
                Debug.LogError($"ObstacleLine could not be parsed! ({line})");
                return null;
            }

            var moveStr = parts[0];
            var success = Enum.TryParse(moveStr, out TrainMove move);
            if (!success)
            {
                Debug.LogError($"Could not parse moveType! ({moveStr})");
                return null;
            }

            var timeStr = parts[1];
            success = int.TryParse(timeStr, out var beat);
            if (!success)
            {
                Debug.LogError($"Could not parse time! ({timeStr})");
                return null;
            }
            
            return new ObstacleData(move, beat);
        }
    }
}

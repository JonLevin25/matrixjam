﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MatrixJam.Team14
{
    [CreateAssetMenu(menuName = "Team14/TrackList", fileName = nameof(TrackList))]
    public class TrackList : ScriptableObject
    {
        [SerializeField] private MusicTrack[] tracks;

        private void OnValidate()
        {
            if (GameManager.Instance) GameManager.Instance.OnValidate();
        }
        
        public int TrackCount => tracks.Length;

        public IEnumerable<Vector3> GetAllBeatPositions(Transform startAndDirection)
        {
            var offset = Vector3.zero;
            for (var i = 0; i < TrackCount; i++)
            {
                var track = tracks[i];
                for (var beatNum = 0; beatNum < Mathf.FloorToInt(track.TotalBeats); beatNum++)
                    yield return track.GetPosition(startAndDirection, beatNum, offset);

                offset = track.GetLastPosition(startAndDirection);
            }
        }

        public IEnumerable<Vector3> GetTrackEndPositions(Transform startAndDirection)
        {
            var offset = Vector3.zero;
            foreach (var track in tracks)
            {
                var lastPos = track.GetLastPosition(startAndDirection);
                yield return offset + lastPos;
                offset += lastPos;
            }
        }
    
        public IEnumerable<Vector3> GetTrackStarts(Transform startAndDirection)
        {
            var offset = Vector3.zero;
            foreach (var track in tracks)
            {
                yield return track.GetPosition(startAndDirection, 0f, offset);
                offset = track.GetLastPosition(startAndDirection);
            }
        }

        public AudioClip GetTrack(int trackIdx) => tracks[trackIdx].Clip;

        // public float BeatsInTrack(int trackIdx) => tracks[trackIdx].TotalBeats;

        // public float GetBeatNum(int trackIdx, float secsInTrack) => tracks[trackIdx].GetBeatNum(secsInTrack);


        public Vector3 GetBeatPosition(Transform startAndDirection, int trackIdx, float trackSecs)
        {
            // The position where this track starts
            var offset = tracks.Take(trackIdx).Aggregate(
                Vector3.zero,
                (sum, track) => sum + track.GetLastPosition(startAndDirection)
            );
            
            var currTrack = tracks[trackIdx];
            var beatNum = currTrack.GetBeatNum(trackSecs);
            var currTrackPos = currTrack.GetPosition(startAndDirection, beatNum);

            return offset + currTrackPos;
        }
    }
}
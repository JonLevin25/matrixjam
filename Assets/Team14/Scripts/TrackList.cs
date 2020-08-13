using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MatrixJam.Team14
{
    [CreateAssetMenu(menuName = "Team14/TrackList", fileName = nameof(TrackList))]
    public class TrackList : ScriptableObject
    {
        [SerializeField] private MusicTrack[] tracks;


        public MusicTrack[] Tracks => tracks;

        public int TrackCount => tracks.Length;

        private IEnumerable<MusicTrack> TracksExceptLast
        {
            get
            {
                for (var i = 0; i < tracks.Length - 1; i++)
                    yield return tracks[i];
            }
        }

        private void OnValidate()
        {
            if (GameManager.Instance) GameManager.Instance.OnValidate();
        }

        public IEnumerable<Vector3> GetAllBeatPositions(Transform startAndDirection)
        {
            var offset = Vector3.zero;
            for (var i = 0; i < TrackCount; i++)
            {
                var track = tracks[i];
                for (var beatNum = 0; beatNum < Mathf.FloorToInt(track.TotalBeats); beatNum++)
                    yield return track.BeatsToPosition(startAndDirection, beatNum, offset);

                offset += track.GetLastPosition(startAndDirection);
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
                yield return track.BeatsToPosition(startAndDirection, 0f, offset);
                offset = track.GetLastPosition(startAndDirection);
            }
        }

        public MusicTrack this[int i] => tracks[i];

        public AudioClip GetClip(int trackIdx) => tracks[trackIdx].Clip;

        public float BeatsInTrack(int trackIdx) => tracks[trackIdx].TotalBeats;

        // public float GetBeatNum(int trackIdx, float secsInTrack) => tracks[trackIdx].GetBeatNum(secsInTrack);
        public AudioClip GetRailway(int trackIdx) => tracks[trackIdx].RailwaySFX;


        public Vector3 GetBeatPosition(Transform startAndDirection, int trackIdx, float trackSecs)
        {
            // The position where this track starts
            var offset = tracks.Take(trackIdx).Aggregate(
                Vector3.zero,
                (sum, track) => sum + track.GetLastPosition(startAndDirection)
            );
            
            var currTrack = tracks[trackIdx];
            var beatNum = currTrack.GetBeatNum(trackSecs);
            var currTrackPos = currTrack.BeatsToPosition(startAndDirection, beatNum);

            return offset + currTrackPos;
        }

        public int GetTrackIdxByGlobalBeat(float beatNum)
        {
            var beatOffset = 0f;
            var trackIdx = 0;

            foreach (var track in TracksExceptLast)
            {
                if (beatOffset + track.TotalBeats >= beatNum) break;
                beatOffset += track.TotalBeats;
                trackIdx++;
            }

            //var trackBeat = beatNum - beatOffset;
            return trackIdx;
        }

        public float GetTotalBeatCount() => tracks.Sum(track => track.TotalBeats);

        public (int trackIdx, float trackBeatNum) GetInfoFromGlobalBeat(float globalBeatNum)
        {
            var remainingBeats = globalBeatNum;
            for (var trackIdx = 0; trackIdx < tracks.Length; trackIdx++)
            {
                var track = tracks[trackIdx];
                if (track.TotalBeats > remainingBeats) return (trackIdx, remainingBeats);
                remainingBeats -= track.TotalBeats;
            }

            throw new ArgumentException($"Beat {globalBeatNum} was not found!");
        }

        public Vector3 GetBeatPositionWithGlobalBeat(Transform startAndDirection, float beatNum)
        {
            var trackIdx = 0;
            var beatOffset = 0f;
            var posOffset = Vector3.zero;

            foreach (var track in TracksExceptLast)
            {
                if (beatOffset + track.TotalBeats >= beatNum) break;
                beatOffset += track.TotalBeats;
                posOffset += track.GetLastPosition(startAndDirection);
                trackIdx++;
            }

            var trackBeat = beatNum - beatOffset;

            return tracks[trackIdx].BeatsToPosition(startAndDirection, trackBeat, posOffset);
        }

        public Vector3 GetPositionWithGlobalTime(Transform startAndDirection, float totalSeconds)
        {
            var trackIdx = 0;
            var timeOffset = 0f;
            foreach (var tttrack in tracks)
            {
                if (timeOffset + tttrack.TotalSeconds >= totalSeconds) break;
                timeOffset += tttrack.TotalSeconds;
                trackIdx++;
            }

            var trackTime = totalSeconds - timeOffset;
            return GetBeatPosition(startAndDirection, trackIdx, trackTime);
        }
    }
}
using System;
using System.Linq;
using UnityEngine;

namespace MatrixJam.Team14
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private TrackList trackList;
        [SerializeField] private AudioSource source;
        [SerializeField] private AudioSource railwaySource;

        private int _trackIdx;
        

        public event Action<int> OnFinishTrack;
        public event Action OnFinishTracklist;
        
        private MusicTrack CurrTrack => trackList[_trackIdx];

        private void Awake()
        {
            source.Stop();
        }
        
        private void Update()
        {
            var donePlayingTrack = source.time >= source.clip.length;
            if (donePlayingTrack) OnTrackFinishedInternal();
        }

        public Vector3 GetCurrPosition(Transform startAndDirection)
        {
            var time = Mathf.Clamp(source.time, 0f, CurrTrack.TotalSeconds);
            var pos = trackList.GetBeatPosition(startAndDirection, _trackIdx, time);
            
            // Debug.Log($"POS: {pos.z}\tFinal: {time:F3}\tTime: {source.time:f3}\tClip: {source.clip.length:f3}");
            
            return pos;
        }
        
        public void Restart(float beatOffset)
        {
            // Wrap around
            if (beatOffset < 0) beatOffset = trackList.GetTotalBeatCount() + beatOffset;

            var (trackIdx, trackBeatOffset) = trackList.GetInfoFromGlobalBeat(beatOffset);
            var trackSecsOffset = trackList[trackIdx].BeatsToSeconds(trackBeatOffset);
            StartTrack(trackIdx, trackSecsOffset);
        }
        
        public void RestartLastCheckpoint()
        {
            var lastTrackWithCheckpoint = trackList.Tracks
                .Take(_trackIdx) // Check up until this index - was there "checkpoint after" prev tracks
                .Select((track, i) => new {track, i})
                .LastOrDefault(x => x.track.CheckpointAfterFinish);

            var trackIdx = lastTrackWithCheckpoint?.i + 1 ?? 0;
            Debug.Log($"RestartLastCheckpoint. Idx: {trackIdx}");
            StartTrack(trackIdx);
        }

        public Vector3[] GetAllBeatPositions(Transform startAndDirection) 
            => trackList.GetAllBeatPositions(startAndDirection).ToArray();

        public Vector3[] GetTrackEndPositions(Transform startAndDirection)
            => trackList.GetTrackEndPositions(startAndDirection).ToArray();
        public Vector3[] GetTrackStartPositions(Transform startAndDirection)
            => trackList.GetTrackStarts(startAndDirection).ToArray();

        private void StartTrack(int track, float secsOffset = 0f)
        {
            _trackIdx = track;
            
            RestartPlayAudio(source, trackList.GetClip(_trackIdx), secsOffset);
            RestartPlayAudio(railwaySource, trackList.GetRailway(_trackIdx), secsOffset);
        }

        private void RestartPlayAudio(AudioSource source, AudioClip clip, float secsOffset = 0f)
        {
            source.Stop();
            source.clip = clip;
            source.time = secsOffset;
            source.Play();
        }

        private void NextTrack()
        {
            Debug.Log($"NextTrack ({_trackIdx} -> {_trackIdx+1})");
            _trackIdx++;
            StartTrack(_trackIdx);
        }

        // TODO: GameManager: handle Events
        private void OnTrackFinishedInternal()
        {
            OnFinishTrack?.Invoke(_trackIdx);
            if (_trackIdx == trackList.TrackCount - 1) OnLastTrackFinished();
            else NextTrack();
        }

        private void OnLastTrackFinished()
        {
            OnFinishTracklist?.Invoke();
        }

        public float GetCurrGlobalSecs()
        {
            var timeOffset = 0f;
            foreach (var track in trackList.Tracks.Take(_trackIdx-1))
            {
                timeOffset += track.TotalSeconds;
            }

            return timeOffset + source.time;
        }

        public void Pause(bool pause)
        {
            if (pause) source.Pause();
            else source.UnPause();
        }
    }
}
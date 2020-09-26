using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace MatrixJam.Team14
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private TrackList trackList;
        [SerializeField] private AudioSource source;
        [SerializeField] private AudioSource railwaySource;

        private bool _aboutToFinishTrack;
        private int _trackIdx;
        
        private const float AboutToFinishTrackPercent = 0.8f;
        private const string LogPrefix = nameof(AudioManager) + ":";

        public event Action<int> OnFinishTrack;
        public event Action OnFinishTracklist;
        
        private MusicTrack CurrTrack => trackList[_trackIdx];

        private void Awake()
        {
            source.Stop();
        }
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                Debug.Log($"{LogPrefix} UPDATE: trackList: {trackList.TrackCount}. trackIdx: {_trackIdx}. srcTime: {source.time}. clipLen: {source.clip.length}. AboutToFinish: {_aboutToFinishTrack}");
            }


            // Fix for WebGL Wrapping around to 0f of audiosource when finishing track
            // Seems first frame after "overflow" source continues (e.g. 0.015f) So need a short arbitrary lower bound.
            // 1.5 is arbitrary but seems long enough to cover a spike of frame drops around track changing.
            var glitchedToBeginning = _aboutToFinishTrack && source.time <  1.5f;
            
            if (glitchedToBeginning) Debug.Log($"{LogPrefix} GLITCHED TO BEGINNING!");
            UpdateAboutToFinishFlag();
            
            var donePlayingTrack = source.time >= source.clip.length;
            if (donePlayingTrack || glitchedToBeginning) OnTrackFinishedInternal();
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
            Debug.Log($"{LogPrefix} {nameof(Restart)}()");
            // Wrap around
            if (beatOffset < 0) beatOffset = trackList.GetTotalBeatCount() + beatOffset;

            var (trackIdx, trackBeatOffset) = trackList.GetInfoFromGlobalBeat(beatOffset);
            var trackSecsOffset = trackList[trackIdx].BeatsToSeconds(trackBeatOffset);
            StartTrack(trackIdx, trackSecsOffset);
        }

        public void RestartLastCheckpoint()
        {
            Debug.Log($"{LogPrefix} {nameof(RestartLastCheckpoint)}()");
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
            Debug.Log($"{LogPrefix} {nameof(StartTrack)}({track}, offset: {secsOffset})");
            _trackIdx = track;
            
            RestartPlayAudio(source, trackList.GetClip(_trackIdx), secsOffset);
            RestartPlayAudio(railwaySource, trackList.GetRailway(_trackIdx), secsOffset);
            _aboutToFinishTrack = false;
        }

        private void RestartPlayAudio(AudioSource source, AudioClip clip, float secsOffset = 0f)
        {
            Debug.Log($"{LogPrefix} {nameof(RestartPlayAudio)}");
            source.Stop();
            source.clip = clip;
            source.time = secsOffset;
            source.Play();
            
            UpdateAboutToFinishFlag();
        }

        private void NextTrack()
        {
            Debug.Log($"{LogPrefix} NextTrack ({_trackIdx} -> {_trackIdx+1})");
            _trackIdx++;
            StartTrack(_trackIdx);
        }

        // TODO: GameManager: handle Events

        private void OnTrackFinishedInternal()
        {
            Debug.Log($"{LogPrefix} {nameof(OnTrackFinishedInternal)}()");
            OnFinishTrack?.Invoke(_trackIdx);
            if (_trackIdx == trackList.TrackCount - 1) OnLastTrackFinished();
            else NextTrack();
        }

        private void OnLastTrackFinished()
        {
            Debug.Log($"{LogPrefix} {nameof(OnLastTrackFinished)}()");
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
            Debug.Log($"{LogPrefix} {nameof(Pause)}()");
            if (pause) source.Pause();
            else source.UnPause();
        }

        private void UpdateAboutToFinishFlag()
        {
            Assert.IsNotNull(source);
            Assert.IsNotNull(source.clip);
            
            var progress = source.time / source.clip.length;
            _aboutToFinishTrack = progress >= AboutToFinishTrackPercent;
        }
    }
}
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

namespace PawnshopSimulator.Audio
{
    public class Sound : MonoBehaviour
    {
        private const float SPATIAL_BLEND_3D = 1;
        private const float SPATIAL_BLEND_2D = 0;

        private const float SOUND_MAX_DISTANCE = 13f;
        private const float SOUND_MIN_DISTANCE = 0;
        private const AudioRolloffMode ROLLOF_MODE = AudioRolloffMode.Linear;
        private const int SOUND_SPREAD = 360;

        private const int PRIORITY_MUSIC = 128;
        private const int PRIORITY_SOUNDS = 256;

        private SoundStreams _stream;
        private AudioSource _source;
        private Coroutine _playRoutine;
        private bool _isLoop;
        private bool _isPlaing;
        private float _baseVolume = 1;

        public UnityEngine.Events.UnityAction<Sound> OnStop;

        public string GetSoundName
        {
            get
            {
                if (_source != null) return _source.clip.name;
                else return "";
            }
        }
        public bool IsPlaying { get => _isPlaing; }
        public SoundStreams GetSoundStream { get => _stream; }
        private void Awake()
        {
            _source = GetComponent<AudioSource>();
            _source.priority = PRIORITY_SOUNDS;
            _source.maxDistance = SOUND_MAX_DISTANCE;
            _source.minDistance = SOUND_MIN_DISTANCE;
            _source.rolloffMode = ROLLOF_MODE;
            _source.spread = SOUND_SPREAD;
        }

        public bool EqualsClip(AudioClip clip)
            => _source.clip == clip;

        public void Init(AudioClip clip, SoundStreams soundsType, Transform holder, bool isLoop, bool is3D, float pitch, AudioMixerGroup mixerGroup)
        {
            transform.SetParent(holder);
            transform.localPosition = Vector3.zero;

            _source.time = 0;
            _source.volume = _baseVolume;
            _source.spatialBlend = is3D ? SPATIAL_BLEND_3D : SPATIAL_BLEND_2D;
            _source.clip = clip;
            _source.loop = isLoop;
            _source.pitch = pitch;
            _source.priority = GetStreamPriority(soundsType);
            _source.outputAudioMixerGroup = mixerGroup;

            _isLoop = isLoop;
            _stream = soundsType;
        }
        public void Init(AudioClip clip, SoundStreams soundsType, Transform holder, bool isLoop, bool is3D, Vector2 positon, float pitch, AudioMixerGroup mixerGroup)
        {
            Init(clip, soundsType, holder, isLoop, is3D, pitch, mixerGroup);

            transform.position = positon;
        }
        public bool CompareClips(AudioClip clip)
        {
            return _source.clip == clip;
        }

        public void Play()
        {
            if (_isPlaing) return;

            if (_source != null)
            {
                gameObject.SetActive(true);
                _isPlaing = true;
                _source.time = 0;
                _source.Play();
                if (_playRoutine != null) StopCoroutine(_playRoutine);
                _playRoutine = StartCoroutine(PlayRoutine());
            }
        }
        public void Stop()
        {
            if (!_isPlaing) return;

            if (_source != null)
            {
                _isPlaing = false;
                if (_playRoutine != null) StopCoroutine(_playRoutine);

                _source.Stop();
                OnStop?.Invoke(this);
                gameObject.SetActive(false);
            }
        }

        private int GetStreamPriority(SoundStreams soundsType)
        {
            switch (soundsType)
            {
                case SoundStreams.MUSIC:
                    return PRIORITY_MUSIC;
                case SoundStreams.EFFECTS:
                    return PRIORITY_SOUNDS;
            }

            return 0;
        }

        private IEnumerator PlayRoutine()
        {
            yield return new WaitForSeconds(_source.clip.length);
            if (_isLoop) Play();
            else Stop();
        }
    }
}

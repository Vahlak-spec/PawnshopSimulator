using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace PawnshopSimulator.Audio
{
    public class SoundsManager
    {
        public const string MUSIC_VOLUME_KEY = "MUSIC_VOLUME";
        public const string EFFECTS_VOLUME_KEY = "EFFECTS_VOLUME";

        public const string SOUNDS_FOLDER_PATH = "Sounds/Effects/";
        public const string MUSIC_FOLDER_PATH = "Sounds/Music/";

        public const int MAX_EFFECTS_DUBLICATES = 5;
        public const int MAX_MUSIC_DUBLICATES = 1;

        public const float EFFECTS_MAX_VOLUME = 0;
        public const float EFFECTS_MIN_VOLUME = -40;
        public const float MUSIC_MAX_VOLUME = 0;
        public const float MUSIC_MIN_VOLUME = -40;

        private static Transform _soundsHolder;
        private static Transform _musicHolder;

        private static List<Sound> _activeSounds = new List<Sound>();
        private static List<Sound> _poolSounds = new List<Sound>();

        private static AudioMixer _mixer;

        private static AudioMixerGroup _soundsGroup;
        private static AudioMixerGroup _musicGroup;

        private static Sound _tempMusic;

        private static bool isInit = false;

        public static void InitPool(AudioMixerGroup soundsGroup, AudioMixerGroup musicGroup, AudioMixer mixer)
        {
            if (isInit) return;

            _mixer = mixer;
            _soundsGroup = soundsGroup;
            _musicGroup = musicGroup;

            isInit = true;

            GameObject _poolHolder = new GameObject();
            _poolHolder.name = "[SOUNDS POOL]";
            GameObject.DontDestroyOnLoad(_poolHolder);

            GameObject soundsHolder = new GameObject();
            soundsHolder.transform.SetParent(_poolHolder.transform);
            soundsHolder.name = "<SOUNDS STREAM>";

            GameObject musicHolder = new GameObject();
            musicHolder.transform.SetParent(_poolHolder.transform);
            musicHolder.name = "<MUSIC STREAM>";

            GameObject _ambientHolder = new GameObject();
            _ambientHolder.transform.SetParent(_poolHolder.transform);
            _ambientHolder.name = "<AMBIENT STREAM>";

            _soundsHolder = soundsHolder.transform;
            _musicHolder = musicHolder.transform;
        }

        public static Sound PlaySound(string soundName, Transform source = null, bool loop = false, bool is3D = false, float pitch = 1)
        {
            if (soundName != "")
            {
                AudioClip loadClip = Resources.Load<AudioClip>(SOUNDS_FOLDER_PATH + soundName);

                if (loadClip != null)
                {
                    Sound tempSound = null;

                    if (!CheckDublicate(loadClip.name, SoundStreams.EFFECTS, out Sound lastSound)) tempSound = lastSound;
                    else tempSound = AddSound();

                    tempSound.Init(loadClip,
                       SoundStreams.EFFECTS,
                        source == null ? _soundsHolder : source,
                        loop,
                       is3D, pitch, _soundsGroup);
                    tempSound.Play();

                    return tempSound;
                }
                else
                {
                    Debug.LogError("- Cant find sound with this name!");
                    return null;
                }
            }
            else
            {
                Debug.LogError("- Sound is empty!");
                return null;
            }
        }
        public static Sound PlaySound(string soundName, Vector2 position, bool loop = false, bool is3D = false, float pitch = 1)
        {
            if (soundName != "")
            {
                AudioClip loadClip = Resources.Load<AudioClip>(SOUNDS_FOLDER_PATH + soundName);

                if (loadClip != null)
                {
                    Sound tempSound = null;

                    if (!CheckDublicate(loadClip.name, SoundStreams.EFFECTS, out Sound lastSound)) tempSound = lastSound;
                    else tempSound = AddSound();

                    tempSound.Init(loadClip,
                       SoundStreams.EFFECTS,
                        _soundsHolder,
                        loop,
                       is3D,
                    position, pitch, _soundsGroup);
                    tempSound.Play();

                    return tempSound;
                }
                else
                {
                    Debug.LogError("- Cant find sound with this name!");
                    return null;
                }
            }
            else
            {
                Debug.LogError("- Sound is empty!");
                return null;
            }
        }
        public static Sound PlaySound(AudioClip sound, Transform source = null, bool loop = false, bool is3D = false, float pitch = 1)
        {
            if (sound != null)
            {
                Sound tempSound = null;

                if (!CheckDublicate(sound.name, SoundStreams.EFFECTS, out Sound lastSound)) tempSound = lastSound;
                else tempSound = AddSound();

                tempSound.Init(sound,
                   SoundStreams.EFFECTS,
                    source == null ? _soundsHolder : source,
                    loop,
                    is3D, pitch, _soundsGroup);
                tempSound.Play();

                return tempSound;
            }
            else
            {
                Debug.LogError("- Sound is empty!");
                return null;
            }
        }
        public static Sound PlaySound(AudioClip sound, Vector2 position, bool loop = false, bool is3D = false, float pitch = 1)
        {
            if (sound != null)
            {
                Sound tempSound = null;

                if (!CheckDublicate(sound.name, SoundStreams.EFFECTS, out Sound lastSound)) tempSound = lastSound;
                else tempSound = AddSound();

                tempSound.Init(sound,
                    SoundStreams.EFFECTS,
                    _soundsHolder,
                    loop,
                   is3D,
                    position, pitch, _soundsGroup);
                tempSound.Play();

                return tempSound;
            }
            else
            {
                Debug.LogError("- Sound is empty!");
                return null;
            }
        }

        public static Sound PlayMusic(AudioClip sound, bool loop = true, bool is3D = false, float pitch = 1)
        {
            if (sound != null)
            {
                StopStream(SoundStreams.MUSIC);

                Sound tempSound = AddSound();
                tempSound.Init(sound,
                    SoundStreams.MUSIC,
                    _musicHolder,
                    loop,
                   is3D, pitch, _musicGroup);
                tempSound.Play();

                return tempSound;
            }
            else
            {
                Debug.LogError("- Sound is empty!");
                return null;
            }
        }
        public static Sound PlayMusic(string soundName, bool loop = true, bool is3D = false, float pitch = 1)
        {
            if (soundName != "")
            {
                AudioClip loadClip = Resources.Load<AudioClip>(MUSIC_FOLDER_PATH + soundName);

                Debug.Log(MUSIC_FOLDER_PATH + soundName);

                if (_tempMusic != null && _tempMusic.EqualsClip(loadClip)) return _tempMusic;

                if (loadClip != null)
                {
                    StopStream(SoundStreams.MUSIC);
                    Sound tempSound = AddSound();
                    tempSound.Init(loadClip,
                        SoundStreams.MUSIC,
                        _musicHolder,
                        loop,
                       is3D, pitch, _musicGroup);
                    tempSound.Play();

                    tempSound.Play();

                    _tempMusic = tempSound;

                    return tempSound;
                }
                else
                {
                    Debug.LogError("- Cant find sound with this name!");
                    return null;
                }
            }
            else
            {
                Debug.LogError("- Sound is empty!");
                return null;
            }
        }
        public static void SetStreamVolume(SoundStreams soundStream, float value)
        {
            switch (soundStream)
            {
                case SoundStreams.MUSIC:
                    _mixer.SetFloat(MUSIC_VOLUME_KEY, value == 0 ? -80 : MUSIC_MIN_VOLUME + value * (MUSIC_MAX_VOLUME - MUSIC_MIN_VOLUME));
                    break;
                case SoundStreams.EFFECTS:
                    _mixer.SetFloat(EFFECTS_VOLUME_KEY, value == 0 ? -80 : EFFECTS_MIN_VOLUME + value * (EFFECTS_MAX_VOLUME - EFFECTS_MIN_VOLUME));
                    break;
                default:
                    break;
            }
        }

        public static float GetStreamVolume(SoundStreams soundStream)
        {
            float res;
            switch (soundStream)
            {
                case SoundStreams.MUSIC:
                    _mixer.GetFloat(MUSIC_VOLUME_KEY, out res);
                    return (res - MUSIC_MIN_VOLUME)/ (MUSIC_MAX_VOLUME - MUSIC_MIN_VOLUME);
                case SoundStreams.EFFECTS:
                    _mixer.GetFloat(EFFECTS_VOLUME_KEY, out res);
                    return (res - EFFECTS_MIN_VOLUME) / (EFFECTS_MAX_VOLUME - EFFECTS_MIN_VOLUME);
                default:
                    return 1;
            }
        }
        public static void SetActiveStream(SoundStreams soundStream, bool isActive)
        {
            switch (soundStream)
            {
                case SoundStreams.MUSIC:
                    _mixer.SetFloat(MUSIC_VOLUME_KEY, isActive ? MUSIC_MAX_VOLUME : -80);
                    break;
                case SoundStreams.EFFECTS:
                    _mixer.SetFloat(EFFECTS_VOLUME_KEY, isActive ? EFFECTS_MAX_VOLUME : -80);
                    break;
                default:
                    break;
            }
        }
        public static void ClearStream(SoundStreams soundStream)
        {
            Sound[] sounds = GetStreamSounds(soundStream);
            Transform holder = GetHolder(soundStream);

            for (int i = 0; i < sounds.Length; i++)
            {
                Sound cash = sounds[i];

                if (cash != null)
                {
                    cash.transform.SetParent(holder);

                    if (_activeSounds.Contains(cash)) _activeSounds.Remove(cash);
                    if (_poolSounds.Contains(cash)) _poolSounds.Remove(cash);
                }
            }

            for (int i = 0; i < sounds.Length; i++)
            {
                Sound cash = sounds[i];

                if (cash != null) GameObject.Destroy(cash.gameObject);
            }
        }

        private static Transform GetHolder(SoundStreams soundStream)
        {
            switch (soundStream)
            {
                case SoundStreams.MUSIC:
                    return _musicHolder;
                case SoundStreams.EFFECTS:
                    return _soundsHolder;
            }

            return null;
        }

        private static List<Sound> _getStreamSoundsResult = new List<Sound>();
        private static Sound[] GetStreamSounds(SoundStreams soundStream)
        {
            _getStreamSoundsResult.Clear();

            for (int i = 0; i < _poolSounds.Count; i++)
            {
                if (_poolSounds[i] == null && i < _poolSounds.Count && i > 0) _poolSounds.RemoveAt(i);
            }
            for (int i = 0; i < _activeSounds.Count; i++)
            {
                if (_activeSounds[i] == null && i < _activeSounds.Count && i > 0) _activeSounds.RemoveAt(i);
            }

            if (_activeSounds.Count > 0)
            {
                for (int i = 0; i < _activeSounds.Count; i++)
                {
                    if (_activeSounds[i].GetSoundStream == soundStream)
                    {
                        _getStreamSoundsResult.Add(_activeSounds[i]);
                    }
                }
            }
            if (_poolSounds.Count > 0)
            {
                for (int i = 0; i < _poolSounds.Count; i++)
                {
                    if (_poolSounds[i].GetSoundStream == soundStream)
                    {
                        _getStreamSoundsResult.Add(_poolSounds[i]);
                    }
                }
            }

            return _getStreamSoundsResult.ToArray();
        }
        public static void StopStream(SoundStreams soundStream)
        {
            Sound[] soundsOnStream = GetStreamSounds(soundStream);

            if (soundsOnStream.Length <= 0) return;

            for (int i = 0; i < soundsOnStream.Length; i++)
            {
                soundsOnStream[i].Stop();
            }
        }
        private static bool CheckDublicate(string soundName, SoundStreams stream, out Sound lastSound)
        {
            Sound[] streamSounds = GetStreamSounds(stream);
            lastSound = null;
            int maxSounds = GetMaxDublicate(stream);

            if (streamSounds.Length <= maxSounds) return true;

            int dublicatesNumber = 0;

            Sound cashedSound;

            for (int i = 0; i < streamSounds.Length; i++)
            {
                cashedSound = streamSounds[i];
                if (cashedSound.GetSoundName == soundName && cashedSound.IsPlaying)
                {
                    lastSound = cashedSound;
                    dublicatesNumber++;
                }
            }

            if (lastSound != null) lastSound.Stop();

            return dublicatesNumber <= maxSounds;
        }
        private static int GetMaxDublicate(SoundStreams stream)
        {
            switch (stream)
            {
                case SoundStreams.MUSIC:
                    return MAX_MUSIC_DUBLICATES;
                case SoundStreams.EFFECTS:
                    return MAX_EFFECTS_DUBLICATES;
                default:
                    return 0;
            }
        }

        private static void PoolSound(Sound sound)
        {
            if (_activeSounds.Contains(sound)) _activeSounds.Remove(sound);
            if (!_poolSounds.Contains(sound)) _poolSounds.Add(sound);

            sound.gameObject.SetActive(false);
        }
        private static Sound AddSound()
        {
            Sound sound = null;

            int count = _poolSounds.Count;
            int index = 0;

            for (int i = 0; i < count;)
            {
                if (_poolSounds[i] == null)
                {
                    _poolSounds.RemoveAt(i);
                    count = _poolSounds.Count;
                }
                else
                {
                    i++;
                }
            }

            count = _activeSounds.Count;
            index = 0;

            for (int i = 0; i < count;)
            {
                if (_activeSounds[i] == null)
                {
                    _activeSounds.RemoveAt(i);
                    count = _activeSounds.Count;
                }
                else
                {
                    i++;
                }
            }

            if (_poolSounds.Count <= 0)
            {
                GameObject newSound = new GameObject();
                GameObject.DontDestroyOnLoad(newSound);
                AudioSource audioSource = newSound.AddComponent<AudioSource>();

                sound = newSound.AddComponent<Sound>();
                sound.OnStop += PoolSound;

                _activeSounds.Add(sound);

                return sound;
            }

            sound = _poolSounds[0];

            _poolSounds.RemoveAt(0);
            _activeSounds.Add(sound);

            return sound;
        }
    }
    public enum SoundStreams
    {
        MUSIC,
        EFFECTS,
    }
}

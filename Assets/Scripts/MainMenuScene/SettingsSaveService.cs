using System.IO;
using UnityEngine;
using PawnshopSimulator.Audio;
using PawnshopSimulator.Services;

namespace PawnshopSimulator.MainMenu
{
    public class SettingsSaveService : IGameService
    {
        public float MusicVolume => _data.MusicVolume;
        public float EffectsVolume => _data.EffectsVolume;

        private SettingsSaveData _data;
        private readonly string _savePath;

        public SettingsSaveService()
        {
            _savePath = Path.Combine(Application.persistentDataPath, "settings.json");
            Load();
        }

        public void Bind(ServicesProvider componentProvider) { }
        public void OnLaunchGame() { }

        public void SetMusicVolume(float value)
        {
            _data.MusicVolume = value;
            SoundsManager.SetStreamVolume(SoundStreams.MUSIC, value);
            Save();
        }

        public void SetEffectsVolume(float value)
        {
            _data.EffectsVolume = value;
            SoundsManager.SetStreamVolume(SoundStreams.EFFECTS, value);
            Save();
        }

        public void ApplyToSoundsManager()
        {
            SoundsManager.SetStreamVolume(SoundStreams.MUSIC, _data.MusicVolume);
            SoundsManager.SetStreamVolume(SoundStreams.EFFECTS, _data.EffectsVolume);
        }

        private void Save()
        {
            File.WriteAllText(_savePath, JsonUtility.ToJson(_data, true));
        }

        private void Load()
        {
            if (!File.Exists(_savePath))
            {
                _data = new SettingsSaveData();
                return;
            }

            _data = JsonUtility.FromJson<SettingsSaveData>(File.ReadAllText(_savePath));
        }
    }
}

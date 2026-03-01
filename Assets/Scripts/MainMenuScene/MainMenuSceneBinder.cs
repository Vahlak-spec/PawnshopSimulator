using UnityEngine;
using UnityEngine.Audio;
using PawnshopSimulator.Audio;
using PawnshopSimulator.Services;

namespace PawnshopSimulator.MainMenu
{
    public class MainMenuSceneBinder : MonoBehaviour
    {
        [SerializeField] private MainMenuUIHolder _uiHolder;
        [Space]
        [SerializeField] private AudioMixer _audioMixer;
        [SerializeField] private AudioMixerGroup _soundsGroup;
        [SerializeField] private AudioMixerGroup _musicGroup;
        [Space]
        [SerializeField] private Ticker _ticker;

        private void Start()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            SoundsManager.InitPool(_soundsGroup, _musicGroup, _audioMixer);

            ServicesProvider componentProvider = new ServicesProvider(_ticker);

            SettingsSaveService settingsSaveService = new SettingsSaveService();
            componentProvider.AddService(settingsSaveService);

            SaveService saveService = new SaveService();
            componentProvider.AddService(saveService);

            SettingsService settingsService = new SettingsService(_uiHolder);
            componentProvider.AddService(settingsService);

            MainMenuService mainMenuService = new MainMenuService(_uiHolder);
            componentProvider.AddService(mainMenuService);

            componentProvider.Bind();
            componentProvider.LaunchGame();

            settingsSaveService.ApplyToSoundsManager();
        }
    }
}

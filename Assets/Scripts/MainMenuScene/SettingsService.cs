using PawnshopSimulator.Services;

namespace PawnshopSimulator.MainMenu
{
    public class SettingsService : IGameService
    {
        private readonly MainMenuUIHolder _uiHolder;
        private SettingsSaveService _settingsSaveService;

        public SettingsService(MainMenuUIHolder uiHolder)
        {
            _uiHolder = uiHolder;

            _uiHolder.MusicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
            _uiHolder.EffectsVolumeSlider.onValueChanged.AddListener(OnEffectsVolumeChanged);
            _uiHolder.BackButton.onClick.AddListener(CloseSettings);
        }

        public void Bind(ServicesProvider componentProvider)
        {
            _settingsSaveService = componentProvider.GetService<SettingsSaveService>();
        }

        public void OnLaunchGame()
        {
            _uiHolder.SettingsPanel.SetActive(false);


            _uiHolder.MusicVolumeSlider.SetValueWithoutNotify(_settingsSaveService.MusicVolume);
            _uiHolder.EffectsVolumeSlider.SetValueWithoutNotify(_settingsSaveService.EffectsVolume);
        }

        public void OpenSettings()
        {
            _uiHolder.SettingsPanel.SetActive(true);
        }

        public void CloseSettings()
        {
            _uiHolder.SettingsPanel.SetActive(false);
        }

        private void OnMusicVolumeChanged(float value)
        {
            _settingsSaveService.SetMusicVolume(value);
        }

        private void OnEffectsVolumeChanged(float value)
        {
            _settingsSaveService.SetEffectsVolume(value);
        }
    }
}

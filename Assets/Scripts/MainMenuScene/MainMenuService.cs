using UnityEngine.SceneManagement;
using PawnshopSimulator.Services;

namespace PawnshopSimulator.MainMenu
{
    public class MainMenuService : IGameService
    {
        private const string GAME_SCENE_NAME = "GameScene";

        private readonly MainMenuUIHolder _uiHolder;
        private SaveService _saveService;
        private SettingsService _settingsService;

        public MainMenuService(MainMenuUIHolder uiHolder)
        {
            _uiHolder = uiHolder;

            _uiHolder.ContinueButton.onClick.AddListener(ContinueGame);
            _uiHolder.NewGameButton.onClick.AddListener(NewGame);
            _uiHolder.SettingsButton.onClick.AddListener(OpenSettings);
            _uiHolder.QuitButton.onClick.AddListener(QuitGame);
        }

        public void Bind(ServicesProvider componentProvider)
        {
            _saveService = componentProvider.GetService<SaveService>();
            _settingsService = componentProvider.GetService<SettingsService>();


            _uiHolder.ContinueButton.interactable = _saveService.LoadedData != null;
        }

        public void OnLaunchGame() { }

        private void ContinueGame()
        {
            SceneManager.LoadScene(GAME_SCENE_NAME);
        }

        private void NewGame()
        {
            _saveService.DeleteSave();
            SceneManager.LoadScene(GAME_SCENE_NAME);
        }

        private void OpenSettings()
        {
            _settingsService.OpenSettings();
        }

        private void QuitGame()
        {
    #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
    #else
            Application.Quit();
    #endif
        }
    }
}

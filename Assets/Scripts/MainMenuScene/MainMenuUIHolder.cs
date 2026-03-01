using UnityEngine;
using UnityEngine.UI;

namespace PawnshopSimulator.MainMenu
{
    public class MainMenuUIHolder : MonoBehaviour
    {
        [field: SerializeField] public Button ContinueButton { get; private set; }
        [field: SerializeField] public Button NewGameButton { get; private set; }
        [field: SerializeField] public Button SettingsButton { get; private set; }
        [field: SerializeField] public Button QuitButton { get; private set; }

        [field: SerializeField] public GameObject SettingsPanel { get; private set; }
        [field: SerializeField] public Slider MusicVolumeSlider { get; private set; }
        [field: SerializeField] public Slider EffectsVolumeSlider { get; private set; }
        [field: SerializeField] public Button BackButton { get; private set; }
    }
}

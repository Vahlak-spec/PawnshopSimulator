using UnityEngine;
using PawnshopSimulator.Characters;
using PawnshopSimulator.UI;
using UnityEngine.SceneManagement;

namespace PawnshopSimulator.Services
{
    public class PlayerControllerService : IGameService
    {
        public Vector3 EyePos => _cameraFollowModule.EyePos;
        public Vector3 CameraDirection => _cameraFollowModule.CameraDirection;

        private const string MENU_SCENE_NAME = "MainMenuScene";

        private CharacterModulesController _playerModulesController;

        private MoveModule _playerMoveModule;
        private CameraFollowModule _cameraFollowModule;

        private InteractibleModuleBase[] _interactibleModules;

        private InputService _inputSystem;
        private GameObject _interactionUI;

        public PlayerControllerService(CharacterModulesController playerModulesController, Camera camera, UIHolder uIHolder)
        {
            _playerModulesController = playerModulesController;

            _playerMoveModule = playerModulesController.GetModule<MoveModule>();
            _cameraFollowModule = playerModulesController.GetModule<CameraFollowModule>();

            _interactibleModules = playerModulesController.GetModules<InteractibleModuleBase>();

            for (int i = 0; i < _interactibleModules.Length; i++)
            {
                _interactibleModules[i].onInteractObjectEnter = OnEnterInteraction;
                _interactibleModules[i].onInteractObjectExit = OnExitInteraction;
            }
            _cameraFollowModule.SetCamera(camera);
            _interactionUI = uIHolder.InteractionUI;
        }
        private void OnEnterInteraction()
        {
            _interactionUI.SetActive(true);

        }
        private void OnExitInteraction()
        {
            for (int i = 0; i < _interactibleModules.Length; i++)
            {
                if (_interactibleModules[i].HasInteractObject())
                    return;
            }

            _interactionUI.SetActive(false);
        }

        public void Bind(ServicesProvider componentProvider)
        {
            _inputSystem = componentProvider.GetService<InputService>();

            _inputSystem.onChangeMoveVelocity += OnChangeMoveDirection;

            _inputSystem.onChangeXMouseAxis += _cameraFollowModule.SetMouseXAxis;
            _inputSystem.onChangeYMouseAxis += _cameraFollowModule.SetMouseYAxis;

            _inputSystem.BindDownKey(KeyCode.G, TryStartInteractModules);
            _inputSystem.BindUpKey(KeyCode.G, TryStopInputMoules);

            _inputSystem.BindDownKey(KeyCode.F, TryOpenInteractMenu);
            _inputSystem.BindDownKey(KeyCode.Mouse1, TryCloseInteractMenu);

            _inputSystem.BindDownKey(KeyCode.R, TryDestroyModules);
            _inputSystem.BindDownKey(KeyCode.Escape, Exit);
        }

        private void Exit()
        {
            SceneManager.LoadScene(MENU_SCENE_NAME);
        }

        private void TryDestroyModules()
        {
            for (int i = 0; i < _interactibleModules.Length; i++)
            {
                _interactibleModules[i].TryDestroy();
            }
            OnExitInteraction();
        }

        private void TryOpenInteractMenu()
        {
            for (int i = 0; i < _interactibleModules.Length; i++)
            {
                _interactibleModules[i].OpenMenu();
            }
        }
        private void TryCloseInteractMenu()
        {
            for (int i = 0; i < _interactibleModules.Length; i++)
            {
                _interactibleModules[i].CloseMenu();
            }
        }

        private void TryStartInteractModules()
        {
            for (int i = 0; i < _interactibleModules.Length; i++)
            {
                _interactibleModules[i].TryStartInteract();
            }
        }
        private void TryStopInputMoules()
        {
            for (int i = 0; i < _interactibleModules.Length; i++)
            {
                _interactibleModules[i].TryStopInteract();
            }
        }

        public void OnLaunchGame()
        {
            _inputSystem.SetActiveCursor(false);
            _interactionUI.SetActive(false);
            _playerModulesController.Launch();
        }

        private void OnChangeMoveDirection(Vector3 direction)
        {
            if (direction == Vector3.zero)
            {
                _playerMoveModule.SetMoveDirection(Vector3.zero);
                return;
            }

            Vector3 camForward = _cameraFollowModule.CameraDirectionFlat;
            Vector3 camRight = Vector3.Cross(Vector3.up, camForward).normalized;

            Vector3 worldDirection = camForward * direction.z + camRight * direction.x;
            worldDirection.y = direction.y;

            _playerMoveModule.SetMoveDirection(worldDirection.normalized);
        }
        public void SetPlayerActivity(bool value) => _playerModulesController.SetModulesActivity(value);
    }
}

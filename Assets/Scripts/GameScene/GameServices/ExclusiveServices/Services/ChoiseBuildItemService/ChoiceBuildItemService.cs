using UnityEngine;
using PawnshopSimulator.Data;
using PawnshopSimulator.UI;

namespace PawnshopSimulator.Services
{
    public class ChoiceBuildItemService : ExclusiveServiceBase
    {
        private InputService _inputSystem;
        private ChoiceBuildItemUI _UI;

        private PlayerControllerService _playerControllerService;
        private BuildingService _buildingService;

        public ChoiceBuildItemService(BuildingServiceData buildingServiceData,RecursesData recursesData, UIHolder uiHolder)
        {
            _UI = uiHolder.ChoiceBuildItemUI;

            _UI.Bind(buildingServiceData, recursesData);
            _UI.onChoiceItem = OnChoiceBuidItem;
        }
        public override void OnLaunchGame() 
        {
            _UI.SetActiveUI(false);
        }
        public override void Bind(ServicesProvider componentProvider)
        {
            _playerControllerService = componentProvider.GetService<PlayerControllerService>();
            _inputSystem = componentProvider.GetService<InputService>();

            PoolService poolService = componentProvider.GetService<PoolService>();

            _buildingService = componentProvider.GetService<BuildingService>();

            _inputSystem.BindDownKey(KeyCode.B, OpenBuildMenu);
            _inputSystem.BindDownKey(KeyCode.Mouse1, Cancel);
        }
        public void OpenBuildMenu()
        {
            _inputSystem.SetActiveCursor(true);

            _exclusiveServicesController.OnServiceStartWork(this);

            _playerControllerService.SetPlayerActivity(false);
            _UI.SetActiveUI(true);
        }
        public override void EndServiceProcess()
        {
            _inputSystem.SetActiveCursor(false);
            _playerControllerService.SetPlayerActivity(true);
            _UI.SetActiveUI(false);
        }
        private void OnChoiceBuidItem(BuildingItemData buildingItemData)
        {
            _buildingService.ChoiceBuidItem(buildingItemData);
        }
        private void Cancel()
        {
            EndServiceProcess();
        }
    }
}

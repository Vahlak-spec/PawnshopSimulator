using UnityEngine;
using UnityEngine.Audio;
using PawnshopSimulator.Audio;
using PawnshopSimulator.Characters;
using PawnshopSimulator.Customers;
using PawnshopSimulator.Data;
using PawnshopSimulator.MainMenu;
using PawnshopSimulator.Services;
using PawnshopSimulator.UI;

namespace PawnshopSimulator
{
    public class GameSceneBinder : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private CharacterModulesController _player;
        [Space]
        [SerializeField] private Transform _poolConteiner;
        [Space]
        [SerializeField] private UIHolder _uiHolder;
        [Space]
        [SerializeField] private BuildingServiceData _buildingServiceData;
        [SerializeField] private RecursesData _recursesData;
        [SerializeField] private InventoryItemsData _inventoryItemsData;
        [SerializeField] private CraftingItemsData _craftingItemsData;
        [Space]
        [SerializeField] private CustomerBinderDataGroup _customerBinderDataGroup;
        [SerializeField] private CustomersObjectsGroup _customersObjectsGroup;
        [SerializeField] private Transform _customersStart;
        [SerializeField] private Transform _customersEnd;
        [Space]
        [SerializeField] private AudioMixer _audioMixer;
        [SerializeField] private AudioMixerGroup _soundsGroup;
        [SerializeField] private AudioMixerGroup _musicGroup;
        [Space]
        [SerializeField] private Ticker _ticker;

        void Start()
        {
            SoundsManager.InitPool(_soundsGroup, _musicGroup, _audioMixer);

            ServicesProvider componentProvider = new ServicesProvider(_ticker);

            SettingsSaveService settingsSaveService = new SettingsSaveService();
            componentProvider.AddService(settingsSaveService);
            settingsSaveService.ApplyToSoundsManager();

            SaveService saveService = new SaveService();
            componentProvider.AddService(saveService);

            PoolService poolProvider = new PoolService(_poolConteiner);
            componentProvider.AddService(poolProvider);

            InputService inputSystem = new InputService();
            componentProvider.AddService(inputSystem);

            PlayerControllerService playerControllerService = new PlayerControllerService(_player, _camera, _uiHolder);
            componentProvider.AddService(playerControllerService);

            ExclusiveServicesGroup exclusiveServicesController = new ExclusiveServicesGroup();
            componentProvider.AddService(exclusiveServicesController);

            BuildingService buildingService = new BuildingService(_buildingServiceData, _uiHolder);
            componentProvider.AddService(buildingService);
            exclusiveServicesController.AddService(buildingService);

            ChoiceBuildItemService choiceBuildItemService = new ChoiceBuildItemService(_buildingServiceData, _recursesData, _uiHolder);
            componentProvider.AddService(choiceBuildItemService);
            exclusiveServicesController.AddService(choiceBuildItemService);

            BuidRegisterService buidRegisterService = new BuidRegisterService(_buildingServiceData, _craftingItemsData, _inventoryItemsData);
            componentProvider.AddService(buidRegisterService);

            RecursesManagerService recursesManager = new RecursesManagerService(_recursesData, _uiHolder);
            componentProvider.AddService(recursesManager);

            InventoryService inventoryService = new InventoryService(_inventoryItemsData, _uiHolder);
            componentProvider.AddService(inventoryService);
            exclusiveServicesController.AddService(inventoryService);

            CraftingService craftingService = new CraftingService(_craftingItemsData, _recursesData, _uiHolder);
            componentProvider.AddService(craftingService);

            DisassemblyService disassemblyService = new DisassemblyService(_recursesData, _uiHolder);
            componentProvider.AddService(disassemblyService);

            ShowCaseService showCaseService = new ShowCaseService(_uiHolder);
            componentProvider.AddService(showCaseService);

            CashRegisterService cashRegisterService = new CashRegisterService(_uiHolder);
            componentProvider.AddService(cashRegisterService);

            CustomersControllerService customersControllerService = new CustomersControllerService(
                _customerBinderDataGroup, _customersObjectsGroup, _customersStart, _customersEnd);
            componentProvider.AddService(customersControllerService);

            componentProvider.Bind();
            componentProvider.LaunchGame();
        }
    }
}

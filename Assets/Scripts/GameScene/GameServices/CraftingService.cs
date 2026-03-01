using System.Collections.Generic;
using PawnshopSimulator.Building;
using PawnshopSimulator.Data;
using PawnshopSimulator.UI;
using static PawnshopSimulator.Building.CraftingTable;

namespace PawnshopSimulator.Services
{
    public class CraftingService : IGameService
    {
        private CraftingUI _craftingUI;
        private RecursesManagerService _recursesManager;
        private PlayerControllerService _playerControllerService;
        private InputService _inputService;
        private CraftingItemsData _craftingItemsData;
        private BuidRegisterService _buidRegisterService;

        private CraftingItemData _tempItem;
        private CraftingTable _tempCraftingTable;

        private RecursesData _recursesData;

        private List<CraftingRequest> _craftingRequests = new List<CraftingRequest>();

        public CraftingService(CraftingItemsData craftingItemsData, RecursesData recursesData, UIHolder uiHolder)
        {
            _craftingItemsData = craftingItemsData;
            _craftingUI = uiHolder.CraftingUI;
            _recursesData = recursesData;

            _craftingUI.AddToCraftListItem.onClick.AddListener(TryAddCraftItem);
            _craftingUI.HireEmployeeButton.onClick.AddListener(TryHireEmployee);
        }

        public void Bind(ServicesProvider componentProvider)
        {
            _recursesManager = componentProvider.GetService<RecursesManagerService>();
            _playerControllerService = componentProvider.GetService<PlayerControllerService>();
            _inputService = componentProvider.GetService<InputService>();
            _buidRegisterService = componentProvider.GetService<BuidRegisterService>();

            for (int i = 0; i < _craftingUI.ChoiceCraftingItemUIs.Length; i++)
                _craftingUI.ChoiceCraftingItemUIs[i].gameObject.SetActive(false);

            for (int i = 0; i < _craftingItemsData.CraftingItems.Length; i++)
            {
                _craftingUI.ChoiceCraftingItemUIs[i].gameObject.SetActive(true);
                _craftingUI.ChoiceCraftingItemUIs[i].Bind(_craftingItemsData.CraftingItems[i]);
                _craftingUI.ChoiceCraftingItemUIs[i].onChoice = OnChoiceCraftItem;
            }
        }

        public void OpenCraftingMenu(CraftingTable craftingTable, List<CraftingRequest> craftingRequests)
        {
            _tempCraftingTable = craftingTable;
            _craftingUI.SetActiveUI(true);
            _playerControllerService.SetPlayerActivity(false);
            _inputService.SetActiveCursor(true);

            if (_tempItem == null)
                OnChoiceCraftItem(_craftingItemsData.CraftingItems[0]);

            _craftingRequests = craftingRequests;
            UpdateRequestView();

            _craftingUI.HireEmployeeButton.interactable = !_tempCraftingTable.HasEmployee;
        }

        public void CloseCraftingMenu()
        {
            if (_tempCraftingTable == null) return;

            _inputService.SetActiveCursor(false);
            _craftingUI.SetActiveUI(false);
            _playerControllerService.SetPlayerActivity(true);
            _tempCraftingTable.SetCraftingRequests(_craftingRequests.ToArray());
            _tempCraftingTable = null;

            _buidRegisterService.SaveBuildings();
        }

        private void OnChoiceCraftItem(CraftingItemData item)
        {
            _tempItem = item;

            _craftingUI.RecursesGroupUI.Bind(_tempItem.RecurseNeed, _recursesData);
            _craftingUI.ChoicedItemLogo.sprite = _tempItem.ResultItem.GetSprite(InventoryItemState.Good);
            _craftingUI.ResultPriceText.text = _tempItem.ResultItem.GetPrice(InventoryItemState.Good).ToString();
            _craftingUI.ResultItemName.text = _tempItem.ResultItem.ItemName;
        }

        private void TryHireEmployee()
        {
            if (_tempCraftingTable == null) return;
            if (_tempCraftingTable.HasEmployee) return;

            if (!_recursesManager.TryPurchase(_tempCraftingTable.EmployeePrice)) return;

            _tempCraftingTable.HireEmployee();
            _craftingUI.HireEmployeeButton.interactable = false;

            _buidRegisterService.SaveBuildings();
        }

        private void TryAddCraftItem()
        {
            if (_tempItem == null) return;

            if (!_recursesManager.TryPurchase(_tempItem.RecurseNeed)) return;

            _craftingRequests.Add(new CraftingRequest(_tempItem));
            UpdateRequestView();
        }

        private void RemoveItem(CraftingRequest craftingItemData)
        {
            _craftingRequests.Remove(craftingItemData);
            _recursesManager.AddRecourses(craftingItemData.CraftingItem.RecurseNeed); 
            UpdateRequestView();
        }

        private void UpdateRequestView()
        {
            for (int i = 0; i < _craftingUI.CraftingItemResultUIs.Length; i++)
                _craftingUI.CraftingItemResultUIs[i].gameObject.SetActive(false);

            if (_craftingRequests.Count == 0) return;

            for (int i = 0; i < _craftingRequests.Count; i++)
            {
                _craftingUI.CraftingItemResultUIs[i].gameObject.SetActive(true);
                _craftingUI.CraftingItemResultUIs[i].Bind(_craftingRequests[i]);
                _craftingUI.CraftingItemResultUIs[i].onClick = RemoveItem;
            }
        }

        public void OnLaunchGame() 
        {
            _craftingUI.SetActiveUI(false);
        }
    }
}

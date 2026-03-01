using System.Collections.Generic;
using UnityEngine;
using PawnshopSimulator.Building;
using PawnshopSimulator.Data;
using PawnshopSimulator.UI;
using static PawnshopSimulator.Building.DisassemblyTable;

namespace PawnshopSimulator.Services
{
    public class DisassemblyService : IGameService
    {
        private DisassemblyUI _disassemblyUI;
        private RecursesManagerService _recursesManager;
        private PlayerControllerService _playerControllerService;
        private InputService _inputService;
        private InventoryService _inventoryService;
        private BuidRegisterService _buidRegisterService;
        private RecursesData _recursesData;

        private DisassemblyTable _tempDisassemblyTable;
        private List<DisassemblyRequest> _disassemblyRequests = new List<DisassemblyRequest>();

        public DisassemblyService(RecursesData recursesData, UIHolder uiHolder)
        {
            _recursesData = recursesData;
            _disassemblyUI = uiHolder.DisassemblyUI;

            _disassemblyUI.HireEmployeeButton.onClick.AddListener(TryHireEmployee);
        }

        public void Bind(ServicesProvider componentProvider)
        {
            _recursesManager = componentProvider.GetService<RecursesManagerService>();
            _playerControllerService = componentProvider.GetService<PlayerControllerService>();
            _inputService = componentProvider.GetService<InputService>();
            _inventoryService = componentProvider.GetService<InventoryService>();
            _buidRegisterService = componentProvider.GetService<BuidRegisterService>();

            _inputService.BindDownKey(KeyCode.Mouse1, CloseDisassemblyMenu);
        }

        public void OpenDisassemblyMenu(DisassemblyTable disassemblyTable, List<DisassemblyRequest> disassemblyRequests)
        {
            _tempDisassemblyTable = disassemblyTable;
            _disassemblyUI.SetActiveUI(true);
            _playerControllerService.SetPlayerActivity(false);
            _inputService.SetActiveCursor(true);

            _disassemblyRequests = disassemblyRequests;

            _disassemblyUI.HireEmployeeButton.interactable = !_tempDisassemblyTable.HasEmployee;

            UpdateInventoryView();
            UpdateRequestView();
        }

        public void CloseDisassemblyMenu()
        {
            if (_tempDisassemblyTable == null) return;

            _inputService.SetActiveCursor(false);
            _disassemblyUI.SetActiveUI(false);
            _playerControllerService.SetPlayerActivity(true);
            _tempDisassemblyTable.SetDisassemblyRequests(_disassemblyRequests.ToArray());
            _tempDisassemblyTable = null;


            _buidRegisterService.SaveBuildings();
        }

        private void TryHireEmployee()
        {
            if (_tempDisassemblyTable == null) return;
            if (_tempDisassemblyTable.HasEmployee) return;

            if (!_recursesManager.TryPurchase(_tempDisassemblyTable.EmployeePrice)) return;

            _tempDisassemblyTable.HireEmployee();
            _disassemblyUI.HireEmployeeButton.interactable = false;


            _buidRegisterService.SaveBuildings();
        }

        private void AddToQueue(InventoryItem item)
        {
            _inventoryService.RemoveItem(item); 
            _disassemblyRequests.Add(new DisassemblyRequest(item, _tempDisassemblyTable.DisassemblyTime));

            UpdateInventoryView();
            UpdateRequestView();

        }

        private void CancelRequest(DisassemblyRequest request)
        {
            _disassemblyRequests.Remove(request);
            _inventoryService.AddItem(request.Item); 

            UpdateInventoryView();
            UpdateRequestView();

        }

        private void UpdateInventoryView()
        {
            List<InventoryItem> items = _inventoryService.GetItems();

            for (int i = 0; i < _disassemblyUI.InventoryItemUIs.Length; i++)
                _disassemblyUI.InventoryItemUIs[i].gameObject.SetActive(false);

            for (int i = 0; i < items.Count && i < _disassemblyUI.InventoryItemUIs.Length; i++)
            {
                _disassemblyUI.InventoryItemUIs[i].gameObject.SetActive(true);
                _disassemblyUI.InventoryItemUIs[i].Bind(items[i], _recursesData);
                _disassemblyUI.InventoryItemUIs[i].onChoice = AddToQueue;
            }
        }

        private void UpdateRequestView()
        {
            for (int i = 0; i < _disassemblyUI.DisassemblyItemResultUIs.Length; i++)
                _disassemblyUI.DisassemblyItemResultUIs[i].gameObject.SetActive(false);

            for (int i = 0; i < _disassemblyRequests.Count && i < _disassemblyUI.DisassemblyItemResultUIs.Length; i++)
            {
                _disassemblyUI.DisassemblyItemResultUIs[i].gameObject.SetActive(true);
                _disassemblyUI.DisassemblyItemResultUIs[i].Bind(_disassemblyRequests[i]);
                _disassemblyUI.DisassemblyItemResultUIs[i].onClick = CancelRequest;
            }
        }

        public void OnLaunchGame()
        {
            _disassemblyUI.SetActiveUI(false);
        }
    }
}

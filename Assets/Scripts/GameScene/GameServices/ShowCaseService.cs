using System.Collections.Generic;
using UnityEngine;
using PawnshopSimulator.Building;
using PawnshopSimulator.UI;

namespace PawnshopSimulator.Services
{
    public class ShowCaseService : IGameService
    {
        private ShowCaseUI _ui;
        private InventoryService _inventoryService;
        private RecursesManagerService _recursesManager;
        private PlayerControllerService _playerController;
        private InputService _inputService;

        private ShowCase _currentShowCase;

        public ShowCaseService(UIHolder uiHolder)
        {
            _ui = uiHolder.ShowCaseUI;
            _ui.TakeButton.onClick.AddListener(TakeItem);
        }

        public void Bind(ServicesProvider componentProvider)
        {
            _inventoryService = componentProvider.GetService<InventoryService>();
            _recursesManager = componentProvider.GetService<RecursesManagerService>();
            _playerController = componentProvider.GetService<PlayerControllerService>();
            _inputService = componentProvider.GetService<InputService>();

            _inputService.BindDownKey(KeyCode.Mouse1, CloseShowCaseMenu);
        }

        public void OnLaunchGame()
        {
            _ui.SetActiveUI(false);
        }

        public void OpenShowCaseMenu(ShowCase showCase)
        {
            _currentShowCase = showCase;
            _ui.SetActiveUI(true);
            _playerController.SetPlayerActivity(false);
            _inputService.SetActiveCursor(true);

            RefreshUI();
        }

        public void CloseShowCaseMenu()
        {
            if (_currentShowCase == null) return;

            _inputService.SetActiveCursor(false);
            _ui.SetActiveUI(false);
            _playerController.SetPlayerActivity(true);
            _currentShowCase = null;
        }

        private void RefreshUI()
        {
            RefreshInventoryList();
            RefreshDisplaySlot();
        }

        private void RefreshInventoryList()
        {
            List<InventoryItem> items = _inventoryService.GetItems();

            for (int i = 0; i < _ui.InventoryItemUIs.Length; i++)
                _ui.InventoryItemUIs[i].gameObject.SetActive(false);

            for (int i = 0; i < items.Count && i < _ui.InventoryItemUIs.Length; i++)
            {
                _ui.InventoryItemUIs[i].gameObject.SetActive(true);
                _ui.InventoryItemUIs[i].Bind(items[i]);
                _ui.InventoryItemUIs[i].onChoice = PlaceItem;
            }
        }

        private void RefreshDisplaySlot()
        {
            bool hasItem = _currentShowCase.HasItem;

            if (hasItem)
                _ui.DisplayUI.Bind(_currentShowCase.ExposedItem, _currentShowCase.SellPrice);
            else
                _ui.DisplayUI.SetEmpty();

            _ui.TakeButton.interactable = hasItem;
        }


        private void PlaceItem(InventoryItem item)
        {

            if (_currentShowCase.HasItem)
                _inventoryService.AddItem(_currentShowCase.TakeItem());

            _inventoryService.RemoveItem(item);
            _currentShowCase.PlaceItem(item);

            RefreshUI();
        }


        private void TakeItem()
        {
            if (!_currentShowCase.HasItem) return;

            InventoryItem item = _currentShowCase.TakeItem();
            _inventoryService.AddItem(item);

            RefreshUI();
        }
    }
}

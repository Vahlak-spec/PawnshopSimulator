using System;
using System.Collections.Generic;
using UnityEngine;
using PawnshopSimulator.Data;
using PawnshopSimulator.UI;

namespace PawnshopSimulator.Services
{
    public class InventoryService : ExclusiveServiceBase
    {
        private List<InventoryItem> _items = new List<InventoryItem>();

        private InventoryUI _inventoryUI;
        private InputService _inputService;
        private InventoryItemsData _inventoryItemsData;
        private PlayerControllerService _playerControllerService;
        private SaveService _saveService;

        public InventoryService(InventoryItemsData inventoryItemsData, UIHolder uiHolder)
        {
            _inventoryItemsData = inventoryItemsData;
            _inventoryUI = uiHolder.InventoryUI;
        }

        public override void Bind(ServicesProvider componentProvider)
        {
            _inputService = componentProvider.GetService<InputService>();
            _playerControllerService = componentProvider.GetService<PlayerControllerService>();
            _saveService = componentProvider.GetService<SaveService>();

            _inputService.BindDownKey(KeyCode.I, OpenInventory);
            _inputService.BindDownKey(KeyCode.Mouse1, CloseInventory);
        }

        public override void EndServiceProcess()
        {
            _inventoryUI.SetActiveUI(false);
            _playerControllerService.SetPlayerActivity(true);
        }

        public override void OnLaunchGame()
        {
            _inventoryUI.SetActiveUI(false);

            if (_saveService.LoadedData == null) return;

            foreach (var saved in _saveService.LoadedData.Inventory)
            {
                InventoryItemData itemData = _inventoryItemsData.InventoryItemDatas[saved.ItemIndex];
                _items.Add(new InventoryItem(itemData, (InventoryItemState)saved.State));
            }
        }

        public void AddItem(InventoryItem inventoryItem)
        {
            _items.Add(inventoryItem);
            _saveService.SaveInventory(GetSaveData());
        }

        public void RemoveItem(InventoryItem inventoryItem)
        {
            _items.Remove(inventoryItem);
            _saveService.SaveInventory(GetSaveData());
        }

        public List<InventoryItem> GetItems() => new List<InventoryItem>(_items);

        public List<InventoryItemSaveData> GetSaveData()
        {
            var result = new List<InventoryItemSaveData>();
            foreach (var item in _items)
            {
                int index = Array.IndexOf(_inventoryItemsData.InventoryItemDatas, item.ItemData);
                result.Add(new InventoryItemSaveData { ItemIndex = index, State = (int)item.ItemState });
            }
            return result;
        }

        private void OpenInventory()
        {
            _exclusiveServicesController.OnServiceStartWork(this);

            _playerControllerService.SetPlayerActivity(false);
            _inventoryUI.SetActiveUI(true);

            for (int i = 0; i < _inventoryUI.InventoryItemUIs.Length; i++)
                _inventoryUI.InventoryItemUIs[i].gameObject.SetActive(false);

            for (int i = 0; i < _items.Count; i++)
            {
                _inventoryUI.InventoryItemUIs[i].gameObject.SetActive(true);
                _inventoryUI.InventoryItemUIs[i].Bind(_items[i]);
            }
        }

        private void CloseInventory()
        {
            _inventoryUI.SetActiveUI(false);
            _playerControllerService.SetPlayerActivity(true);
        }
    }
}

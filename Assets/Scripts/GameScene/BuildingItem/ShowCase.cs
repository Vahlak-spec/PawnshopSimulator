using System;
using UnityEngine;
using PawnshopSimulator.Data;
using PawnshopSimulator.Services;


namespace PawnshopSimulator.Building
{
    public class ShowCase : BuildebleObject
    {
        public const float SELL_PRICE_MULTIPLIER = 1.2f;

        [SerializeField] private GameObject _item;

        [field: SerializeField] public Transform CustomerInteractionPoint { get; private set; }

        public bool HasItem => _exposedItem != null;
        public InventoryItem ExposedItem => _exposedItem;
        public int SellPrice => HasItem ? Mathf.RoundToInt(_exposedItem.Price * SELL_PRICE_MULTIPLIER) : 0;

        private InventoryItem _exposedItem;

        private ShowCaseService _showCaseService;
        private InventoryService _inventoryService;
        private BuidRegisterService _buidRegisterService;
        private RecursesManagerService _recursesManager;

        public override void Bind(ServicesProvider servicesProvider)
        {
            _showCaseService = servicesProvider.GetService<ShowCaseService>();
            _inventoryService = servicesProvider.GetService<InventoryService>();
            _buidRegisterService = servicesProvider.GetService<BuidRegisterService>();
            _recursesManager = servicesProvider.GetService<RecursesManagerService>();
        }

        public void OpenMenu() => _showCaseService.OpenShowCaseMenu(this);
        public void CloseMenu() => _showCaseService.CloseShowCaseMenu();

        protected override void OnSummonBuild()
        {
            base.OnSummonBuild();
            _item.SetActive(false);
        }

        public override void TryDestroy()
        {
            if (HasItem)
            {
                _inventoryService.AddItem(_exposedItem);
                _exposedItem = null;
            }

            base.TryDestroy();
        }

        public void PlaceItem(InventoryItem item)
        {
            _item.SetActive(true);
            _exposedItem = item;
            _buidRegisterService.SaveBuildings();
        }

        public InventoryItem TakeItem()
        {
            InventoryItem item = _exposedItem;
            _exposedItem = null;
            _item.SetActive(false);
            _buidRegisterService.SaveBuildings();
            return item;
        }

        public bool TryCustomerBuy(int budget)
        {
            if (!HasItem || SellPrice > budget) return false;

            _recursesManager.AddAmount(RecursType.Money, SellPrice);
            _exposedItem = null;
            _item.SetActive(false);
            _buidRegisterService.SaveBuildings();
            return true;
        }

        public void LoadSaveData(ShowCaseSaveData data, InventoryItemsData inventoryItemsData)
        {
            if (data.ItemIndex < 0) return;
            _exposedItem = new InventoryItem(
                inventoryItemsData.InventoryItemDatas[data.ItemIndex],
                (InventoryItemState)data.ItemState);
        }

        public ShowCaseSaveData GetSaveData(InventoryItemsData inventoryItemsData)
        {
            if (!HasItem)
                return new ShowCaseSaveData { ItemIndex = -1 };

            int index = Array.IndexOf(inventoryItemsData.InventoryItemDatas, _exposedItem.ItemData);
            return new ShowCaseSaveData { ItemIndex = index, ItemState = (int)_exposedItem.ItemState };
        }
    }
}

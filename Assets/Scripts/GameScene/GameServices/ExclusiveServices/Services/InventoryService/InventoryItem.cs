using UnityEngine;
using PawnshopSimulator.Data;

namespace PawnshopSimulator.Services
{
    public class InventoryItem
    {
        public string ItemName => _itemData.ItemName;
        public int Price => _itemData.GetPrice(_itemState);
        public Sprite Sprite => _itemData.GetSprite(_itemState);
        public RecurseGroupData RecurseGroupData => _itemData.RecurseGroupData;
        public InventoryItemState ItemState => _itemState;
        public InventoryItemData ItemData => _itemData;

        private InventoryItemState _itemState;
        private InventoryItemData _itemData;

        public InventoryItem(InventoryItemData inventoryItemData, InventoryItemState itemState)
        {
            _itemData = inventoryItemData;
            _itemState = itemState;
        }
    }
}

using System;
using UnityEngine;
using PawnshopSimulator.Services;

namespace PawnshopSimulator.Data
{
    [CreateAssetMenu(fileName = "InventoryItemData", menuName = "Data/InventoryItemData")]
    public class InventoryItemData : ScriptableObject
    {
        [field: SerializeField] public string ItemName { get;private set; }
        [field: SerializeField] public InventoryItemObject Prefab { get; private set; }
        [field: SerializeField] public RecurseGroupData RecurseGroupData { get; private set; }

        [SerializeField] private ItemStateData[] _itemStateDatas;

        public Sprite GetSprite(InventoryItemState ItemState)
        {
            return Array.Find(_itemStateDatas, i => i.ItemState == ItemState).Sprite;
        }
        public int GetPrice(InventoryItemState ItemState)
        {
            return Array.Find(_itemStateDatas, i => i.ItemState == ItemState).Price;
        }
        [System.Serializable]
        private class ItemStateData
        {
            [field: SerializeField] public InventoryItemState ItemState { get; private set; }
            [field: SerializeField] public Sprite Sprite { get; private set; }
            [field: SerializeField] public int Price {  get; private set; }
        }
    }
}

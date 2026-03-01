using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using PawnshopSimulator.Services;
using static PawnshopSimulator.Building.CraftingTable;

namespace PawnshopSimulator.UI
{
    public class CraftingItemResultUI : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private TextMeshProUGUI _itemNameText;
        [SerializeField] private Image _logo;
        [SerializeField] private TextMeshProUGUI _priceText;

        public Action<CraftingRequest> onClick;

        private CraftingRequest _craftingItemData;

        public void Bind(CraftingRequest craftingItemData)
        {
            _craftingItemData = craftingItemData;

            _logo.sprite = craftingItemData.CraftingItem.ResultItem.GetSprite(InventoryItemState.Good);
            _itemNameText.text = craftingItemData.CraftingItem.ResultItem.ItemName;

            _priceText.text = craftingItemData.CraftingItem.ResultItem.GetPrice(InventoryItemState.Good).ToString();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            onClick?.Invoke(_craftingItemData);
        }
    }
}

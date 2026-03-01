using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using PawnshopSimulator.Data;
using PawnshopSimulator.Services;

namespace PawnshopSimulator.UI
{
    public class ChoiceCraftingItemUI : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private TextMeshProUGUI _priceText;
        [SerializeField] private Image _logo;

        private CraftingItemData _craftingItemData;

        public Action<CraftingItemData> onChoice;

        public void Bind(CraftingItemData craftingItemData)
        {
            _craftingItemData = craftingItemData;

            _logo.sprite = _craftingItemData.ResultItem.GetSprite(InventoryItemState.Good);
            _nameText.text = _craftingItemData.ResultItem.ItemName;

            _priceText.text = _craftingItemData.ResultItem.GetPrice(InventoryItemState.Good).ToString();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            onChoice?.Invoke(_craftingItemData);
        }
    }
}

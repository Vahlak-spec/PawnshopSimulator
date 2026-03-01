using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using PawnshopSimulator.Services;

namespace PawnshopSimulator.UI
{
    public class ShowCaseInventoryItemUI : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Image _logo;
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private TextMeshProUGUI _stateText;
        [SerializeField] private TextMeshProUGUI _priceText;

        public Action<InventoryItem> onChoice;

        private InventoryItem _item;

        public void Bind(InventoryItem item)
        {
            _item = item;
            _logo.sprite = item.Sprite;
            _nameText.text = item.ItemName;
            _stateText.text = item.ItemState.ToString();
            _priceText.text = item.Price.ToString();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            onChoice?.Invoke(_item);
        }
    }
}

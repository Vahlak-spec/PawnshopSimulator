using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using PawnshopSimulator.Data;
using PawnshopSimulator.Services;

namespace PawnshopSimulator.UI
{
    public class DisassemblyInventoryItemUI : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Image _logo;
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private TextMeshProUGUI _stateText;
        [SerializeField] private RecursesGroupUI _recursesGroupUI;

        public Action<InventoryItem> onChoice;

        private InventoryItem _item;

        public void Bind(InventoryItem item, RecursesData recursesData)
        {
            _item = item;
            _logo.sprite = item.Sprite;
            _nameText.text = item.ItemName;
            _stateText.text = item.ItemState.ToString();
            _recursesGroupUI.Bind(item.RecurseGroupData, recursesData);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            onChoice?.Invoke(_item);
        }
    }
}

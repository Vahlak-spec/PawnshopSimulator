using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using PawnshopSimulator.Building;

namespace PawnshopSimulator.UI
{
    public class DisassemblyItemResultUI : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Image _logo;
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private TextMeshProUGUI _stateText;

        public Action<DisassemblyTable.DisassemblyRequest> onClick;

        private DisassemblyTable.DisassemblyRequest _request;

        public void Bind(DisassemblyTable.DisassemblyRequest request)
        {
            _request = request;
            _logo.sprite = request.Item.Sprite;
            _nameText.text = request.Item.ItemName;
            _stateText.text = request.Item.ItemState.ToString();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            onClick?.Invoke(_request);
        }
    }
}

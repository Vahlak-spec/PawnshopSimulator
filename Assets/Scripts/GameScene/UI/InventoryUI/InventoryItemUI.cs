using UnityEngine;
using UnityEngine.UI;
using TMPro;
using PawnshopSimulator.Services;

namespace PawnshopSimulator.UI
{
    public class InventoryItemUI : MonoBehaviour
    {
        [SerializeField] private Image _logo;
        [Space]
        [SerializeField] private TextMeshProUGUI _stateText;
        [SerializeField] private TextMeshProUGUI _priceText;
        [SerializeField] private TextMeshProUGUI _nameText;

        public void Bind(InventoryItem inventoryItem)
        {
            _stateText.text = inventoryItem.ItemState.ToString();
            _priceText.text = inventoryItem.Price.ToString();
            _nameText.text = inventoryItem.ItemName.ToString();
            _logo.sprite = inventoryItem.Sprite;
        }
    }
}

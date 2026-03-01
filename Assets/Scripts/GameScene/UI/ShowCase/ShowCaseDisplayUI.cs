using UnityEngine;
using UnityEngine.UI;
using TMPro;
using PawnshopSimulator.Services;

namespace PawnshopSimulator.UI
{
    public class ShowCaseDisplayUI : MonoBehaviour
    {
        [SerializeField] private Image _logo;
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private TextMeshProUGUI _stateText;
        [SerializeField] private TextMeshProUGUI _priceText;
        [SerializeField] private Image _dolorImage;

        public void Bind(InventoryItem item, int sellPrice)
        {
            _dolorImage.gameObject.SetActive(true);
            _nameText.gameObject.SetActive(true);
            _priceText.gameObject.SetActive(true);
            _stateText.gameObject.SetActive(true);
            _logo.gameObject.SetActive(true);

            _logo.sprite = item.Sprite;
            _nameText.text = item.ItemName;
            _stateText.text = item.ItemState.ToString();
            _priceText.text = sellPrice.ToString();
        }

        public void SetEmpty()
        {
            _dolorImage.gameObject.SetActive(false);
            _nameText.gameObject.SetActive(false);
            _priceText.gameObject.SetActive(false);
            _stateText.gameObject.SetActive(false);
            _logo.gameObject.SetActive(false);
        }
    }
}

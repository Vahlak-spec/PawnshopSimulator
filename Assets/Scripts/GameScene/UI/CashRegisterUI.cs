using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace PawnshopSimulator.UI
{
    public class CashRegisterUI : MonoBehaviour
    {
        [SerializeField] private GameObject _uiRoot;

        [field: SerializeField] public Image ItemSprite { get; private set; }
        [field: SerializeField] public TextMeshProUGUI ItemNameText { get; private set; }
        [field: SerializeField] public TextMeshProUGUI ItemStateText { get; private set; }
        [field: SerializeField] public TextMeshProUGUI ItemPriceText { get; private set; }
        [field: SerializeField] public Button BuyButton { get; private set; }
        [field: SerializeField] public Button DeclineButton { get; private set; }

        public void SetActiveUI(bool value) => _uiRoot.SetActive(value);

        public void SetOfferVisible(bool value)
        {
            BuyButton.interactable = value;
            DeclineButton.interactable = value;

            ItemNameText.gameObject.SetActive(value);
            ItemPriceText.gameObject.SetActive(value);
            ItemStateText.gameObject.SetActive(value);
            ItemSprite.gameObject.SetActive(value);
        }
    }
}

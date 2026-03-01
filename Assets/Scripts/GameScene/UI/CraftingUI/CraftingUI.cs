using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace PawnshopSimulator.UI
{
    public class CraftingUI : MonoBehaviour
    {
        [SerializeField] private GameObject _uiHolder;
        [field: SerializeField] public ChoiceCraftingItemUI[] ChoiceCraftingItemUIs { get; private set; }
        [field: SerializeField] public CraftingItemResultUI[] CraftingItemResultUIs { get; private set; }
        [field: SerializeField] public Image ChoicedItemLogo { get; private set; }
        [field: SerializeField] public RecursesGroupUI RecursesGroupUI { get; private set; }
        [field: SerializeField] public TextMeshProUGUI ResultItemName { get; private set; }
        [field: SerializeField] public TextMeshProUGUI ResultPriceText { get; private set; }
        [field: SerializeField] public Button AddToCraftListItem { get; private set; }
        [field: SerializeField] public Button HireEmployeeButton { get; private set; }

        public void SetActiveUI(bool value) => _uiHolder.SetActive(value);
    }
}

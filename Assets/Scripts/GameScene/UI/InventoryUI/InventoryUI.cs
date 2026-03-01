using UnityEngine;

namespace PawnshopSimulator.UI
{
    public class InventoryUI : MonoBehaviour
    {
        [SerializeField] private GameObject _uiHolder;
        [field: SerializeField] public InventoryItemUI[] InventoryItemUIs {  get; private set; }

        public void SetActiveUI(bool value)
        {
            _uiHolder.SetActive(value);
        }
    }
}

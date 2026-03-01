using UnityEngine;
using UnityEngine.UI;

namespace PawnshopSimulator.UI
{
    public class DisassemblyUI : MonoBehaviour
    {
        [SerializeField] private GameObject _uiHolder;
        [field: SerializeField] public DisassemblyInventoryItemUI[] InventoryItemUIs { get; private set; }
        [field: SerializeField] public DisassemblyItemResultUI[] DisassemblyItemResultUIs { get; private set; }
        [field: SerializeField] public Button HireEmployeeButton { get; private set; }

        public void SetActiveUI(bool value) => _uiHolder.SetActive(value);
    }
}

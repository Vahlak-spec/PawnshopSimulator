using UnityEngine;

namespace PawnshopSimulator.UI
{
    public class UIHolder : MonoBehaviour
    {
        [field: SerializeField] public ChoiceBuildItemUI ChoiceBuildItemUI { get; private set; }
        [field: SerializeField] public BuildingUI BuildingUI { get; private set; }
        [field: SerializeField] public RecursesUI RecursesUI { get; private set; }
        [field: SerializeField] public InventoryUI InventoryUI { get; private set; }
        [field: SerializeField] public CraftingUI CraftingUI { get; private set; }
        [field: SerializeField] public DisassemblyUI DisassemblyUI { get; private set; }
        [field: SerializeField] public CashRegisterUI CashRegisterUI { get; private set; }
        [field: SerializeField] public GameObject InteractionUI { get; private set; }
        [field: SerializeField] public ShowCaseUI ShowCaseUI { get; private set; }
    }
}

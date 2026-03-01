using UnityEngine;

namespace PawnshopSimulator.Data
{
    [CreateAssetMenu(fileName = "CraftingItemData", menuName = "Data/CraftingItemData")]
    public class CraftingItemData : ScriptableObject
    {
        [field: SerializeField] public InventoryItemData ResultItem;
        [field: SerializeField] public RecurseGroupData RecurseNeed;
        [field: SerializeField] public float CraftingTime;
    }
}

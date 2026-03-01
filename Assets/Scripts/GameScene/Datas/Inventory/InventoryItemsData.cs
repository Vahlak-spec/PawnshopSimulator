using UnityEngine;

namespace PawnshopSimulator.Data
{
    [CreateAssetMenu(fileName = "InventoryItemsData", menuName = "Data/InventoryItemsData")]
    public class InventoryItemsData : ScriptableObject
    {
        [field: SerializeField] public InventoryItemData[] InventoryItemDatas {  get; private set; }
    }
}

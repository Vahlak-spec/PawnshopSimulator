using UnityEngine;


namespace PawnshopSimulator.Data
{
    [CreateAssetMenu(fileName = "CraftingItemsData", menuName = "Data/CraftingItemsData")]
    public class CraftingItemsData : ScriptableObject
    {
        [field: SerializeField] public CraftingItemData[] CraftingItems {  get; private set; }
    }
}

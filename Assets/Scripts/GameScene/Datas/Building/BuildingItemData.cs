using UnityEngine;
using PawnshopSimulator.Building;

namespace PawnshopSimulator.Data
{
    [CreateAssetMenu(fileName = "BuildingItemData", menuName = "Data/BuildingItemData")]
    public class BuildingItemData : ScriptableObject
    {
        [field: SerializeField] public string ItemName { get; private set; }
        [field: SerializeField] public BuildebleObject Prefab {  get; private set; }
        [field: SerializeField] public RecurseGroupData Price { get; private set; }
        [field: SerializeField] public Sprite Logo { get; private set; }
    }
}

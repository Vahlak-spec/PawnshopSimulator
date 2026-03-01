using UnityEngine;

namespace PawnshopSimulator.Data
{
    [CreateAssetMenu(fileName = "BuildingServiceData", menuName = "Data/BuildingServiceData")]
    public class BuildingServiceData : ScriptableObject
    {
        [field: SerializeField] public BuildingItemData[] Items { get; private set; }

    }
}

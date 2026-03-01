using UnityEngine;
using PawnshopSimulator.Characters;


namespace PawnshopSimulator.Data
{
    [CreateAssetMenu(fileName = "CustomersObjectsGroup", menuName = "Data/CustomersObjectsGroup")]
    public class CustomersObjectsGroup : ScriptableObject
    {
        [field: SerializeField] public CharacterModulesController[] Prefabs {  get; private set; }
    }
}

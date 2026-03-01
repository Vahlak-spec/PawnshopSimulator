using UnityEngine;


namespace PawnshopSimulator.Data
{
    [CreateAssetMenu(fileName = "CustomersData", menuName = "Data/CustomersData")]
    public class CustomerBinderDataGroup : ScriptableObject
    {
        [field: SerializeField] public CustomerBinderBase[] CustomerBinders {  get; private set; }
    }
}

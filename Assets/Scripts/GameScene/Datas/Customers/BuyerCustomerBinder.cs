using UnityEngine;
using PawnshopSimulator.Customers;


namespace PawnshopSimulator.Data
{
    [CreateAssetMenu(fileName = "BuyerCustomerBinder", menuName = "Data/BuyerCustomerBinder")]
    public class BuyerCustomerBinder : CustomerBinderBase
    {
        [SerializeField] private int _budget;
        [SerializeField] private float _searchRadius = 20f;

        public override CustomerBase BindCustomer()
        {
            return new BuyerCustomer(_budget, _searchRadius);
        }
    }
}

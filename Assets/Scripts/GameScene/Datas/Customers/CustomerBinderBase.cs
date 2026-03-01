using UnityEngine;
using PawnshopSimulator.Customers;


namespace PawnshopSimulator.Data
{
    public abstract class CustomerBinderBase : ScriptableObject
    {
        public abstract CustomerBase BindCustomer();
    }
}

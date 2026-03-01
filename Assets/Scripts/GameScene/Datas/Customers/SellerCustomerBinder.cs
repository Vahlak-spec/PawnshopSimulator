using UnityEngine;
using PawnshopSimulator.Customers;
using PawnshopSimulator.Services;

namespace PawnshopSimulator.Data
{
    [CreateAssetMenu(fileName = "SellerCustomerBinder", menuName = "Data/SellerCustomerBinder")]
    public class SellerCustomerBinder : CustomerBinderBase
    {
        [SerializeField] private InventoryItemData _inventoryItemData;
        [SerializeField] private InventoryItemState _itemState = InventoryItemState.Bad;
        [SerializeField] private float _searchRadius = 20f;

        public override CustomerBase BindCustomer()
        {
            InventoryItem item = new InventoryItem(_inventoryItemData, _itemState);
            return new SellerCustomer(item, _searchRadius);
        }
    }
}

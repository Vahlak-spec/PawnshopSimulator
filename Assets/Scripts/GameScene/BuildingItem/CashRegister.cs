using System;
using UnityEngine;
using PawnshopSimulator.Services;

namespace PawnshopSimulator.Building
{
    public class CashRegister : BuildebleObject
    {
        [field: SerializeField] public Transform CustomerInteractionPoint { get; private set; }

        public bool IsOccupied => _currentOffer != null;

        private InventoryItem _currentOffer;
        private Action<bool> _offerCallback;
        private CashRegisterService _cashRegisterService;

        public override void Bind(ServicesProvider servicesProvider)
        {
            base.Bind(servicesProvider);
            _cashRegisterService = servicesProvider.GetService<CashRegisterService>();
        }

        public void OpenMenu()
        {
            _cashRegisterService.OpenCashRegisterMenu(this);
        }

        public void CloseMenu()
        {
            _cashRegisterService.CloseCashRegisterMenu();
        }

        public bool TrySetOffer(InventoryItem item, Action<bool> onResolved)
        {
            if (IsOccupied) return false;

            _currentOffer = item;
            _offerCallback = onResolved;
            return true;
        }

        public InventoryItem GetCurrentOffer() => _currentOffer;

        public void AcceptOffer()
        {
            var callback = _offerCallback;
            ClearOffer();
            callback?.Invoke(true);
        }

        public void DeclineOffer()
        {
            var callback = _offerCallback;
            ClearOffer();
            callback?.Invoke(false);
        }

        public void CancelOffer()
        {
            ClearOffer();
        }

        private void ClearOffer()
        {
            _currentOffer = null;
            _offerCallback = null;
        }
    }
}

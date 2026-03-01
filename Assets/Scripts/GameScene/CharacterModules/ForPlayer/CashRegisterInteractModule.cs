using UnityEngine;
using PawnshopSimulator.Building;

namespace PawnshopSimulator.Characters
{
    public class CashRegisterInteractModule : InteractibleModuleBase
    {
        [SerializeField] private TriggerCollider _triggerCollider;

        private CashRegister _tempRegister;
        private bool _isActive;

        public override void Launch()
        {
            _triggerCollider.SetEnterAction<CashRegister>(OnEnter);
            _triggerCollider.SetExitAction<CashRegister>(OnExit);

            _isActive = true;
        }

        public override void SetModuleActive(bool value)
        {
            _isActive = value;
        }

        public override void TryStartInteract() { }
        public override void TryStopInteract() { }

        public override void OpenMenu()
        {
            if (!_isActive || _tempRegister == null) return;
            _tempRegister.OpenMenu();
        }

        public override void CloseMenu()
        {
            if (_tempRegister == null) return;
            _tempRegister.CloseMenu();
        }

        public override void TryDestroy()
        {
            if (!_isActive || _tempRegister == null) return;
            _tempRegister.CloseMenu();
            _tempRegister.TryDestroy();
            _tempRegister = null;
        }

        private void OnEnter(GameObject obj)
        {
            CashRegister cashRegister = obj.GetComponent<CashRegister>();

            if(!cashRegister.IsInteract)return;

            _tempRegister = cashRegister;
            onInteractObjectEnter?.Invoke();
        }

        private void OnExit(GameObject obj)
        {
            if (obj.GetComponent<CashRegister>() == _tempRegister)
            {
                _tempRegister.CloseMenu();
                _tempRegister = null;
                onInteractObjectExit?.Invoke();
            }
        }

        public override bool HasInteractObject()
        {
            return _tempRegister != null;
        }
    }
}

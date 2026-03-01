using UnityEngine;
using PawnshopSimulator.Building;


namespace PawnshopSimulator.Characters
{
    public class CraftingTableInteractModule : InteractibleModuleBase
    {
        [SerializeField] private TriggerCollider _triggerCollider;

        private CraftingTable _tempTable;

        private bool _isActive;

        public override void Launch()
        {
            _triggerCollider.SetEnterAction<CraftingTable>(TableEnter);
            _triggerCollider.SetExitAction<CraftingTable>(TableExit);

            _isActive = true;
        }
        public override void SetModuleActive(bool value)
        {
            _isActive = value;

            if (!_isActive)
            {
                TryStopInteract();
            }
        }

        public override void TryStartInteract()
        {
            if (!_isActive) return;
            if (_tempTable == null) return;

            _tempTable.StartInteract();
        }
        public override void TryStopInteract()
        {
            if (!_isActive) return;
            if (_tempTable == null) return;

            _tempTable.StopInteract();
        }
        public override void OpenMenu()
        {
            if (!_isActive) return;
            if (_tempTable == null) return;

            _tempTable.OpenMenu();
        }
        public override void TryDestroy()
        {
            if (!_isActive) return;
            if (_tempTable == null) return;

            _tempTable.CloseMenu();
            _tempTable.TryDestroy();
            _tempTable = null;
        }
        private void TableEnter(GameObject table)
        {
            CraftingTable newTable = table.GetComponent<CraftingTable>();

            if(!newTable.IsInteract)return;

            _tempTable = newTable;
            onInteractObjectEnter?.Invoke();
        }
        private void TableExit(GameObject table)
        {
            CraftingTable exidTable = table.GetComponent<CraftingTable>();

            if (exidTable == _tempTable)
            {
                _tempTable.StopInteract();
                _tempTable.CloseMenu();
                _tempTable = null;
                onInteractObjectExit?.Invoke();
            }
        }

        public override void CloseMenu()
        {
            if (_tempTable == null) return;
            _tempTable.CloseMenu();
        }

        public override bool HasInteractObject()
        {
            return _tempTable != null;
        }
    }
}

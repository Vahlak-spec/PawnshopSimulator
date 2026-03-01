using UnityEngine;
using PawnshopSimulator.Building;


namespace PawnshopSimulator.Characters
{
    public class DisassemblyTableInteractModule : InteractibleModuleBase
    {
        [SerializeField] private TriggerCollider _triggerCollider;

        private DisassemblyTable _tempTable;
        private bool _isActive;

        public override void Launch()
        {
            _triggerCollider.SetEnterAction<DisassemblyTable>(TableEnter);
            _triggerCollider.SetExitAction<DisassemblyTable>(TableExit);

            _isActive = true;
        }

        public override void SetModuleActive(bool value)
        {
            _isActive = value;

            if (!_isActive)
                TryStopInteract();
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

        public override void CloseMenu()
        {
            if (_tempTable == null) return;
            _tempTable.CloseMenu();
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
            DisassemblyTable newTable = table.GetComponent<DisassemblyTable>();

            if (!newTable.IsInteract) return;

            _tempTable = newTable;
            onInteractObjectEnter?.Invoke();
        }

        private void TableExit(GameObject table)
        {
            DisassemblyTable exitedTable = table.GetComponent<DisassemblyTable>();

            if (exitedTable == _tempTable)
            {
                _tempTable.StopInteract();
                _tempTable.CloseMenu();
                _tempTable = null;
                onInteractObjectExit?.Invoke();
            }
        }

        public override bool HasInteractObject()
        {
            return _tempTable != null;
        }
    }
}

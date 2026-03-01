using UnityEngine;
using PawnshopSimulator.Building;


namespace PawnshopSimulator.Characters
{
    public class ShowCaseInteractModule : InteractibleModuleBase
    {
        [SerializeField] private TriggerCollider _triggerCollider;

        private ShowCase _tempShowCase;
        private bool _isActive;

        public override void Launch()
        {
            _triggerCollider.SetEnterAction<ShowCase>(OnEnter);
            _triggerCollider.SetExitAction<ShowCase>(OnExit);
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
            if (!_isActive || _tempShowCase == null) return;
            _tempShowCase.OpenMenu();
        }

        public override void CloseMenu()
        {
            if (_tempShowCase == null) return;
            _tempShowCase.CloseMenu();
        }

        public override void TryDestroy()
        {
            if (!_isActive || _tempShowCase == null) return;
            _tempShowCase.CloseMenu();
            _tempShowCase.TryDestroy();
            _tempShowCase = null;
        }

        private void OnEnter(GameObject obj)
        {
            _tempShowCase = obj.GetComponent<ShowCase>();
            onInteractObjectEnter?.Invoke();
        }

        private void OnExit(GameObject obj)
        {
            if (obj.GetComponent<ShowCase>() == _tempShowCase)
            {
                _tempShowCase.CloseMenu();
                _tempShowCase = null;
                onInteractObjectExit?.Invoke();
            }
        }

        public override bool HasInteractObject()
        {
            return _tempShowCase != null;
        }
    }
}

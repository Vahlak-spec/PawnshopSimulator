using UnityEngine;
using PawnshopSimulator.Building;


namespace PawnshopSimulator.Characters
{
    public class CustomerBuyModule : CharacterModuleBase
    {
        [SerializeField] private TriggerCollider _triggerCollider;

        private ShowCase _tempShowCase;

        public override void Launch()
        {
            _triggerCollider.SetEnterAction<ShowCase>(EnterShowcase);
            _triggerCollider.SetExitAction<ShowCase>(ExitShowcase);
        }

        public override void SetModuleActive(bool value) { }

        public bool TryBuy(int budget)
        {
            return false;
        }

        private void EnterShowcase(GameObject gameObject)
        {
            _tempShowCase = gameObject.GetComponent<ShowCase>();
        }
        private void ExitShowcase(GameObject gameObject)
        {
            if(gameObject.GetComponent<ShowCase>() == _tempShowCase)
                _tempShowCase = null;
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PawnshopSimulator.Building;
using PawnshopSimulator.Characters;
using PawnshopSimulator.Services;

namespace PawnshopSimulator.Customers
{
    public class BuyerCustomer : CustomerBase
    {
        private const float MAX_SHOWCASES_TO_VISIT = 3;

        private NavMeshMoveModule _moveModule;
        private CustomerBuyModule _buyModule;
        private GameObject _characterGO;

        private Transform _start;
        private Transform _end;
        private Ticker _ticker;
        private BuidRegisterService _buidRegisterService;

        private readonly int _budget;
        private readonly float _searchRadius;

        public BuyerCustomer(int budget, float searchRadius)
        {
            _budget = budget;
            _searchRadius = searchRadius;
        }

        public override void Bind(
            CharacterModulesController characterController,
            Ticker ticker,
            Transform start,
            Transform end,
            BuidRegisterService buidRegisterService)
        {
            _moveModule = characterController.GetModule<NavMeshMoveModule>();
            _buyModule = characterController.GetModule<CustomerBuyModule>();
            _characterGO = characterController.gameObject;

            _characterGO.transform.position = start.position;

            _ticker = ticker;
            _start = start;
            _end = end;
            _buidRegisterService = buidRegisterService;
        }

        public override void LaunchProcces(Action<CustomerBase> onComplete)
        {
            _ticker.StartCoroutine(Procces(onComplete));
        }

        private IEnumerator Procces(Action<CustomerBase> onComplete)
        {
            List<ShowCase> showcases = PickRandomShowCases();

            foreach (ShowCase showCase in showcases)
            {
                _moveModule.SetDestination(showCase.CustomerInteractionPoint.position);
                yield return new WaitUntil(() => _moveModule.HasReachedDestination);

                if (!showCase.HasItem) continue;
                if (showCase.SellPrice > _budget) continue;

                if (showCase.TryCustomerBuy(_budget))
                    break;
                break;
            }

            _moveModule.SetDestination(_end.position);
            yield return new WaitUntil(() => _moveModule.HasReachedDestination);

            _characterGO.SetActive(false);
            onComplete?.Invoke(this);
        }

        private List<ShowCase> PickRandomShowCases()
        {
            List<ShowCase> all = _buidRegisterService.GetBuildings<ShowCase>();
            List<ShowCase> inRadius = new List<ShowCase>();

            foreach (var s in all)
            {
                if (Vector3.Distance(_characterGO.transform.position, s.Transform.position) <= _searchRadius)
                    inRadius.Add(s);
            }


            for (int i = inRadius.Count - 1; i > 0; i--)
            {
                int j = UnityEngine.Random.Range(0, i + 1);
                (inRadius[i], inRadius[j]) = (inRadius[j], inRadius[i]);
            }

            int count = Mathf.Min(inRadius.Count, (int)MAX_SHOWCASES_TO_VISIT);
            return inRadius.GetRange(0, count);
        }
    }
}

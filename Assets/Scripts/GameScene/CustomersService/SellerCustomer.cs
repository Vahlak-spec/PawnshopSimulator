using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PawnshopSimulator.Building;
using PawnshopSimulator.Characters;
using PawnshopSimulator.Services;

namespace PawnshopSimulator.Customers
{
    public class SellerCustomer : CustomerBase
    {
        private const float REGISTER_CHECK_INTERVAL = 0.2f;
        private const float NO_REGISTER_WAIT_TIME = 10f;
        private const float NO_REGISTER_CHECK_INTERVAL = 1f;

        private NavMeshMoveModule _moveModule;
        private CustomerSellModule _sellModule;
        private Transform _characterTransform;

        private Transform _start;
        private Transform _end;
        private Ticker _ticker;
        private BuidRegisterService _buidRegisterService;

        private readonly InventoryItem _inventoryItem;
        private readonly float _searchRadius;

        private bool _offerResolved;

        public SellerCustomer(InventoryItem inventoryItem, float searchRadius)
        {
            _inventoryItem = inventoryItem;
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
            _sellModule = characterController.GetModule<CustomerSellModule>();
            _characterTransform = characterController.transform;

            _ticker = ticker;
            _start = start;
            _end = end;
            _buidRegisterService = buidRegisterService;

            _characterTransform.position = _start.position;
        }

        public override void LaunchProcces(Action<CustomerBase> onComplete)
        {
            _ticker.StartCoroutine(Procces(onComplete));
        }

        private IEnumerator Procces(Action<CustomerBase> onComplete)
        {
            CashRegister chosenRegister = null;

            while (true)
            {
                chosenRegister = FindFreeRegister();

                if (chosenRegister == null)
                {
                    float waited = 0f;
                    while (waited < NO_REGISTER_WAIT_TIME)
                    {
                        yield return new WaitForSeconds(NO_REGISTER_CHECK_INTERVAL);
                        waited += NO_REGISTER_CHECK_INTERVAL;

                        chosenRegister = FindFreeRegister();
                        if (chosenRegister != null) break;
                    }

                    if (chosenRegister == null)
                    {
                        yield return WalkTo(_end.position);
                        onComplete?.Invoke(this);
                        yield break;
                    }
                }

                _moveModule.SetDestination(chosenRegister.CustomerInteractionPoint.position);

                bool interrupted = false;
                float checkTimer = 0f;

                while (!_moveModule.HasReachedDestination)
                {
                    checkTimer += Time.deltaTime;
                    if (checkTimer >= REGISTER_CHECK_INTERVAL)
                    {
                        checkTimer = 0f;
                        if (chosenRegister.IsOccupied)
                        {
                            _moveModule.Stop();
                            interrupted = true;
                            break;
                        }
                    }
                    yield return new WaitForEndOfFrame();
                }

                if (interrupted)
                    continue;

                _moveModule.Stop();
                _offerResolved = false;
                bool claimed = _sellModule.TrySellToRegister(chosenRegister, _inventoryItem, OnOfferResolved);

                if (!claimed)
                {
                    continue;
                }

                yield return new WaitUntil(() => _offerResolved);
                break;
            }

            yield return WalkTo(_end.position);
            onComplete?.Invoke(this);
        }

        private IEnumerator WalkTo(Vector3 target)
        {
            _moveModule.SetDestination(target);
            yield return new WaitUntil(() => _moveModule.HasReachedDestination);
        }

        private CashRegister FindFreeRegister()
        {
            List<CashRegister> all = _buidRegisterService.GetBuildings<CashRegister>();
            List<CashRegister> candidates = new List<CashRegister>();

            foreach (var r in all)
            {
                if (!r.IsOccupied &&
                    Vector3.Distance(_characterTransform.position, r.Transform.position) <= _searchRadius)
                {
                    candidates.Add(r);
                }
            }

            if (candidates.Count == 0) return null;
            return candidates[UnityEngine.Random.Range(0, candidates.Count)];
        }

        private void OnOfferResolved(bool accepted)
        {
            _offerResolved = true;
        }
    }
}

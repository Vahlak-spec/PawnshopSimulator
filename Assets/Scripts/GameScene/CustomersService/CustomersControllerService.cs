using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PawnshopSimulator.Characters;
using PawnshopSimulator.Data;
using PawnshopSimulator.Services;

namespace PawnshopSimulator.Customers
{
    public class CustomersControllerService : IGameService
    {
        private CustomerBinderDataGroup _customerBinderDataGroup;
        private CustomersObjectsGroup _customersObjectsGroup;

        private Transform _start;
        private Transform _end;

        private PoolService _poolService;
        private BuidRegisterService _buidRegisterService;
        private Ticker _ticker;

        private List<CustomerBase> _customers = new List<CustomerBase>();
        private PoolComponent<CharacterModulesController>[] _characterObjPools;

        public CustomersControllerService(
            CustomerBinderDataGroup customerBinderDataGroup,
            CustomersObjectsGroup customersObjectsGroup,
            Transform start,
            Transform end)
        {
            _customerBinderDataGroup = customerBinderDataGroup;
            _customersObjectsGroup = customersObjectsGroup;
            _start = start;
            _end = end;
        }

        public void Bind(ServicesProvider componentProvider)
        {
            _poolService = componentProvider.GetService<PoolService>();
            _buidRegisterService = componentProvider.GetService<BuidRegisterService>();
            _ticker = componentProvider.Ticker;

            _characterObjPools = new PoolComponent<CharacterModulesController>[_customersObjectsGroup.Prefabs.Length];
            for (int i = 0; i < _characterObjPools.Length; i++)
                _characterObjPools[i] = _poolService.CreatePool(_customersObjectsGroup.Prefabs[i], 10);
        }

        public void OnLaunchGame()
        {
            _ticker.StartCoroutine(SpawnLoop());
        }

        private IEnumerator SpawnLoop()
        {
            while (true)
            {
                SpawnCustomer();
                yield return new WaitForSeconds(30f);
            }
        }

        private void SpawnCustomer()
        {
            CustomerBinderBase binder = _customerBinderDataGroup
                .CustomerBinders[UnityEngine.Random.Range(0, _customerBinderDataGroup.CustomerBinders.Length)];

            CustomerBase customer = binder.BindCustomer();

            CharacterModulesController characterObj =
                _characterObjPools[UnityEngine.Random.Range(0, _characterObjPools.Length)].GetFreeElement();

            customer.Bind(characterObj, _ticker, _start, _end, _buidRegisterService);
            customer.LaunchProcces(OnCustomerComplete);

            _customers.Add(customer);
        }

        private void OnCustomerComplete(CustomerBase customer)
        {
            _customers.Remove(customer);
        }
    }
}

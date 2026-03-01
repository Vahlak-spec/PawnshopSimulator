using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PawnshopSimulator.Data;
using PawnshopSimulator.Services;

namespace PawnshopSimulator.Building
{
    public class DisassemblyTable : BuildebleObject
    {
        public DisassemblyRequest[] DisassemblyRequests => _disassemblyRequests.ToArray();
        public bool HasEmployee => _hasEmployee;
        public RecurseGroupData EmployeePrice => _employeePrice;
        public float DisassemblyTime => _disassemblyTime;

        [SerializeField] private Image _disassemblyFill;
        [SerializeField] private float _disassemblyTime = 10f;
        [SerializeField] private RecurseGroupData _employeePrice;
        [Space]
        [SerializeField] private GameObject _employer;

        private DisassemblyService _disassemblyService;
        private InventoryService _inventoryService;
        private RecursesManagerService _recursesManagerService;
        private List<DisassemblyRequest> _disassemblyRequests = new List<DisassemblyRequest>();

        private Coroutine _procces;
        private bool _hasEmployee;

        protected override void OnSummonBuild()
        {
            _disassemblyFill.fillAmount = 0;
            _employer.SetActive(false);
        }

        public override void Bind(ServicesProvider servicesProvider)
        {
            base.Bind(servicesProvider);

            _inventoryService = servicesProvider.GetService<InventoryService>();
            _disassemblyService = servicesProvider.GetService<DisassemblyService>();
            _recursesManagerService = servicesProvider.GetService<RecursesManagerService>();

            _disassemblyFill.fillAmount = 0;
        }

        public void StartInteract()
        {
            if (_procces != null) return;
            _procces = StartCoroutine(Procces());
        }

        public void StopInteract()
        {
            if (_hasEmployee) return;

            if (_procces != null)
            {
                StopCoroutine(_procces);
                _procces = null;
            }
        }

        public void OpenMenu()
        {
            if (_disassemblyService == null) return;
            _disassemblyService.OpenDisassemblyMenu(this, _disassemblyRequests);
        }

        public void CloseMenu()
        {
            if (_disassemblyService == null) return;
            _disassemblyService.CloseDisassemblyMenu();
        }

        public void SetDisassemblyRequests(DisassemblyRequest[] requests)
        {
            _disassemblyRequests.Clear();

            for (int i = 0; i < requests.Length; i++)
                _disassemblyRequests.Add(requests[i]);

            if (_hasEmployee && _procces == null && _disassemblyRequests.Count > 0)
                _procces = StartCoroutine(Procces());
        }

        public void HireEmployee()
        {
            _hasEmployee = true;
            _employer.SetActive(true);

            if (_procces == null && _disassemblyRequests.Count > 0)
                _procces = StartCoroutine(Procces());
        }

        public override void TryDestroy()
        {
            StopInteract();

            for (int i = 0; i < _disassemblyRequests.Count; i++)
                _inventoryService.AddItem(_disassemblyRequests[i].Item);

            _disassemblyRequests.Clear();

            if (_hasEmployee)
                RecursesManager.AddRecourses(_employeePrice);

            base.TryDestroy();
        }

        public List<DisassemblyRequestSaveData> GetDisassemblyQueueSaveData(InventoryItemsData inventoryItemsData)
        {
            var result = new List<DisassemblyRequestSaveData>();
            foreach (var req in _disassemblyRequests)
            {
                int index = Array.IndexOf(inventoryItemsData.InventoryItemDatas, req.Item.ItemData);
                result.Add(new DisassemblyRequestSaveData
                {
                    InventoryItemIndex = index,
                    ItemState = (int)req.Item.ItemState,
                    TimeLeft = req.TimeLeft
                });
            }
            return result;
        }

        public void LoadSaveData(BuildingSaveData data, InventoryItemsData inventoryItemsData)
        {
            _hasEmployee = data.HasEmployee;
            _employer.SetActive(_hasEmployee);

            foreach (var reqData in data.DisassemblyQueue)
            {
                InventoryItemData itemData = inventoryItemsData.InventoryItemDatas[reqData.InventoryItemIndex];
                InventoryItem item = new InventoryItem(itemData, (InventoryItemState)reqData.ItemState);
                _disassemblyRequests.Add(new DisassemblyRequest(item, _disassemblyTime, reqData.TimeLeft));
            }

            if (_hasEmployee && _disassemblyRequests.Count > 0)
                _procces = StartCoroutine(Procces());
        }

        private IEnumerator Procces()
        {
            while (_disassemblyRequests.Count > 0)
            {
                _disassemblyRequests[0].DisassemblyTick();
                _disassemblyFill.fillAmount = _disassemblyRequests[0].T;

                if (_disassemblyRequests[0].IsReady)
                {
                    DisassemblyRequest completed = _disassemblyRequests[0];
                    _disassemblyRequests.RemoveAt(0);
                    _recursesManagerService.AddRecourses(completed.Item.RecurseGroupData);
                    ServicesProvider.GetService<BuidRegisterService>().SaveBuildings();
                }

                yield return new WaitForEndOfFrame();
            }

            _disassemblyFill.fillAmount = 0;
            _procces = null;
        }

        public class DisassemblyRequest
        {
            public InventoryItem Item => _item;
            public bool IsReady => _timeLeft <= 0;
            public float T => _timeLeft / _allTime;
            public float TimeLeft => _timeLeft;

            private InventoryItem _item;
            private float _timeLeft;
            private float _allTime;

            public DisassemblyRequest(InventoryItem item, float disassemblyTime)
            {
                _item = item;
                _timeLeft = disassemblyTime;
                _allTime = disassemblyTime;
            }

            public DisassemblyRequest(InventoryItem item, float disassemblyTime, float timeLeft)
            {
                _item = item;
                _timeLeft = timeLeft;
                _allTime = disassemblyTime;
            }

            public void DisassemblyTick()
            {
                _timeLeft -= Time.deltaTime;
            }
        }
    }
}

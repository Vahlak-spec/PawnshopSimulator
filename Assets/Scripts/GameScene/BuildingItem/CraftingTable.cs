using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PawnshopSimulator.Data;
using PawnshopSimulator.Services;

namespace PawnshopSimulator.Building
{
    public class CraftingTable : BuildebleObject
    {
        public CraftingRequest[] CraftingRequests => _craftingRequests.ToArray();
        public bool HasEmployee => _hasEmployee;
        public RecurseGroupData EmployeePrice => _employeePrice;

        [SerializeField] private Image _craftingFill;
        [SerializeField] private RecurseGroupData _employeePrice;
        [Space]
        [SerializeField] private GameObject _employer;

        private CraftingService _craftingService;
        private InventoryService _inventoryService;
        private List<CraftingRequest> _craftingRequests = new List<CraftingRequest>();

        private Coroutine _procces;
        private bool _hasEmployee;

        protected override void OnSummonBuild()
        {
            _craftingFill.fillAmount = 0;
            _employer.SetActive(false);
        }

        public override void Bind(ServicesProvider servicesProvider)
        {
            base.Bind(servicesProvider);
            _inventoryService = servicesProvider.GetService<InventoryService>();
            _craftingService = servicesProvider.GetService<CraftingService>();
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
            _craftingService.OpenCraftingMenu(this, _craftingRequests);
        }

        public void CloseMenu()
        {
            _craftingService.CloseCraftingMenu();
        }

        public void SetCraftingRequests(CraftingRequest[] craftingRequests)
        {
            _craftingRequests.Clear();

            for (int i = 0; i < craftingRequests.Length; i++)
                _craftingRequests.Add(craftingRequests[i]);

            if (_hasEmployee && _procces == null && _craftingRequests.Count > 0)
                _procces = StartCoroutine(Procces());
        }

        public void HireEmployee()
        {
            _hasEmployee = true;
            _employer.SetActive(true);

            if (_procces == null && _craftingRequests.Count > 0)
                _procces = StartCoroutine(Procces());
        }

        public override void TryDestroy()
        {
            StopInteract();

            for (int i = 0; i < _craftingRequests.Count; i++)
                RecursesManager.AddRecourses(_craftingRequests[i].CraftingItem.RecurseNeed);

            _craftingRequests.Clear();

            if (_hasEmployee)
                RecursesManager.AddRecourses(_employeePrice);

            base.TryDestroy();
        }

        public List<CraftingRequestSaveData> GetCraftingQueueSaveData(CraftingItemsData craftingItemsData)
        {
            var result = new List<CraftingRequestSaveData>();
            foreach (var req in _craftingRequests)
            {
                int index = Array.IndexOf(craftingItemsData.CraftingItems, req.CraftingItem);
                result.Add(new CraftingRequestSaveData { CraftingItemIndex = index, TimeLeft = req.TimeLeft });
            }
            return result;
        }

        public void LoadSaveData(BuildingSaveData data, CraftingItemsData craftingItemsData)
        {
            _hasEmployee = data.HasEmployee;
            _employer.SetActive(_hasEmployee);

            foreach (var reqData in data.CraftingQueue)
            {
                CraftingItemData craftingItem = craftingItemsData.CraftingItems[reqData.CraftingItemIndex];
                _craftingRequests.Add(new CraftingRequest(craftingItem, reqData.TimeLeft));
            }

            if (_hasEmployee && _craftingRequests.Count > 0)
                _procces = StartCoroutine(Procces());
        }

        private IEnumerator Procces()
        {
            while (_craftingRequests.Count > 0)
            {
                _craftingRequests[0].CraftingTick();
                _craftingFill.fillAmount = _craftingRequests[0].T;

                if (_craftingRequests[0].IsReady)
                {
                    CraftingRequest completed = _craftingRequests[0];
                    _craftingRequests.RemoveAt(0);
                    _inventoryService.AddItem(new InventoryItem(completed.CraftingItem.ResultItem, InventoryItemState.Good));
                    ServicesProvider.GetService<BuidRegisterService>().SaveBuildings();
                }

                yield return new WaitForEndOfFrame();
            }

            _craftingFill.fillAmount = 0;
            _procces = null;
        }

        public class CraftingRequest
        {
            public CraftingItemData CraftingItem => _craftingItem;
            public bool IsReady => _timeLeft <= 0;
            public float T => _timeLeft / _allTime;
            public float TimeLeft => _timeLeft;

            private CraftingItemData _craftingItem;
            private float _timeLeft;
            private float _allTime;

            public CraftingRequest(CraftingItemData craftingItem)
            {
                _craftingItem = craftingItem;
                _timeLeft = _craftingItem.CraftingTime;
                _allTime = _craftingItem.CraftingTime;
            }


            public CraftingRequest(CraftingItemData craftingItem, float timeLeft)
            {
                _craftingItem = craftingItem;
                _timeLeft = timeLeft;
                _allTime = craftingItem.CraftingTime;
            }

            public void CraftingTick()
            {
                _timeLeft -= Time.deltaTime;
            }
        }
    }
}

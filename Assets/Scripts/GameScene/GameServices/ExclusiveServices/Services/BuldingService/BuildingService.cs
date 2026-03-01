using System;
using System.Collections;
using UnityEngine;
using PawnshopSimulator.Building;
using PawnshopSimulator.Data;
using PawnshopSimulator.UI;

namespace PawnshopSimulator.Services
{
    public class BuildingService : ExclusiveServiceBase
    {
        private InputService _inputSystem;
        private BuildingUI _UI;
        private BuildingServiceData _buildingServiceData;

        private MonoBehaviour _coroutineRunner;

        private PlayerControllerService _playerControllerService;
        private BuidRegisterService _buidRegisterService;
        private RecursesManagerService _recursesManagerService;

        private PoolComponent<BuildebleObject>[] _pools;

        private BuildebleObject _tempBuldingObject;
        private BuildingItemData _tempBuildingItemData;

        private Coroutine _buildingProcces;

        private const float RaycastDistance = 50f;
        private const float RotationSpeed = 90f;

        private float _rotationDirection = 0f;

        public BuildingService(BuildingServiceData buildingServiceData, UIHolder uiHolder)
        {
            _UI = uiHolder.BuildingUI;
            _buildingServiceData = buildingServiceData;
        }

        public override void OnLaunchGame() 
        {
            _UI.SetActiveUIHolder(false);
        }

        public override void Bind(ServicesProvider componentProvider)
        {
            _coroutineRunner = componentProvider.Ticker;

            _playerControllerService = componentProvider.GetService<PlayerControllerService>();
            _buidRegisterService = componentProvider.GetService<BuidRegisterService>();
            _recursesManagerService = componentProvider.GetService<RecursesManagerService>();
            _inputSystem = componentProvider.GetService<InputService>();

            PoolService poolService = componentProvider.GetService<PoolService>();

            _pools = new PoolComponent<BuildebleObject>[_buildingServiceData.Items.Length];

            for (int i = 0; i < _pools.Length; i++)
            {
                _pools[i] = poolService.CreatePool<BuildebleObject>(_buildingServiceData.Items[i].Prefab, 10);
            }

            _inputSystem.BindDownKey(KeyCode.Mouse0, TryBuild);
            _inputSystem.BindDownKey(KeyCode.Mouse1, Cancel);

            _inputSystem.BindDownKey(KeyCode.Q, StartRotatePlus);
            _inputSystem.BindUpKey(KeyCode.Q, StopRotatePlus);

            _inputSystem.BindDownKey(KeyCode.E, StartRotateMinus);
            _inputSystem.BindUpKey(KeyCode.E, StopRotateMinus);
        }

        public override void EndServiceProcess()
        {
            _UI.SetActiveUIHolder(false);

            _rotationDirection = 0f;

            if (_tempBuldingObject != null)
            {
                _tempBuldingObject.gameObject.SetActive(false);
                _tempBuldingObject = null;
            }

            if (_buildingProcces != null)
                _coroutineRunner.StopCoroutine(_buildingProcces);
        }

        public void ChoiceBuidItem(BuildingItemData buildingItemData)
        {
            _tempBuildingItemData = buildingItemData;
            _exclusiveServicesController.OnServiceStartWork(this);

            _playerControllerService.SetPlayerActivity(true);

            _tempBuldingObject = Array.Find(_pools, pool => pool.Prefab == buildingItemData.Prefab).GetFreeElement();

            _UI.SetActiveUIHolder(true);
            _buildingProcces = _coroutineRunner.StartCoroutine(BuildingProcces());
        }

        private void TryBuild()
        {
            if (_tempBuldingObject == null) return;
            if (!_tempBuldingObject.gameObject.activeSelf) return;
            if (HasOverlapOnXZ(_tempBuldingObject.Collider)) return;
            if (!_recursesManagerService.TryPurchase(_tempBuildingItemData.Price)) return;

            _buidRegisterService.RegistBuild(_tempBuldingObject, _tempBuildingItemData);

            _tempBuldingObject = null;
            _tempBuildingItemData = null;

            EndServiceProcess();
        }

        private void Cancel()
        {
            EndServiceProcess();
        }

        private void StartRotatePlus() => _rotationDirection += 1f;
        private void StopRotatePlus() => _rotationDirection -= 1f;
        private void StartRotateMinus() => _rotationDirection -= 1f;
        private void StopRotateMinus() => _rotationDirection += 1f;

        private IEnumerator BuildingProcces()
        {
            while (true)
            {
                if (_tempBuldingObject != null)
                {
                    if (_rotationDirection != 0f)
                    {
                        _tempBuldingObject.Transform.Rotate(
                            Vector3.up,
                            RotationSpeed * _rotationDirection * Time.deltaTime,
                            Space.World
                        );
                    }

                    _tempBuldingObject.Collider.enabled = false;

                    if (Physics.Raycast(_playerControllerService.EyePos, _playerControllerService.CameraDirection, out RaycastHit hit, RaycastDistance))
                    {
                        if (hit.collider.GetComponentInParent<BuildHolder>() != null)
                        {
                            if (!_tempBuldingObject.gameObject.activeSelf)
                                _tempBuldingObject.gameObject.SetActive(true);

                            _tempBuldingObject.Transform.position = hit.point;

                            _tempBuldingObject.Collider.enabled = true;

                            _tempBuldingObject.SetBuildMaterial(!HasOverlapOnXZ(_tempBuldingObject.Collider));
                        }
                        else
                        {
                            _tempBuldingObject.gameObject.SetActive(false);
                        }
                    }
                    else
                    {
                        _tempBuldingObject.gameObject.SetActive(false);
                    }
                }

                yield return new WaitForEndOfFrame();
            }
        }

        private bool HasOverlapOnXZ(Collider buildingCollider)
        {
            Bounds bounds = buildingCollider.bounds;
            Vector3 halfExtents = bounds.extents * 0.99f;

            Collider[] overlaps = Physics.OverlapBox(bounds.center, halfExtents, _tempBuldingObject.Transform.rotation);

            foreach (Collider col in overlaps)
            {
                if (col == buildingCollider) continue;
                if (col.isTrigger) continue;
                if (col.GetComponentInParent<BuildHolder>() != null) continue;

                Bounds otherBounds = col.bounds;

                float overlapX = (bounds.extents.x + otherBounds.extents.x) - Mathf.Abs(bounds.center.x - otherBounds.center.x);
                float overlapZ = (bounds.extents.z + otherBounds.extents.z) - Mathf.Abs(bounds.center.z - otherBounds.center.z);

                if (overlapX > 0f || overlapZ > 0f)
                    return true;
            }

            return false;
        }
    }
}

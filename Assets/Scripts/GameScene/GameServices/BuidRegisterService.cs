using System;
using System.Collections.Generic;
using UnityEngine;
using PawnshopSimulator.Building;
using PawnshopSimulator.Data;


namespace PawnshopSimulator.Services
{
    public class BuidRegisterService : IGameService
    {
        private ServicesProvider _componentProvider;
        private SaveService _saveService;
        private PoolService _poolService;

        private readonly BuildingServiceData _buildingServiceData;
        private readonly CraftingItemsData _craftingItemsData;
        private readonly InventoryItemsData _inventoryItemsData;

        private readonly List<(BuildebleObject obj, BuildingItemData data)> _registeredBuildings
            = new List<(BuildebleObject, BuildingItemData)>();

        public BuidRegisterService(BuildingServiceData buildingServiceData, CraftingItemsData craftingItemsData, InventoryItemsData inventoryItemsData)
        {
            _buildingServiceData = buildingServiceData;
            _craftingItemsData = craftingItemsData;
            _inventoryItemsData = inventoryItemsData;
        }

        public void Bind(ServicesProvider componentProvider)
        {
            _componentProvider = componentProvider;
            _saveService = componentProvider.GetService<SaveService>();
            _poolService = componentProvider.GetService<PoolService>();
        }

        public void OnLaunchGame()
        {
            if (_saveService.LoadedData == null) return;

            foreach (var buildingSave in _saveService.LoadedData.Buildings)
            {
                BuildingItemData buildingItemData = _buildingServiceData.Items[buildingSave.BuildingItemIndex];

                BuildebleObject obj = _poolService
                    .CreatePool<BuildebleObject>(buildingItemData.Prefab, 10)
                    .GetFreeElement();

                obj.Transform.position = new Vector3(buildingSave.PosX, buildingSave.PosY, buildingSave.PosZ);
                obj.Transform.rotation = new Quaternion(buildingSave.RotX, buildingSave.RotY, buildingSave.RotZ, buildingSave.RotW);

                RegistBuildInternal(obj, buildingItemData);

                if (obj is CraftingTable craftingTable)
                    craftingTable.LoadSaveData(buildingSave, _craftingItemsData);
                else if (obj is DisassemblyTable disassemblyTable)
                    disassemblyTable.LoadSaveData(buildingSave, _inventoryItemsData);
                else if (obj is ShowCase showCase)
                    showCase.LoadSaveData(buildingSave.ShowCase, _inventoryItemsData);
            }
        }

        public void RegistBuild(BuildebleObject buildebleObject, BuildingItemData buildingItemData)
        {
            RegistBuildInternal(buildebleObject, buildingItemData);
            SaveBuildings();
        }

        public void UnregisterBuild(BuildebleObject buildebleObject)
        {
            _registeredBuildings.RemoveAll(b => b.obj == buildebleObject);
            SaveBuildings();
        }

        public void SaveBuildings()
        {
            _saveService.SaveBuildings(GetBuildingsSaveData());
        }

        public List<T> GetBuildings<T>() where T : BuildebleObject
        {
            var result = new List<T>();
            foreach (var (obj, _) in _registeredBuildings)
                if (obj.gameObject.activeSelf && obj is T typed)
                    result.Add(typed);
            return result;
        }

        private List<BuildingSaveData> GetBuildingsSaveData()
        {
            var result = new List<BuildingSaveData>();

            foreach (var (obj, data) in _registeredBuildings)
            {
                if (!obj.gameObject.activeSelf) continue;

                int index = Array.IndexOf(_buildingServiceData.Items, data);

                var buildingSave = new BuildingSaveData
                {
                    BuildingItemIndex = index,
                    PosX = obj.Transform.position.x,
                    PosY = obj.Transform.position.y,
                    PosZ = obj.Transform.position.z,
                    RotX = obj.Transform.rotation.x,
                    RotY = obj.Transform.rotation.y,
                    RotZ = obj.Transform.rotation.z,
                    RotW = obj.Transform.rotation.w,
                };

                if (obj is CraftingTable craftingTable)
                {
                    buildingSave.HasEmployee = craftingTable.HasEmployee;
                    buildingSave.CraftingQueue = craftingTable.GetCraftingQueueSaveData(_craftingItemsData);
                }
                else if (obj is DisassemblyTable disassemblyTable)
                {
                    buildingSave.HasEmployee = disassemblyTable.HasEmployee;
                    buildingSave.DisassemblyQueue = disassemblyTable.GetDisassemblyQueueSaveData(_inventoryItemsData);
                }
                else if (obj is ShowCase showCase)
                {
                    buildingSave.ShowCase = showCase.GetSaveData(_inventoryItemsData);
                }

                result.Add(buildingSave);
            }

            return result;
        }

        private void RegistBuildInternal(BuildebleObject buildebleObject, BuildingItemData buildingItemData)
        {
            buildebleObject.SetOriginMaterial();
            buildebleObject.SetBuildingData(buildingItemData, _componentProvider);
            buildebleObject.Bind(_componentProvider);
            _registeredBuildings.Add((buildebleObject, buildingItemData));
        }
    }
}

using System.Collections.Generic;
using System.IO;
using UnityEngine;


namespace PawnshopSimulator.Services
{
    public class SaveService : IGameService
    {
        public SaveData LoadedData { get; private set; }

        private readonly string _savePath;

        public SaveService()
        {
            _savePath = Path.Combine(Application.persistentDataPath, "save.json");
            Load();
        }

        public void Bind(ServicesProvider componentProvider) { }
        public void OnLaunchGame() { }

        public void SaveResources(List<ResourceSaveData> data)
        {
            EnsureSaveData();
            LoadedData.Resources = data;
            Write();
        }

        public void SaveInventory(List<InventoryItemSaveData> data)
        {
            EnsureSaveData();
            LoadedData.Inventory = data;
            Write();
        }

        public void SaveBuildings(List<BuildingSaveData> data)
        {
            EnsureSaveData();
            LoadedData.Buildings = data;
            Write();
        }

        public void DeleteSave()
        {
            if (!File.Exists(_savePath)) return;

            File.Delete(_savePath);
            LoadedData = null;
        }

        private void EnsureSaveData()
        {
            if (LoadedData == null)
                LoadedData = new SaveData();
        }

        private void Write()
        {
            File.WriteAllText(_savePath, JsonUtility.ToJson(LoadedData, true));
        }

        private void Load()
        {
            if (!File.Exists(_savePath))
            {
                LoadedData = null;
                return;
            }

            LoadedData = JsonUtility.FromJson<SaveData>(File.ReadAllText(_savePath));
        }
    }
}

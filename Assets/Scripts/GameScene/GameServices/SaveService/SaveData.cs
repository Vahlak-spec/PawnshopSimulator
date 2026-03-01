using System.Collections.Generic;


namespace PawnshopSimulator.Services
{
    [System.Serializable]
    public class SaveData
    {
        public List<ResourceSaveData> Resources = new List<ResourceSaveData>();
        public List<InventoryItemSaveData> Inventory = new List<InventoryItemSaveData>();
        public List<BuildingSaveData> Buildings = new List<BuildingSaveData>();
    }

    [System.Serializable]
    public class ResourceSaveData
    {
        public int Type;
        public int Value;
    }

    [System.Serializable]
    public class InventoryItemSaveData
    {
        public int ItemIndex;
        public int State;
    }

    [System.Serializable]
    public class BuildingSaveData
    {
        public int BuildingItemIndex;
        public float PosX, PosY, PosZ;
        public float RotX, RotY, RotZ, RotW;
        public bool HasEmployee;
        public List<CraftingRequestSaveData> CraftingQueue = new List<CraftingRequestSaveData>();
        public List<DisassemblyRequestSaveData> DisassemblyQueue = new List<DisassemblyRequestSaveData>();
        public ShowCaseSaveData ShowCase = new ShowCaseSaveData();
    }

    [System.Serializable]
    public class CraftingRequestSaveData
    {
        public int CraftingItemIndex;
        public float TimeLeft;
    }

    [System.Serializable]
    public class DisassemblyRequestSaveData
    {
        public int InventoryItemIndex;
        public int ItemState;
        public float TimeLeft;
    }

    [System.Serializable]
    public class ShowCaseSaveData
    {
        public int ItemIndex = -1; 
        public int ItemState;
    }
}

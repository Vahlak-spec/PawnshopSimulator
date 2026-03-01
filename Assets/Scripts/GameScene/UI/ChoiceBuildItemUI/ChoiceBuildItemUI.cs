using System;
using UnityEngine;
using PawnshopSimulator.Data;

namespace PawnshopSimulator.UI
{
    public class ChoiceBuildItemUI : MonoBehaviour
    {
        [SerializeField] private GameObject _builChoiceUIHolder;
        [SerializeField] private BuildUIItem[] _buildUIItems;

        public Action<BuildingItemData> onChoiceItem;

        public void Bind(BuildingServiceData buildingServiceData, RecursesData recursesData)
        {
            for(int i = 0; i < _buildUIItems.Length; i++)
            {
                _buildUIItems[i].gameObject.SetActive(false);
            }

            for (int i = 0; i < buildingServiceData.Items.Length; i++)
            {
                _buildUIItems[i].gameObject.SetActive(true);
                _buildUIItems[i].Bind(buildingServiceData.Items[i], recursesData);
                _buildUIItems[i].onChoice = OnChoise;
            }
        }
        private void OnChoise(BuildingItemData buildingItemData)
        {
            onChoiceItem?.Invoke(buildingItemData);
        }
        public void SetActiveUI(bool value) => _builChoiceUIHolder.SetActive(value);
    }
}

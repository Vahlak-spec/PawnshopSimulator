using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using PawnshopSimulator;
using PawnshopSimulator.Data;

namespace PawnshopSimulator.UI
{
    public class BuildUIItem : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private Image _image;
        [Space]
        [SerializeField] private BuildRecuistResursUI[] _buildRecuistResursUIs;

        private BuildingItemData _data;

        public Action<BuildingItemData> onChoice;

        public void Bind(BuildingItemData data, RecursesData recursesData)
        {
            _data = data;

            _nameText.text = data.ItemName;
            _image.sprite = data.Logo;

            for (int i = 0; i < _buildRecuistResursUIs.Length; i++)
                _buildRecuistResursUIs[i].SetActive(false);

            for (int i = 0; i < data.Price.RecurseRequests.Length; i++)
            {
                _buildRecuistResursUIs[i].SetActive(true);

                _buildRecuistResursUIs[i].Bind
                    (
                    Array.Find(recursesData.recursDatas,r => r.RecursType == data.Price.RecurseRequests[i].RecursType).Logo,
                    data.Price.RecurseRequests[i].Value
                    );
            }
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            onChoice?.Invoke(_data);
        }

        [System.Serializable]
        private class BuildRecuistResursUI
        {
            [SerializeField] private GameObject _holder;
            [Space]
            [SerializeField] private Image _image;
            [SerializeField] private TextMeshProUGUI _valueText;

            public void SetActive(bool value)
            {
                _holder.SetActive(value);
            }
            public void Bind(Sprite sprite, int value)
            {
                _image.sprite = sprite;
                _valueText.text = value.ToString();
            }
        }
    }
}

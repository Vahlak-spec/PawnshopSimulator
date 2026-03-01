using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using PawnshopSimulator.Data;

namespace PawnshopSimulator.UI
{
    public class RecursesGroupUI : MonoBehaviour
    {
        [SerializeField] private CraftRecuistResursUI[] _recuistResursUI;

        public void Bind(RecurseGroupData recurseRequests, RecursesData recursesData)
        {
            for (int i = 0; i < _recuistResursUI.Length; i++)
            {
                _recuistResursUI[i].SetActive(false);
            }

            for (int i = 0; i < recurseRequests.RecurseRequests.Length; i++)
            {
                _recuistResursUI[i].SetActive(true);
                _recuistResursUI[i].Bind
                    (
                    Array.Find(recursesData.recursDatas, r => r.RecursType == recurseRequests.RecurseRequests[i].RecursType).Logo,
                    recurseRequests.RecurseRequests[i].Value
                    );
            }
        }

        [System.Serializable]
        private class CraftRecuistResursUI
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

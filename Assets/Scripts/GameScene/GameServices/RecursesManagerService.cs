using System;
using System.Collections.Generic;
using PawnshopSimulator.Data;
using PawnshopSimulator.UI;
using static PawnshopSimulator.Data.RecurseGroupData;

namespace PawnshopSimulator.Services
{
    public class RecursesManagerService : IGameService
    {
        private Recurs[] _recurs;

        private RecursesUI _recursesUI;
        private RecursesData _recursesData;
        private SaveService _saveService;

        public RecursesManagerService(RecursesData recursesData, UIHolder uiHolder)
        {
            _recursesData = recursesData;
            _recurs = new Recurs[recursesData.recursDatas.Length];
            _recursesUI = uiHolder.RecursesUI;
        }

        public void Bind(ServicesProvider componentProvider)
        {
            _saveService = componentProvider.GetService<SaveService>();

            for (int i = 0; i < _recursesUI.recursesTypeUI.Length; i++)
                _recursesUI.recursesTypeUI[i].SetActive(false);

            for (int i = 0; i < _recurs.Length; i++)
            {
                _recursesUI.recursesTypeUI[i].SetActive(true);
                _recursesUI.recursesTypeUI[i].SetSprite(_recursesData.recursDatas[i].Logo);

                _recurs[i] = new Recurs(
                    _recursesData.recursDatas[i].DefaultValue,
                    _recursesData.recursDatas[i].RecursType,
                    _recursesUI.recursesTypeUI[i].OnChangeValue,
                    () => _saveService.SaveResources(GetSaveData())
                );
            }
        }

        public void OnLaunchGame()
        {
            if (_saveService.LoadedData == null) return;

            foreach (var saved in _saveService.LoadedData.Resources)
            {
                Recurs recurs = Array.Find(_recurs, r => r.RecursType == (RecursType)saved.Type);
                if (recurs != null)
                    recurs.SetValueSilent(saved.Value);
            }
        }

        public bool TryPurchase(RecurseGroupData price)
        {
            for (int i = 0; i < price.RecurseRequests.Length; i++)
                if (!HaveResurce(price.RecurseRequests[i]))
                    return false;

            for (int i = 0; i < price.RecurseRequests.Length; i++)
                UseResurse(price.RecurseRequests[i]);

            return true;
        }

        public void AddRecourses(RecurseGroupData recurseNeed)
        {
            for (int i = 0; i < recurseNeed.RecurseRequests.Length; i++)
                AddRecouse(recurseNeed.RecurseRequests[i]);
        }

        public List<ResourceSaveData> GetSaveData()
        {
            var result = new List<ResourceSaveData>();
            foreach (var recurs in _recurs)
                result.Add(new ResourceSaveData { Type = (int)recurs.RecursType, Value = recurs.Value });
            return result;
        }

        public int GetAmount(RecursType type)
            => Array.Find(_recurs, r => r.RecursType == type).Value;

        public void AddAmount(RecursType type, int amount)
        {
            Array.Find(_recurs, r => r.RecursType == type).Value += amount;
        }

        public bool TrySpend(RecursType type, int amount)
        {
            Recurs recurs = Array.Find(_recurs, r => r.RecursType == type);
            if (recurs.Value < amount) return false;
            recurs.Value -= amount;
            return true;
        }

        private bool HaveResurce(RecurseRequest recurseRequest)
            => Array.Find(_recurs, r => r.RecursType == recurseRequest.RecursType).Value >= recurseRequest.Value;

        private void UseResurse(RecurseRequest recurseRequest)
            => Array.Find(_recurs, r => r.RecursType == recurseRequest.RecursType).Value -= recurseRequest.Value;

        private void AddRecouse(RecurseRequest recurseRequest)
            => Array.Find(_recurs, r => r.RecursType == recurseRequest.RecursType).Value += recurseRequest.Value;

        private class Recurs
        {
            public RecursType RecursType => _recursType;

            public int Value
            {
                get => _value;
                set
                {
                    _value = value;
                    _onChange?.Invoke(_value);
                    _onSave?.Invoke();
                }
            }

            public void SetValueSilent(int value)
            {
                _value = value;
                _onChange?.Invoke(_value);
            }

            public Recurs(int value, RecursType recursType, Action<int> onChange, Action onSave)
            {
                _value = value;
                _onChange = onChange;
                _recursType = recursType;
                _onSave = onSave;

                _onChange?.Invoke(value);
            }

            private int _value;
            private Action<int> _onChange;
            private Action _onSave;
            private RecursType _recursType;
        }
    }
}

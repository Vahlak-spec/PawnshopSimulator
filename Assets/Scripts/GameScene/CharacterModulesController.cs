using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using PawnshopSimulator.Services;

namespace PawnshopSimulator.Characters
{
    public class CharacterModulesController : PoolObject
    {
        [SerializeField] private CharacterModuleBase[] _modules;

        public T GetModule<T>() where T : CharacterModuleBase
        {
            return _modules.OfType<T>().FirstOrDefault()
            ?? throw new KeyNotFoundException();
        }
        public T[] GetModules<T>() where T : CharacterModuleBase
        {
            return _modules.OfType<T>().ToArray();
        }
        public void Launch()
        {
            for (int i = 0; i < _modules.Length; i++)
                _modules[i].Launch();
        }
        public override void OnSummon() { }
        public void SetModulesActivity(bool value)
        {
            for (int i = 0; i < _modules.Length; i++)
            {
                _modules[i].SetModuleActive(value);
            }
        }
    }
}

using System.Collections.Generic;
using UnityEngine;

namespace PawnshopSimulator.Services
{
    public class PoolService : IGameService
    {
        private Dictionary<GameObject, object> _poolsContainers;
        private Transform _container;

        public PoolService(Transform container)
        {
            _poolsContainers = new Dictionary<GameObject, object>();
            _container = container;
        }

        public void Bind(ServicesProvider componentProvider) 
        {

        }

        public void Clear()
        {
            _poolsContainers.Clear();
            _poolsContainers = null;
        }

        public PoolComponent<T> CreatePool<T>(T prefab, int amount) where T : PoolObject
        {
            if (_poolsContainers.TryGetValue(prefab.gameObject, out object item))
            {
                var pool = item as PoolComponent<T>;
                if (pool.Size < amount) pool.Add(amount - pool.Size);
                return pool;
            }
            else
            {
                Transform container = new GameObject().transform;
                container.gameObject.name = "[" + prefab.gameObject.name + "]Pool";
                container.SetParent(_container);

                PoolComponent<T> newPool = new PoolComponent<T>(prefab, amount, container, true);

                _poolsContainers.Add(prefab.gameObject, newPool);

                return newPool;
            }
        }
        public void OnLaunchGame() { }
    }
}

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PawnshopSimulator.Services
{
    public class PoolComponent<T> where T : PoolObject
    {
        private List<T> _pool;
        private readonly T _prefab;
        private readonly bool _autoExpand;
        private Transform _container;

        public T Prefab => _prefab;
        public Transform Container => _container;
        public int Size => _pool.Count;

        public PoolComponent(T prefab, int count, Transform container, bool autoExpand)
        {
            _prefab = prefab;
            _autoExpand = autoExpand;
            _container = container;

            CreatePool(count);
        }

        public void Add(int amount)
        {
            while (_pool.Count < _pool.Count + amount)
                CreateObject();
        }

        public void BoundContainer(Transform container)
            => _container = container;

        public void ClearPool()
        {
            foreach (var item in _pool)
                GameObject.Destroy(item.gameObject);

            _pool.Clear();
        }

        public T GetFreeElement()
        {
            if (HasFreeElement(out T element))
                return element;

            if (_autoExpand)
                return CreateObject(true);

            throw new System.Exception("There is no free element in pool");
        }

        public List<T> GetElements(int count)
        {
            var elements = new List<T>();
            for (int i = 0; i < count; i++)
            {
                if (HasFreeElement(out T element))
                    elements.Add(element);
                else if (_autoExpand)
                    elements.Add(CreateObject(true));
                else
                    throw new System.Exception("Not enough free elements in the pool to get the requested amount.");
            }
            return elements;
        }

        public List<T> GetAllFreeElements() =>
            _pool.Where(element => element.gameObject.activeInHierarchy == false).ToList();

        public List<T> GetAllElements() =>
            _pool.ToList();

        private void CreatePool(int count)
        {
            _pool = new List<T>();
            for (int i = 0; i < count; i++)
            {
                CreateObject();
            }
        }

        private T CreateObject(bool isActiveByDefault = false)
        {
            T createdObject = Object.Instantiate(_prefab, _container);
            createdObject.gameObject.SetActive(isActiveByDefault);
            createdObject.OnSummon();
            _pool.Add(createdObject);
            return createdObject;
        }

        private bool HasFreeElement(out T element)
        {
            foreach (T mono in _pool.Where(mono => mono.gameObject.activeInHierarchy == false))
            {
                element = mono;
                mono.gameObject.SetActive(true);
                mono.OnSummon();
                return true;
            }
            element = null;
            return false;
        }
    }
}

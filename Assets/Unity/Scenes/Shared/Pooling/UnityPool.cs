using System;
using System.Collections.Generic;
using UnityEngine;

namespace Unity.Scenes.Shared.Pooling
{
    public class UnityPool : MonoBehaviour, IUnityPool
    {
        private Transform _poolRoot = null;
        private readonly Dictionary<Type, Stack<Component>> _pool = new Dictionary<Type, Stack<Component>>();

        public static IUnityPool CreatePool(Transform poolRoot = null)
        {
            UnityPool newPool = new GameObject("UnityPool").AddComponent<UnityPool>();
            if (poolRoot)
            {
                newPool._poolRoot = poolRoot;
            }
            else
            {
                newPool._poolRoot = newPool.transform;
                DontDestroyOnLoad(newPool);
            }

            return newPool;
        }

        public T Pop<T>(T prefabReference = null, Transform parent = null) where T : Component
        {
            Type type = typeof(T);

            // Get or create object pool for this type.
            if (!_pool.TryGetValue(type, out var bag))
            {
                _pool[type] = bag = new Stack<Component>();
            }

            // Pull next element from the pool, or create a new one.
            if (bag.Count != 0 &&
                bag.Pop() is T asType)
            {
                asType.transform.SetParent(parent);
                return asType;
            }
            else if (prefabReference)
            {
                return Instantiate<T>(prefabReference, parent);
            }

            return null;
        }

        public void Push<T>(T obj) where T : Component
        {
            Type type = typeof(T);

            // Get or create object pool for this type.
            if (!_pool.TryGetValue(type, out var bag))
            {
                _pool[type] = bag = new Stack<Component>();
            }

            // Push object into the pool.
            bag.Push(obj);
            obj.transform.SetParent(_poolRoot);
        }

        public void Clear<T>()
        {
            Type type = typeof(T);

            // Get or create object pool for this type.
            if (_pool.TryGetValue(type, out var bag))
            {
                foreach (Component component in bag)
                {
                    if (component)
                    {
                        Destroy(component.gameObject);
                    }
                }

                bag.Clear();
            }
        }

        public void Clear()
        {
            foreach (var poolKvp in _pool)
            {
                foreach (Component component in poolKvp.Value)
                {
                    if (component)
                    {
                        Destroy(component.gameObject);
                    }
                }

                poolKvp.Value.Clear();
            }
        }
    }
}
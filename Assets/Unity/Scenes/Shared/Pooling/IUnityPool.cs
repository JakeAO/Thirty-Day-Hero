using UnityEngine;

namespace Unity.Scenes.Shared.Pooling
{
    public interface IUnityPool
    {
        T Pop<T>(T prefabReference = null, Transform parent = null) where T : Component;
        void Push<T>(T obj) where T : Component;

        void Clear<T>();
        void Clear();
    }
}
using UnityEngine;

namespace Unity.Extensions
{
    public static class GameObjectExtensions
    {
        public static void UpdateActive(this GameObject go, bool active)
        {
            if (go &&
                go.activeSelf != active)
            {
                go.SetActive(active);
            }
        }
    }
}
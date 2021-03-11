using UnityEngine;

namespace Unity.Extensions
{
    public static class MonobehaviorExtensions
    {
        public static void UpdateActive(this MonoBehaviour mono, bool active)
        {
            if (mono &&
                mono.gameObject &&
                mono.gameObject.activeSelf != active)
            {
                mono.gameObject.SetActive(active);
            }
        }
    }
}
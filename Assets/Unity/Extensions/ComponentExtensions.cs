using UnityEngine;

namespace Unity.Extensions
{
    public static class ComponentExtensions
    {
        public static void UpdateActive(this Component component, bool active)
        {
            if (component &&
                component.gameObject &&
                component.gameObject.activeSelf != active)
            {
                component.gameObject.SetActive(active);
            }
        }
    }
}
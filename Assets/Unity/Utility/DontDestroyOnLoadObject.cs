using UnityEngine;

namespace Unity.Utility
{
    public class DontDestroyOnLoadObject : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}

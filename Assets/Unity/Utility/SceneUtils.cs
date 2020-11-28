using UnityEngine;
using UnityEngine.SceneManagement;

namespace Unity.Scenes
{
    public static class SceneUtils
    {
        public static T GetSceneComponent<T>(this Scene scene)
        {
            GameObject[] objs = scene.GetRootGameObjects();
            foreach (GameObject go in objs)
            {
                T component = go.GetComponent<T>();
                if (component != null)
                {
                    return component;
                }
            }

            return default;
        }
    }
}
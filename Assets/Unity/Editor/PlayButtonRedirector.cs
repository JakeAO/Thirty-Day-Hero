using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

namespace Unity.Editor
{
    [InitializeOnLoad]
    public static class PlayButtonRedirector
    {
        static PlayButtonRedirector()
        {
            EditorApplication.playModeStateChanged += EditorApplicationOnplayModeStateChanged;
        }

        private const string PREF_NAME = "LastOpenSceneInEditMode";
        private const string BOOT_PATH = "Assets/Scenes/Boot.unity";

        private static void EditorApplicationOnplayModeStateChanged(PlayModeStateChange change)
        {
            switch (change)
            {
                case PlayModeStateChange.ExitingEditMode:
                {
                    EditorPrefs.SetString("LastOpenSceneInEditMode", SceneManager.GetActiveScene().path);
                    EditorSceneManager.OpenScene(BOOT_PATH, OpenSceneMode.Single);
                    break;
                }
                case PlayModeStateChange.EnteredEditMode:
                {
                    if (EditorPrefs.HasKey(PREF_NAME) &&
                        EditorPrefs.GetString(PREF_NAME) is string scenePath &&
                        !string.IsNullOrWhiteSpace(scenePath))
                    {
                        EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
                    }

                    break;
                }
            }
        }
    }
}
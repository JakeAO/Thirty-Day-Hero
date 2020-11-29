using Core.Etc;
using Core.Wrappers;
using UnityEditor;
using UnityEngine;

namespace Unity.Editor.DataEntryWindows
{
    public class EnemyGroupEditorWindow : DatabaseEditorWindowBase<EnemyGroupWrapper>
    {
        [MenuItem("SadPumpkin/Editors/Enemy Group", false, 0)]
        public static EnemyGroupEditorWindow GetWindow()
        {
            var window = EditorWindow.GetWindow<EnemyGroupEditorWindow>();
            window.minSize = new Vector2(300, 400);
            return window;
        }

        protected override string GetRootPathForType(PathUtility pathUtility)
        {
            return pathUtility.EnemyGroupPath;
        }

        protected override bool CanSave(EnemyGroupWrapper entry)
        {
            return entry != null &&
                   entry.Id != 0u &&
                   entry.EnemyTypes.Count > 0;
        }
    }
}
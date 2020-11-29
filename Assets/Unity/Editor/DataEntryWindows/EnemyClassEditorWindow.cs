using Core.Classes.Enemy;
using Core.Etc;
using UnityEditor;
using UnityEngine;

namespace Unity.Editor.DataEntryWindows
{
    public class EnemyClassEditorWindow : DatabaseEditorWindowBase<EnemyClass>
    {
        [MenuItem("SadPumpkin/Editors/Enemy Class", false, 0)]
        public static EnemyClassEditorWindow GetWindow()
        {
            var window = EditorWindow.GetWindow<EnemyClassEditorWindow>();
            window.minSize = new Vector2(300, 400);
            return window;
        }

        protected override string GetRootPathForType(PathUtility pathUtility)
        {
            return pathUtility.EnemyClassPath;
        }

        protected override bool CanSave(EnemyClass entry)
        {
            return entry != null &&
                   entry.Id != 0u &&
                   !string.IsNullOrWhiteSpace(entry.Name) &&
                   !string.IsNullOrWhiteSpace(entry.Desc) &&
                   entry.NameGenerator != null &&
                   entry.StartingStats != null &&
                   entry.LevelUpStats != null;
        }
    }
}
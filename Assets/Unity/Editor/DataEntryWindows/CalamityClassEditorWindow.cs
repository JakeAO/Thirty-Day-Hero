using Core.Classes.Calamity;
using Core.Etc;
using UnityEditor;
using UnityEngine;

namespace Unity.Editor.DataEntryWindows
{
    public class CalamityClassEditorWindow : DatabaseEditorWindowBase<CalamityClass>
    {
        [MenuItem("SadPumpkin/Editors/Calamity Class", false, 0)]
        public static CalamityClassEditorWindow GetWindow()
        {
            var window = EditorWindow.GetWindow<CalamityClassEditorWindow>();
            window.minSize = new Vector2(300, 400);
            return window;
        }

        protected override string GetRootPathForType(PathUtility pathUtility)
        {
            return pathUtility.CalamityClassPath;
        }

        protected override bool CanSave(CalamityClass entry)
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
using Core.Classes.Player;
using Core.Etc;
using UnityEditor;
using UnityEngine;

namespace Unity.Editor.DataEntryWindows
{
    public class PlayerClassEditorWindow : DatabaseEditorWindowBase<PlayerClass>
    {
        [MenuItem("SadPumpkin/Editors/Player Class", false, 0)]
        public static PlayerClassEditorWindow GetWindow()
        {
            var window = EditorWindow.GetWindow<PlayerClassEditorWindow>();
            window.minSize = new Vector2(300, 400);
            return window;
        }

        protected override string GetRootPathForType(PathUtility pathUtility)
        {
            return pathUtility.PlayerClassPath;
        }

        protected override bool CanSave(PlayerClass entry)
        {
            return entry != null &&
                   entry.Id != 0u &&
                   !string.IsNullOrWhiteSpace(entry.Name) &&
                   !string.IsNullOrWhiteSpace(entry.Desc) &&
                   entry.NameGenerator != null &&
                   entry.StartingStats != null &&
                   entry.LevelUpStats != null &&
                   entry.StartingEquipment != null;
        }
    }
}
using Core.Etc;
using Core.Items.Armors;
using UnityEditor;
using UnityEngine;

namespace Unity.Editor.DataEntryWindows
{
    public class ArmorEditorWindow : DatabaseEditorWindowBase<Armor>
    {
        [MenuItem("SadPumpkin/Editors/Armor", false, 0)]
        public static ArmorEditorWindow GetWindow()
        {
            var window = EditorWindow.GetWindow<ArmorEditorWindow>();
            window.minSize = new Vector2(300, 400);
            return window;
        }

        protected override string GetRootPathForType(PathUtility pathUtility)
        {
            return pathUtility.ArmorDefinitionPath;
        }

        protected override bool CanSave(Armor entry)
        {
            return entry != null &&
                   entry.Id != 0u &&
                   !string.IsNullOrWhiteSpace(entry.Name) &&
                   !string.IsNullOrWhiteSpace(entry.Desc) &&
                   entry.ArmorType != ArmorType.Invalid &&
                   entry.ItemType != ItemType.Invalid;
        }
    }
}
using Core.Etc;
using Core.Items;
using UnityEditor;
using UnityEngine;

namespace Unity.Editor.DataEntryWindows
{
    public class ItemEditorWindow : DatabaseEditorWindowBase<Item>
    {
        [MenuItem("SadPumpkin/Editors/Item", false, 0)]
        public static ItemEditorWindow GetWindow()
        {
            var window = EditorWindow.GetWindow<ItemEditorWindow>();
            window.minSize = new Vector2(300, 400);
            return window;
        }

        protected override string GetRootPathForType(PathUtility pathUtility)
        {
            return pathUtility.ItemDefinitionPath;
        }

        protected override bool CanSave(Item entry)
        {
            return entry != null &&
                   entry.Id != 0u &&
                   !string.IsNullOrWhiteSpace(entry.Name) &&
                   !string.IsNullOrWhiteSpace(entry.Desc) &&
                   !string.IsNullOrWhiteSpace(entry.ArtPath) &&
                   entry.ItemType != ItemType.Invalid;
        }
    }
}
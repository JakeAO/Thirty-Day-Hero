using Core.Etc;
using Core.Items.Weapons;
using UnityEditor;
using UnityEngine;

namespace Unity.Editor.DataEntryWindows
{
    public class WeaponEditorWindow : DatabaseEditorWindowBase<Weapon>
    {
        [MenuItem("SadPumpkin/Editors/Weapon", false, 0)]
        public static WeaponEditorWindow GetWindow()
        {
            var window = EditorWindow.GetWindow<WeaponEditorWindow>();
            window.minSize = new Vector2(300, 400);
            return window;
        }

        protected override string GetRootPathForType(PathUtility pathUtility)
        {
            return pathUtility.WeaponDefinitionPath;
        }

        protected override bool CanSave(Weapon entry)
        {
            return entry != null &&
                   entry.Id != 0u &&
                   !string.IsNullOrWhiteSpace(entry.Name) &&
                   !string.IsNullOrWhiteSpace(entry.Desc) &&
                   entry.WeaponType != WeaponType.Invalid &&
                   entry.ItemType != ItemType.Invalid &&
                   entry.AttackAbility != null;
        }
    }
}
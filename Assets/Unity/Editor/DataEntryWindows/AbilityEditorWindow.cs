using Core.Abilities;
using Core.Etc;
using Unity.Editor.DataEntryWindows;
using UnityEditor;
using UnityEngine;

public class AbilityEditorWindow : DatabaseEditorWindowBase<Ability>
{
    [MenuItem("SadPumpkin/Editors/Ability", false, 0)]
    public static AbilityEditorWindow GetWindow()
    {
        var window = EditorWindow.GetWindow<AbilityEditorWindow>();
        window.minSize = new Vector2(300, 400);
        return window;
    }

    protected override string GetRootPathForType(PathUtility pathUtility)
    {
        return pathUtility.AbilityPath;
    }

    protected override bool CanSave(Ability entry)
    {
        return entry != null &&
               entry.Id != 0u &&
               !string.IsNullOrWhiteSpace(entry.Name) &&
               !string.IsNullOrWhiteSpace(entry.Desc) &&
               entry.Target != null &&
               entry.Cost != null &&
               entry.Requirements != null &&
               entry.Effect != null;
    }
}
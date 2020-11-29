using System;
using System.Collections.Generic;
using System.IO;
using Core.Abilities;
using Core.Etc;
using Newtonsoft.Json;
using SadPumpkin.Util.Context;
using Unity.Editor;
using UnityEditor;
using UnityEngine;

public class AbilityEditorWindow : EditorWindow
{
    [MenuItem("SadPumpkin/Editors/Ability", false, 0)]
    public static AbilityEditorWindow GetWindow()
    {
        var window = EditorWindow.GetWindow<AbilityEditorWindow>();
        window.minSize = new Vector2(300, 400);
        return window;
    }

    private IDataDrawer<Ability> _dataDrawer = null;
    private IContext _sharedContext = null;
    private Ability _currentAbility = null;
    private IReadOnlyList<Ability> _abilitiesOnDisk = null;

    public bool CanSave()
    {
        return _currentAbility != null &&
               _currentAbility.Id != 0u &&
               !string.IsNullOrWhiteSpace(_currentAbility.Name) &&
               !string.IsNullOrWhiteSpace(_currentAbility.Desc) &&
               _currentAbility.Target != null &&
               _currentAbility.Cost != null &&
               _currentAbility.Requirements != null &&
               _currentAbility.Effect != null;
    }

    private string GetFileNameForEntry(Ability ability)
    {
        if (ability == null)
            return string.Empty;

        string abilityName = ability.Name.Replace(" ", string.Empty);
        foreach (char invalidFileNameChar in Path.GetInvalidFileNameChars())
        {
            abilityName = abilityName.Replace(invalidFileNameChar.ToString(), string.Empty);
        }

        PathUtility pathUtility = _sharedContext.Get<PathUtility>();
        DirectoryInfo abilityDir = new DirectoryInfo(pathUtility.AbilityPath);
        string abilityPath = Path.Combine(abilityDir.FullName, $"{ability.Id}_{abilityName}.json");

        return abilityPath;
    }

    private void UpdateEntriesFromDisk()
    {
        PathUtility pathUtility = _sharedContext.Get<PathUtility>();
        JsonSerializerSettings jsonSettings = _sharedContext.Get<JsonSerializerSettings>();

        DirectoryInfo abilityDir = new DirectoryInfo(pathUtility.AbilityPath);
        FileInfo[] abilityFiles = abilityDir.GetFiles("*.json", SearchOption.AllDirectories);

        List<Ability> allAbilities = new List<Ability>(abilityFiles.Length);
        foreach (FileInfo abilityFile in abilityFiles)
        {
            string filePath = abilityFile.FullName;
            string fileContent = File.ReadAllText(filePath);
            Ability ability = JsonConvert.DeserializeObject<Ability>(fileContent, jsonSettings);
            if (ability != null)
            {
                allAbilities.Add(ability);

                string expectedPath = GetFileNameForEntry(ability);
                if (!string.Equals(filePath, expectedPath))
                {
                    Debug.LogError($"Loaded ability path does not match its expected path:\n" +
                                   $"   Actual: {filePath}\n" +
                                   $"   Expected: {expectedPath}");
                }
            }
        }

        _abilitiesOnDisk = allAbilities;
    }

    private bool SaveEntryToDisk(Func<bool> overrideExisting)
    {
        if (_currentAbility == null)
            return false;

        string abilityPath = GetFileNameForEntry(_currentAbility);

        if (File.Exists(abilityPath))
        {
            if (!overrideExisting?.Invoke() ?? false)
                return false;

            File.SetAttributes(abilityPath, FileAttributes.Normal);
        }

        JsonSerializerSettings jsonSettings = _sharedContext.Get<JsonSerializerSettings>();
        string abilityText = JsonConvert.SerializeObject(_currentAbility, jsonSettings);
        if (string.IsNullOrWhiteSpace(abilityText))
            return false;

        File.WriteAllText(abilityPath, abilityText);

        UpdateEntriesFromDisk();
        return true;
    }

    private void OnEnable()
    {
        _dataDrawer = DataDrawerFinder.Find<Ability>();
        _sharedContext = EditorDataLoadHelper.LoadEditorContext();
        _currentAbility = null;
        _abilitiesOnDisk = null;

        UpdateEntriesFromDisk();
    }

    private void OnGUI()
    {
        GUILayout.BeginVertical();
        {
            OnGUI_DrawHeader();

            OnGUI_DrawToolbar();

            if (_currentAbility != null)
            {
                OnGUI_DrawEntry(_currentAbility);
            }
            else
            {
                EditorGUILayout.HelpBox("Nothing selected.\nCreate or load an Ability using the toolbar above.", MessageType.Info);
            }
        }
        GUILayout.EndVertical();
    }

    private void OnGUI_DrawHeader()
    {
        GUILayout.BeginHorizontal();
        {
            GUILayout.Label("Ability Editor", new GUIStyle(GUI.skin.label) {fontStyle = FontStyle.Bold, fontSize = GUI.skin.label.fontSize + 5});
        }
        GUILayout.EndHorizontal();
    }

    private void OnGUI_DrawToolbar()
    {
        // Always Buttons
        GUILayout.BeginHorizontal();
        {
            if (GUILayout.Button("Load"))
            {
                GenericMenu gm = new GenericMenu();
                if (_abilitiesOnDisk.Count == 0)
                {
                    gm.AddDisabledItem(new GUIContent("No Abilities Found"));
                }
                else
                {
                    void SelectAbility(object abilityObj)
                    {
                        if (abilityObj is Ability ability)
                            _currentAbility = ability;
                    }

                    foreach (Ability ability in _abilitiesOnDisk)
                    {
                        Ability abilityCache = ability;
                        GUIContent content = new GUIContent(
                            ability.Name,
                            ability.Desc);
                        bool active = _currentAbility?.Id == ability?.Id;

                        gm.AddItem(content, active, SelectAbility, abilityCache);
                    }
                }
                gm.ShowAsContext();
            }

            if (GUILayout.Button("New"))
            {
                _currentAbility = new Ability();
            }
        }
        GUILayout.EndHorizontal();

        // Buttons if Loaded
        if (_currentAbility != null)
        {
            GUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("Clear"))
                {
                    _currentAbility = null;
                }

                GUI.enabled = CanSave();
                if (GUILayout.Button("Save"))
                {
                    bool result = SaveEntryToDisk(
                        () => EditorUtility.DisplayDialog(
                            "Overwrite Confirmation",
                            "Do you want to overwrite the existing file?",
                            "Yes",
                            "No"));
                    if (!result)
                    {
                        EditorUtility.DisplayDialog(
                            "Error",
                            "Failed to save the current entry to disk.",
                            "Okay");
                    }
                }
                GUI.enabled = true;
            }
            GUILayout.EndHorizontal();
        }
    }

    private void OnGUI_DrawEntry(Ability ability)
    {
        GUILayout.BeginVertical(GUI.skin.box);
        {
            if (_dataDrawer != null)
            {
                _dataDrawer.DrawGUI(ability, _sharedContext, null);
            }
            else
            {
                EditorGUILayout.HelpBox(
                    "No corresponding DataDrawer could be found for Ability!", 
                    MessageType.Error);
            }
        }
        GUILayout.EndVertical();
    }
}
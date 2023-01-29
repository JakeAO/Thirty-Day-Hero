using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Core.Etc;
using Newtonsoft.Json;
using SadPumpkin.Util.CombatEngine;
using SadPumpkin.Util.Context;
using Unity.Editor.DataDrawers;
using Unity.Editor.WindowUtilities;
using UnityEditor;
using UnityEngine;

namespace Unity.Editor.DataEntryWindows
{
    public abstract class DatabaseEditorWindowBase<EntryType> : EditorWindow where EntryType : class, IIdTracked, INamed, new()
    {
        private readonly string _typeName = typeof(EntryType).Name;

        private IDataDrawer<EntryType> _dataDrawer = null;
        private IContext _sharedContext = null;
        private EntryType _currentEntry = null;
        private IReadOnlyList<EntryType> _entriesOnDisk = null;
        private Vector2 _scrollPos = Vector2.zero;
        
        protected abstract string GetRootPathForType(PathUtility pathUtility);
        protected abstract bool CanSave(EntryType entry);

        protected virtual GUIContent GetLoadDropdownContent(EntryType entry)
        {
            return new GUIContent($"{entry.Name} ({entry.Id}) - {entry.Desc}");
        }

        private string GetFileNameForEntry(EntryType entry)
        {
            if (entry == null)
                return string.Empty;

            string entryName = entry.Name.Replace(" ", string.Empty);
            foreach (char invalidFileNameChar in Path.GetInvalidFileNameChars())
            {
                entryName = entryName.Replace(invalidFileNameChar.ToString(), string.Empty);
            }

            PathUtility pathUtility = _sharedContext.Get<PathUtility>();
            DirectoryInfo entryDir = new DirectoryInfo(GetRootPathForType(pathUtility));
            string entryPath = Path.Combine(entryDir.FullName, $"{entry.Id}_{entryName}.json");

            return entryPath;
        }

        private void UpdateEntriesFromDisk()
        {
            PathUtility pathUtility = _sharedContext.Get<PathUtility>();
            JsonSerializerSettings jsonSettings = _sharedContext.Get<JsonSerializerSettings>();

            DirectoryInfo entryDir = new DirectoryInfo(GetRootPathForType(pathUtility));
            FileInfo[] entryFiles = entryDir.GetFiles("*.json", SearchOption.AllDirectories);

            List<EntryType> allEntries = new List<EntryType>(entryFiles.Length);
            foreach (FileInfo entryFile in entryFiles)
            {
                string filePath = entryFile.FullName;
                string fileContent = File.ReadAllText(filePath);
                EntryType entry = JsonConvert.DeserializeObject<EntryType>(fileContent, jsonSettings);
                if (entry != null)
                {
                    allEntries.Add(entry);

                    string expectedPath = GetFileNameForEntry(entry);
                    if (!string.Equals(filePath, expectedPath))
                    {
                        Debug.LogError($"Loaded {_typeName} path does not match its expected path:\n" +
                                       $"   Actual: {filePath}\n" +
                                       $"   Expected: {expectedPath}");
                    }
                }
            }

            _entriesOnDisk = allEntries;
        }

        private bool SaveEntryToDisk(Func<bool> overrideExisting)
        {
            if (_currentEntry == null)
                return false;

            string entryPath = GetFileNameForEntry(_currentEntry);

            if (File.Exists(entryPath))
            {
                if (!overrideExisting?.Invoke() ?? false)
                    return false;

                File.SetAttributes(entryPath, FileAttributes.Normal);
            }

            JsonSerializerSettings jsonSettings = _sharedContext.Get<JsonSerializerSettings>();
            string entryText = JsonConvert.SerializeObject(_currentEntry, jsonSettings);
            if (string.IsNullOrWhiteSpace(entryText))
                return false;

            File.WriteAllText(entryPath, entryText);

            UpdateEntriesFromDisk();
            return true;
        }

        private void OnEnable()
        {
            _dataDrawer = DataDrawerFinder.Find<EntryType>();
            _sharedContext = EditorDataLoadHelper.LoadEditorContext();
            _currentEntry = null;
            _entriesOnDisk = null;

            UpdateEntriesFromDisk();
        }

        private void OnGUI()
        {
            _scrollPos = GUILayout.BeginScrollView(_scrollPos);
            GUILayout.BeginVertical();
            {
                OnGUI_DrawHeader();

                OnGUI_DrawToolbar();

                if (_currentEntry != null)
                {
                    OnGUI_DrawEntry(_currentEntry);
                }
                else
                {
                    EditorGUILayout.HelpBox($"Nothing selected.\nCreate or load an {_typeName} using the toolbar above.", MessageType.Info);
                }
            }
            GUILayout.EndVertical();
            GUILayout.EndScrollView();
        }

        private void OnGUI_DrawHeader()
        {
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label($"{_typeName} Editor", new GUIStyle(GUI.skin.label) {fontStyle = FontStyle.Bold, fontSize = GUI.skin.label.fontSize + 5});
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
                    if (_entriesOnDisk.Count == 0)
                    {
                        gm.AddDisabledItem(new GUIContent($"No {_typeName} Entries Found"));
                    }
                    else
                    {
                        void SelectEntry(object entryObj)
                        {
                            if (entryObj is EntryType entry)
                                _currentEntry = entry;
                        }

                        foreach (EntryType entry in _entriesOnDisk.OrderBy(x => x.Id))
                        {
                            EntryType entryCache = entry;
                            GUIContent content = GetLoadDropdownContent(entry);
                            bool active = _currentEntry?.Id == entry?.Id;

                            gm.AddItem(content, active, SelectEntry, entryCache);
                        }
                    }

                    gm.ShowAsContext();
                }

                if (GUILayout.Button("New"))
                {
                    _currentEntry = new EntryType();
                }
            }
            GUILayout.EndHorizontal();

            // Buttons if Loaded
            if (_currentEntry != null)
            {
                GUILayout.BeginHorizontal();
                {
                    if (GUILayout.Button("Clear"))
                    {
                        _currentEntry = null;
                    }

                    GUI.enabled = CanSave(_currentEntry);
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

        private void OnGUI_DrawEntry(EntryType entry)
        {
            GUILayout.BeginVertical(GUI.skin.box);
            {
                if (_dataDrawer != null)
                {
                    _dataDrawer.DrawGUI(entry, _sharedContext, null);
                }
                else
                {
                    EditorGUILayout.HelpBox(
                        $"No corresponding DataDrawer could be found for {_typeName}!",
                        MessageType.Error);
                }
            }
            GUILayout.EndVertical();
        }
    }
}
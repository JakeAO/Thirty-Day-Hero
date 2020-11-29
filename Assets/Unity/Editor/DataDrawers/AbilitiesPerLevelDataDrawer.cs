using System;
using System.Collections.Generic;
using System.Linq;
using Core.Abilities;
using Core.Database;
using Core.Etc;
using SadPumpkin.Util.Context;
using UnityEditor;
using UnityEngine;

namespace Unity.Editor.DataDrawers
{
    public class AbilitiesPerLevelDataDrawer : IDataDrawer<IReadOnlyDictionary<uint, IReadOnlyCollection<IAbility>>>
    {
        public void DrawGUI(IReadOnlyDictionary<uint, IReadOnlyCollection<IAbility>> value, IContext context, Action<IReadOnlyDictionary<uint, IReadOnlyCollection<IAbility>>> lateRef)
        {
            AbilityDatabase abilityDatabase = context.Get<AbilityDatabase>();

            GUILayout.BeginVertical();
            {
                var mutableDict = value.ToSorted();
                
                // Add New Level Category
                GUI.color = Color.green;
                if (GUILayout.Button("Add Level", GUILayout.ExpandWidth(false)))
                {
                    void SelectLevel(object levelObj)
                    {
                        if (levelObj is uint level)
                        {
                            mutableDict[level] = new List<IAbility>();
                            lateRef?.Invoke(mutableDict);
                        }
                    }

                    GenericMenu gm = new GenericMenu();
                    foreach (int entry in Enumerable.Range(0, 20))
                    {
                        uint entryCache = (uint) entry;
                        GUIContent content = new GUIContent($"Lvl {entry.ToString()}");
                        bool active = value.ContainsKey(entryCache);

                        if (active)
                            gm.AddDisabledItem(content, true);
                        else
                            gm.AddItem(content, false, SelectLevel, entryCache);
                    }
                    gm.ShowAsContext();
                }
                GUI.color = Color.white;
                
                // Foreach Level List
                uint? removeLevel = null;
                foreach (uint level in value.Keys)
                {
                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.Label($"Level {level}");
                        GUILayout.BeginVertical(GUI.skin.box);
                        {
                            GUILayout.BeginHorizontal();
                            {
                                // Add new ability to level
                                GUI.color = Color.green;
                                if (GUILayout.Button("+", GUILayout.ExpandWidth(false)))
                                {
                                    void SelectAbility(object addAbilityObj)
                                    {
                                        if (addAbilityObj is IAbility addAbility)
                                        {
                                            (mutableDict[level] as List<IAbility>).Add(addAbility);
                                            lateRef?.Invoke(value);
                                        }
                                    }

                                    GenericMenu gm = new GenericMenu();
                                    foreach (IAbility entry in abilityDatabase.EnumerateAll())
                                    {
                                        IAbility entryCache = entry;
                                        GUIContent content = new GUIContent($"{entry.Name} ({entry.Id}) - {entry.Desc}");
                                        bool active = mutableDict[level].Any(x => x.Id == entry.Id);

                                        if (active)
                                            gm.AddDisabledItem(content, true);
                                        else
                                            gm.AddItem(content, false, SelectAbility, entryCache);
                                    }

                                    gm.ShowAsContext();
                                }

                                GUI.color = Color.white;

                                GUILayout.FlexibleSpace();

                                // Remove level entirely
                                GUI.color = Color.red;
                                if (GUILayout.Button("Remove Level", GUILayout.ExpandWidth(false)))
                                {
                                    removeLevel = level;
                                }

                                GUI.color = Color.white;
                            }
                            GUILayout.EndHorizontal();

                            GUILayout.BeginVertical(GUI.skin.box);
                            {
                                // Foreach ability in level
                                IAbility removeAbility = null;
                                foreach (IAbility ability in mutableDict[level])
                                {
                                    GUILayout.BeginHorizontal();
                                    GUILayout.Label($"{ability.Id} \"{ability.Name}\"");
                                    GUI.color = Color.red;
                                    if (GUILayout.Button("X", GUILayout.ExpandWidth(false)))
                                    {
                                        removeAbility = ability;
                                    }

                                    GUI.color = Color.white;
                                    GUILayout.EndHorizontal();
                                }

                                if (removeAbility != null)
                                {
                                    (mutableDict[level] as List<IAbility>).Remove(removeAbility);
                                    lateRef?.Invoke(value);
                                }
                            }
                            GUILayout.EndVertical();
                        }
                        GUILayout.EndVertical();
                    }
                    GUILayout.EndHorizontal();
                }

                if (removeLevel != null)
                {
                    mutableDict.Remove(removeLevel.Value);
                    lateRef?.Invoke(mutableDict);
                }
            }
            GUILayout.EndVertical();
        }
    }
}
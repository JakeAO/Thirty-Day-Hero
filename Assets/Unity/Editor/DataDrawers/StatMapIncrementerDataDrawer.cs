using System;
using System.Collections.Generic;
using Core.Etc;
using Core.StatMap;
using SadPumpkin.Util.Context;
using Unity.Editor.WindowUtilities;
using UnityEditor;
using UnityEngine;

namespace Unity.Editor.DataDrawers
{
    public class StatMapIncrementerDataDrawer : IDataDrawer<IStatMapIncrementor>
    {
        public void DrawGUI(IStatMapIncrementor value, IContext context, Action<IStatMapIncrementor> lateRef)
        {
            // Settings Change
            GUILayout.BeginVertical(GUI.skin.box);
            {
                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label(value.GetType().Name, new GUIStyle(GUI.skin.label) {fontStyle = FontStyle.Bold});
                }
                GUILayout.EndHorizontal();

                switch (value)
                {
                    case StatMapIncrementor statMapIncrementor:
                    {
                        statMapIncrementor.Total = (uint) EditorGUILayout.IntField("Total:", (int) statMapIncrementor.Total);
                        statMapIncrementor.MinPerStat = (uint) EditorGUILayout.IntField("Min Per Stat:", (int) statMapIncrementor.MinPerStat);
                        
                        GUILayout.BeginHorizontal();
                        {
                            for (StatType statType = StatType.STR; statType <= StatType.CHA; statType++)
                            {
                                GUILayout.BeginVertical(GUI.skin.box);
                                {
                                    GUILayout.Label(statType.ToString(), new GUIStyle(GUI.skin.label) {fontStyle = FontStyle.Bold});
                                    statMapIncrementor.Priorities[statType] = (RankPriority) EditorGUILayout.EnumPopup(statMapIncrementor.Priorities[statType]);
                                }
                                GUILayout.EndVertical();
                            }
                        }
                        GUILayout.EndHorizontal();
                        
                        break;
                    }
                    case NullStatMapIncrementor nullStatMapIncrementor:
                    {
                        EditorGUILayout.HelpBox(
                            "No configuration available.",
                            MessageType.Info);
                        break;
                    }
                    default:
                    {
                        EditorGUILayout.HelpBox(
                            "Unhandled Type!.",
                            MessageType.Error);
                        break;
                    }
                }
            }
            GUILayout.EndVertical();

            // Type Change
            if (GUILayout.Button("[CHANGE]", GUILayout.ExpandWidth(false)))
            {
                IReadOnlyDictionary<Type, Func<IStatMapIncrementor>> implementations = ImplementationFinder.Find<IStatMapIncrementor>();

                GenericMenu gm = new GenericMenu();
                if (implementations.Count == 0)
                {
                    gm.AddDisabledItem(new GUIContent($"No {nameof(IStatMapIncrementor)} implementations found"));
                }
                else
                {
                    foreach (var implementationKvp in implementations)
                    {
                        void SetImplementation(object funcObj)
                        {
                            if (funcObj is Func<IStatMapIncrementor> func)
                                lateRef?.Invoke(func());
                        }

                        Func<IStatMapIncrementor> funcCache = implementationKvp.Value;
                        GUIContent content = new GUIContent(implementationKvp.Key.Name);
                        bool active = value.GetType() == implementationKvp.Key;

                        gm.AddItem(content, active, SetImplementation, funcCache);
                    }
                }
                gm.ShowAsContext();
            }
        }
    }
}
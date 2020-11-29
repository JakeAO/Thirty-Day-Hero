using System;
using System.Collections.Generic;
using Core.Costs;
using Core.Etc;
using Core.StatMap;
using SadPumpkin.Util.CombatEngine.CostCalculators;
using SadPumpkin.Util.Context;
using Unity.Editor.WindowUtilities;
using UnityEditor;
using UnityEngine;

namespace Unity.Editor.DataDrawers
{
    public class StatMapBuilderDataDrawer : IDataDrawer<IStatMapBuilder>
    {
        public void DrawGUI(IStatMapBuilder value, IContext context, Action<IStatMapBuilder> lateRef)
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
                    case StatMapBuilder statMapBuilder:
                    {
                        statMapBuilder.Total = (uint) EditorGUILayout.IntField("Total:", (int) statMapBuilder.Total);
                        statMapBuilder.MinPerStat = (uint) EditorGUILayout.IntField("Min Per Stat:", (int) statMapBuilder.MinPerStat);
                        
                        GUILayout.BeginHorizontal();
                        {
                            for (StatType statType = StatType.STR; statType <= StatType.CHA; statType++)
                            {
                                GUILayout.BeginVertical(GUI.skin.box);
                                {
                                    GUILayout.Label(statType.ToString(), new GUIStyle(GUI.skin.label) {fontStyle = FontStyle.Bold});
                                    statMapBuilder.Priorities[statType] = (RankPriority) EditorGUILayout.EnumPopup(statMapBuilder.Priorities[statType]);
                                }
                                GUILayout.EndVertical();
                            }
                        }
                        GUILayout.EndHorizontal();
                        
                        break;
                    }
                    case NullStatMapBuilder nullStatMapBuilder:
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
                IReadOnlyDictionary<Type, Func<IStatMapBuilder>> implementations = ImplementationFinder.Find<IStatMapBuilder>();

                GenericMenu gm = new GenericMenu();
                if (implementations.Count == 0)
                {
                    gm.AddDisabledItem(new GUIContent($"No {nameof(IStatMapBuilder)} implementations found"));
                }
                else
                {
                    foreach (var implementationKvp in implementations)
                    {
                        void SetImplementation(object funcObj)
                        {
                            if (funcObj is Func<IStatMapBuilder> func)
                                lateRef?.Invoke(func());
                        }

                        Func<IStatMapBuilder> funcCache = implementationKvp.Value;
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
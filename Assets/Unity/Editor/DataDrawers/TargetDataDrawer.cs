using System;
using System.Collections.Generic;
using SadPumpkin.Util.CombatEngine.TargetCalculators;
using SadPumpkin.Util.Context;
using Unity.Editor.WindowUtilities;
using UnityEditor;
using UnityEngine;

namespace Unity.Editor.DataDrawers
{
    public class TargetDataDrawer : IDataDrawer<ITargetCalc>
    {
        public void DrawGUI(ITargetCalc value, IContext context, Action<ITargetCalc> lateRef)
        {
            // Settings Change
            GUILayout.BeginVertical(GUI.skin.box);
            {
                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label(value.GetType().Name, new GUIStyle(GUI.skin.label) {fontStyle = FontStyle.Bold});
                    GUILayout.Label($"\"Target: {value.Description}\"", new GUIStyle(GUI.skin.label) {fontStyle = FontStyle.Italic});
                }
                GUILayout.EndHorizontal();
                
                switch (value)
                {
                    case SelfTargetCalculator selfTargetCalculator:
                    case SingleAllyTargetCalculator singleAllyTargetCalculator:
                    case SingleEnemyTargetCalculator singleEnemyTargetCalculator:
                    case AllAllyTargetCalculator allAllyTargetCalculator:
                    case AllEnemyTargetCalculator allEnemyTargetCalculator:
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
                IReadOnlyDictionary<Type, Func<ITargetCalc>> implementations = ImplementationFinder.Find<ITargetCalc>();

                GenericMenu gm = new GenericMenu();
                if (implementations.Count == 0)
                {
                    gm.AddDisabledItem(new GUIContent("No ITargetCalc implementations found"));
                }
                else
                {
                    foreach (var implementationKvp in implementations)
                    {
                        void SetImplementation(object funcObj)
                        {
                            if (funcObj is Func<ITargetCalc> func)
                                lateRef?.Invoke(func());
                        }

                        Func<ITargetCalc> funcCache = implementationKvp.Value;
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
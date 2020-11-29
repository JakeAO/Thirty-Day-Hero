using System;
using System.Collections.Generic;
using Core.Costs;
using Core.Etc;
using SadPumpkin.Util.CombatEngine.CostCalculators;
using SadPumpkin.Util.Context;
using Unity.Editor.WindowUtilities;
using UnityEditor;
using UnityEngine;

namespace Unity.Editor.DataDrawers
{
    public class CostDataDrawer : IDataDrawer<ICostCalc>
    {
        public void DrawGUI(ICostCalc value, IContext context, Action<ICostCalc> lateRef)
        {
            // Settings Change
            GUILayout.BeginVertical(GUI.skin.box);
            {
                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label(value.GetType().Name, new GUIStyle(GUI.skin.label) {fontStyle = FontStyle.Bold});
                    GUILayout.Label(value.Description(), new GUIStyle(GUI.skin.label) {fontStyle = FontStyle.Italic});
                }
                GUILayout.EndHorizontal();
                
                switch (value)
                {
                    case StatCost statCost:
                    {
                        statCost.Type = (StatType) EditorGUILayout.EnumPopup("Stat:", statCost.Type);
                        statCost.Amount = (uint) EditorGUILayout.IntField("Amount:", (int) statCost.Amount);
                        break;
                    }
                    case DestroyThisItemCost destroyThisItemCost:
                    {
                        EditorGUILayout.HelpBox(
                            "No configuration available.",
                            MessageType.Info);
                        break;
                    }
                    case NoCost noCost:
                    {
                        EditorGUILayout.HelpBox(
                            "No configuration available.",
                            MessageType.Info);
                        break;
                    }
                }
            }
            GUILayout.EndVertical();

            // Type Change
            if (GUILayout.Button("[CHANGE]", GUILayout.ExpandWidth(false)))
            {
                IReadOnlyDictionary<Type, Func<ICostCalc>> implementations = ImplementationFinder.Find<ICostCalc>();

                GenericMenu gm = new GenericMenu();
                if (implementations.Count == 0)
                {
                    gm.AddDisabledItem(new GUIContent("No ICostCalc implementations found"));
                }
                else
                {
                    foreach (var implementationKvp in implementations)
                    {
                        void SetImplementation(object funcObj)
                        {
                            if (funcObj is Func<ICostCalc> func)
                                lateRef?.Invoke(func());
                        }

                        Func<ICostCalc> funcCache = implementationKvp.Value;
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
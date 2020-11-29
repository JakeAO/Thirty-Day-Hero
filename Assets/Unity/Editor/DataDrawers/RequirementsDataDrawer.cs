using System;
using System.Collections.Generic;
using Core.Etc;
using Core.Requirements;
using SadPumpkin.Util.CombatEngine.RequirementCalculators;
using SadPumpkin.Util.Context;
using Unity.Editor.WindowUtilities;
using UnityEditor;
using UnityEngine;

namespace Unity.Editor.DataDrawers
{
    public class RequirementsDataDrawer : IDataDrawer<IRequirementCalc>
    {
        public void DrawGUI(IRequirementCalc value, IContext context, Action<IRequirementCalc> lateRef)
        {
            // Settings Change
            GUILayout.BeginVertical(GUI.skin.box);
            {
                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label(value.GetType().Name, new GUIStyle(GUI.skin.label) {fontStyle = FontStyle.Bold});
                    GUILayout.Label($"\"Requirement: {value.Description}\"", new GUIStyle(GUI.skin.label) {fontStyle = FontStyle.Italic});
                }
                GUILayout.EndHorizontal();
                
                switch (value)
                {
                    case EquippedWeaponRequirement equippedWeaponRequirement:
                    {
                        equippedWeaponRequirement.RequiredType = (WeaponType) EditorGUILayout.EnumFlagsField("Type:", equippedWeaponRequirement.RequiredType);
                        break;
                    }
                    case EquippedArmorRequirement equippedArmorRequirement:
                    {
                        equippedArmorRequirement.RequiredType = (ArmorType) EditorGUILayout.EnumFlagsField("Type:", equippedArmorRequirement.RequiredType);
                        break;
                    }
                    case CriticalHealthRequirement criticalHealthRequirement:
                    case NoRequirements noRequirements:
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
                IReadOnlyDictionary<Type, Func<IRequirementCalc>> implementations = ImplementationFinder.Find<IRequirementCalc>();

                GenericMenu gm = new GenericMenu();
                if (implementations.Count == 0)
                {
                    gm.AddDisabledItem(new GUIContent("No IRequirementCalc implementations found"));
                }
                else
                {
                    foreach (var implementationKvp in implementations)
                    {
                        void SetImplementation(object funcObj)
                        {
                            if (funcObj is Func<IRequirementCalc> func)
                                lateRef?.Invoke(func());
                        }

                        Func<IRequirementCalc> funcCache = implementationKvp.Value;
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
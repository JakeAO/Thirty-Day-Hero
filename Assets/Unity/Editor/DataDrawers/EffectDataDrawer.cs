using System;
using System.Collections.Generic;
using Core.Effects;
using Core.Etc;
using SadPumpkin.Util.CombatEngine.EffectCalculators;
using SadPumpkin.Util.Context;
using Unity.Editor.WindowUtilities;
using UnityEditor;
using UnityEngine;

namespace Unity.Editor.DataDrawers
{
    public class EffectDataDrawer : IDataDrawer<IEffectCalc>
    {
        public void DrawGUI(IEffectCalc value, IContext context, Action<IEffectCalc> lateRef)
        {
            // Settings Change
            GUILayout.BeginVertical(GUI.skin.box);
            {
                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label(value.GetType().Name, new GUIStyle(GUI.skin.label) {fontStyle = FontStyle.Bold});
                    GUILayout.Label($"\"Effect: {value.Description}\"", new GUIStyle(GUI.skin.label) {fontStyle = FontStyle.Italic});
                }
                GUILayout.EndHorizontal();
                
                switch (value)
                {
                    case StatEffect statEffect:
                    {
                        statEffect.Stat = (StatType) EditorGUILayout.EnumPopup("Affected Stat:", statEffect.Stat);
                        statEffect.BaseAmount = EditorGUILayout.IntField("Base Amount:", statEffect.BaseAmount);
                        statEffect.VariableAmount = EditorGUILayout.IntField("Random Amount:", statEffect.VariableAmount);
                        statEffect.EffectType = (DamageType) EditorGUILayout.EnumPopup("Effect Type:", statEffect.EffectType);
                        statEffect.ScalingStat = (StatType) EditorGUILayout.EnumPopup("Scale w/ Stat:", statEffect.ScalingStat);
                        statEffect.ScalingRank = (RankPriority) EditorGUILayout.EnumPopup("Scaling Rank:", statEffect.ScalingRank);
                        break;
                    }
                    case CombinedEffect combinedEffect:
                    {
                        for (int i = 0; i < combinedEffect.ChildEffects.Count; i++)
                        {
                            IEffectCalc childEffect = combinedEffect.ChildEffects[i];
                            
                            GUILayout.BeginHorizontal();
                            int cacheIndex = i;
                            DrawGUI(childEffect, context, newChild => combinedEffect.ChildEffects[cacheIndex] = newChild);
                            GUI.color = Color.red;
                            if (GUILayout.Button("X", GUILayout.ExpandWidth(false)))
                            {
                                combinedEffect.ChildEffects.RemoveAt(i);
                                i--;
                            }
                            GUI.color = Color.white;
                            GUILayout.EndHorizontal();
                        }

                        GUILayout.BeginHorizontal();
                        GUILayout.FlexibleSpace();
                        GUI.color = Color.green;
                        if (GUILayout.Button("+", GUILayout.ExpandWidth(false)))
                        {
                            combinedEffect.ChildEffects.Add(new NoEffect());
                        }
                        GUI.color = Color.white;
                        GUILayout.EndHorizontal();
                        
                        break;
                    }
                    case NoEffect noEffect:
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
                IReadOnlyDictionary<Type, Func<IEffectCalc>> implementations = ImplementationFinder.Find<IEffectCalc>();

                GenericMenu gm = new GenericMenu();
                if (implementations.Count == 0)
                {
                    gm.AddDisabledItem(new GUIContent("No IEffectCalc implementations found"));
                }
                else
                {
                    foreach (var implementationKvp in implementations)
                    {
                        void SetImplementation(object funcObj)
                        {
                            if (funcObj is Func<IEffectCalc> func)
                                lateRef?.Invoke(func());
                        }

                        Func<IEffectCalc> funcCache = implementationKvp.Value;
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
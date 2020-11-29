using System;
using System.Collections.Generic;
using Core.Etc;
using SadPumpkin.Util.Context;
using UnityEditor;
using UnityEngine;

namespace Unity.Editor.DataDrawers
{
    public class DamageModListDataDrawer : IDataDrawer<IReadOnlyDictionary<DamageType, float>>
    {
        public void DrawGUI(IReadOnlyDictionary<DamageType, float> value, IContext context, Action<IReadOnlyDictionary<DamageType, float>> lateRef)
        {
            GUILayout.BeginVertical();
            {
                var mutableDict = value.ToMutable();

                GUI.color = Color.green;
                if (GUILayout.Button("+", GUILayout.ExpandWidth(false)))
                {
                    void SelectDamageType(object damageTypeObj)
                    {
                        if (damageTypeObj is DamageType damageType)
                        {
                            mutableDict[damageType] = 1f;
                            lateRef?.Invoke(mutableDict);
                        }
                    }

                    GenericMenu gm = new GenericMenu();
                    foreach (DamageType entry in Enum.GetValues(typeof(DamageType)))
                    {
                        if (entry == DamageType.Invalid)
                            continue;
                        DamageType entryCache = entry;
                        GUIContent content = new GUIContent(entry.ToString());
                        bool active = value.ContainsKey(entry);

                        if (active)
                            gm.AddDisabledItem(content, true);
                        else
                            gm.AddItem(content, false, SelectDamageType, entryCache);
                    }
                    gm.ShowAsContext();
                }
                GUI.color = Color.white;

                DamageType removeType = DamageType.Invalid;
                foreach (DamageType damageType in value.Keys)
                {
                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.Label($"{damageType} X", GUILayout.ExpandWidth(false));
                        
                        EditorGUI.BeginChangeCheck();
                        {
                            float mod = mutableDict[damageType];
                            mod = EditorGUILayout.Slider(mod, 0f, 2f);
                            mutableDict[damageType] = mod;
                        }
                        if (EditorGUI.EndChangeCheck())
                        {
                            lateRef?.Invoke(mutableDict);
                        }

                        GUI.color = Color.red;
                        if (GUILayout.Button("X", GUILayout.ExpandWidth(false)))
                        {
                            removeType = damageType;
                        }
                        GUI.color = Color.white;
                    }
                    GUILayout.EndHorizontal();
                }

                if (removeType != DamageType.Invalid)
                {
                    mutableDict.Remove(removeType);
                    lateRef?.Invoke(mutableDict);
                }
            }
            GUILayout.EndVertical();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using Core.Abilities;
using Core.Database;
using SadPumpkin.Util.Context;
using UnityEditor;
using UnityEngine;

namespace Unity.Editor.DataDrawers
{
    public class AbilityListDataDrawer : IDataDrawer<IReadOnlyCollection<IAbility>>
    {
        public void DrawGUI(IReadOnlyCollection<IAbility> value, IContext context, Action<IReadOnlyCollection<IAbility>> lateRef)
        {
            GUILayout.BeginVertical();
            {
                GUI.color = Color.green;
                if (GUILayout.Button("+", GUILayout.ExpandWidth(false)))
                {
                    void SelectAbility(object addAbilityObj)
                    {
                        if (addAbilityObj is IAbility addAbility)
                        {
                            value = new List<IAbility>(value) {addAbility};
                            lateRef?.Invoke(value);
                        }
                    }

                    AbilityDatabase abilityDatabase = context.Get<AbilityDatabase>();

                    GenericMenu gm = new GenericMenu();
                    foreach (IAbility entry in abilityDatabase.EnumerateAll())
                    {
                        IAbility entryCache = entry;
                        GUIContent content = new GUIContent($"{entry.Name} ({entry.Id}) - {entry.Desc}");
                        bool active = value.Any(x => x.Id == entry.Id);

                        if (active)
                            gm.AddDisabledItem(content, true);
                        else
                            gm.AddItem(content, false, SelectAbility, entryCache);
                    }
                    gm.ShowAsContext();
                }
                GUI.color = Color.white;

                IAbility removeAbility = null;
                foreach (IAbility addedAbility in value)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Label($"{addedAbility.Id} \"{addedAbility.Name}\"");
                    GUI.color = Color.red;
                    if (GUILayout.Button("X", GUILayout.ExpandWidth(false)))
                    {
                        removeAbility = addedAbility;
                    }
                    GUI.color = Color.white;
                    GUILayout.EndHorizontal();
                }

                if (removeAbility != null)
                {
                    List<IAbility> abilities = new List<IAbility>(value);
                    abilities.Remove(removeAbility);
                    value = abilities;
                    lateRef?.Invoke(value);
                }
            }
            GUILayout.EndVertical();
        }
    }
}
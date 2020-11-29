using System;
using System.Collections.Generic;
using System.Linq;
using Core.Database;
using Core.EquipMap;
using Core.Etc;
using Core.Items;
using Core.Items.Armors;
using Core.Items.Weapons;
using SadPumpkin.Util.Context;
using Unity.Editor.WindowUtilities;
using UnityEditor;
using UnityEngine;

namespace Unity.Editor.DataDrawers
{
    public class EquipMapBuilderDataDrawer : IDataDrawer<IEquipMapBuilder>
    {
        public void DrawGUI(IEquipMapBuilder value, IContext context, Action<IEquipMapBuilder> lateRef)
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
                    case EquipMapBuilder equipMapBuilder:
                    {
                        void DrawStartingEquipment<T>(List<(T, RankPriority)> list,
                            WeaponDatabase weaponDatabase, ArmorDatabase armorDatabase, ItemDatabase itemDatabase)
                            where T : IItem
                        {
                            GUILayout.BeginVertical(GUI.skin.box);
                            {
                                for(int i = 0; i < list.Count; i++)
                                {
                                    (T item, RankPriority priority) = list[i];
                                    
                                    GUILayout.BeginHorizontal();
                                    GUILayout.Label($"{item.Id} \"{item.Name}\"");
                                    priority = (RankPriority) EditorGUILayout.EnumPopup(priority);
                                    GUI.color = Color.red;
                                    if (GUILayout.Button("X", GUILayout.ExpandWidth(false)))
                                    {
                                        list.RemoveAt(i);
                                        i--;
                                    }
                                    else
                                    {
                                        list[i] = (item, priority);
                                    }
                                    GUI.color = Color.white;
                                    GUILayout.EndHorizontal();
                                }

                                void OnAddItem(object itemObj)
                                {
                                    if (itemObj is T itemT)
                                    {
                                        list.Add((itemT, RankPriority.C));
                                    }
                                }
                                
                                GUI.color = Color.green;
                                if (GUILayout.Button("+", GUILayout.ExpandWidth(false)))
                                {
                                    GenericMenu gm = new GenericMenu();
                                    if (weaponDatabase != null)
                                    {
                                        foreach (IWeapon entry in weaponDatabase.EnumerateAll())
                                        {
                                            IWeapon entryCache = entry;
                                            GUIContent content = new GUIContent(
                                                entry.Name,
                                                entry.Desc);

                                            gm.AddItem(content, false, OnAddItem, entryCache);
                                        }
                                    }
                                    if (armorDatabase != null)
                                    {
                                        foreach (IArmor entry in armorDatabase.EnumerateAll())
                                        {
                                            IArmor entryCache = entry;
                                            GUIContent content = new GUIContent(
                                                entry.Name,
                                                entry.Desc);

                                            gm.AddItem(content, false, OnAddItem, entryCache);
                                        }
                                    }
                                    if (itemDatabase != null)
                                    {
                                        foreach (IItem entry in itemDatabase.EnumerateAll())
                                        {
                                            IItem entryCache = entry;
                                            GUIContent content = new GUIContent(
                                                entry.Name,
                                                entry.Desc);

                                            gm.AddItem(content, false, OnAddItem, entryCache);
                                        }
                                    }
                                    gm.ShowAsContext();
                                }
                                GUI.color = Color.white;
                            }
                            GUILayout.EndVertical();
                        }

                        WeaponDatabase wData = context.Get<WeaponDatabase>();
                        ArmorDatabase aData = context.Get<ArmorDatabase>();
                        ItemDatabase iData = context.Get<ItemDatabase>();

                        GUILayout.Label("Weapon", new GUIStyle(GUI.skin.label) {fontStyle = FontStyle.Bold});
                        DrawStartingEquipment(equipMapBuilder.StartingWeapon, wData, null, null);
                        
                        GUILayout.Label("Armor", new GUIStyle(GUI.skin.label) {fontStyle = FontStyle.Bold});
                        DrawStartingEquipment(equipMapBuilder.StartingArmor, null, aData, null);
                        
                        GUILayout.Label("Item 1", new GUIStyle(GUI.skin.label) {fontStyle = FontStyle.Bold});
                        DrawStartingEquipment(equipMapBuilder.StartingItemA, wData, aData, iData);
                        
                        GUILayout.Label("Item 2", new GUIStyle(GUI.skin.label) {fontStyle = FontStyle.Bold});
                        DrawStartingEquipment(equipMapBuilder.StartingItemB, wData, aData, iData);
                        break;
                    }
                    case NullEquipMapBuilder nullEquipMapBuilder:
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
                IReadOnlyDictionary<Type, Func<IEquipMapBuilder>> implementations = ImplementationFinder.Find<IEquipMapBuilder>();

                GenericMenu gm = new GenericMenu();
                if (implementations.Count == 0)
                {
                    gm.AddDisabledItem(new GUIContent($"No {nameof(IEquipMapBuilder)} implementations found"));
                }
                else
                {
                    foreach (var implementationKvp in implementations)
                    {
                        void SetImplementation(object funcObj)
                        {
                            if (funcObj is Func<IEquipMapBuilder> func)
                                lateRef?.Invoke(func());
                        }

                        Func<IEquipMapBuilder> funcCache = implementationKvp.Value;
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
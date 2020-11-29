using System;
using Core.Classes.Enemy;
using Core.Database;
using Core.Etc;
using Core.Wrappers;
using SadPumpkin.Util.Context;
using UnityEditor;
using UnityEngine;

namespace Unity.Editor.DataDrawers
{
    public class EnemyGroupDataDrawer : IDataDrawer<EnemyGroupWrapper>
    {
        public void DrawGUI(EnemyGroupWrapper value, IContext context, Action<EnemyGroupWrapper> lateRef)
        {
            value.Id = (uint) EditorGUILayout.IntField("Id:", (int) value.Id);
            value.Name = EditorGUILayout.TextField("Name:", value.Name);
            value.Desc = EditorGUILayout.TextField("Description:", value.Desc);
            value.Rarity = (RarityCategory) EditorGUILayout.EnumPopup("Rarity:", value.Rarity);
            
            GUILayout.BeginHorizontal();
            GUILayout.BeginVertical();
            {
                GUILayout.Label("Enemy Types:", GUILayout.ExpandWidth(false));
                
                GUI.color = Color.green;
                if (GUILayout.Button("+", GUILayout.ExpandWidth(false)))
                {
                    void SelectAbility(object addObj)
                    {
                        if (addObj is IEnemyClass add)
                        {
                            value.EnemyTypes.Add(add);
                        }
                    }

                    EnemyClassDatabase enemyClassDatabase = context.Get<EnemyClassDatabase>();

                    GenericMenu gm = new GenericMenu();
                    foreach (IEnemyClass entry in enemyClassDatabase.EnumerateAll())
                    {
                        IEnemyClass entryCache = entry;
                        GUIContent content = new GUIContent($"{entry.Name} ({entry.Id}) - {entry.Desc}");
                        bool active = value.EnemyTypes.Exists(x => x.Id == entry.Id);

                        gm.AddItem(content, active, SelectAbility, entryCache);
                    }
                    gm.ShowAsContext();
                }
                GUI.color = Color.white;
            }
            GUILayout.EndVertical();
            GUILayout.BeginVertical(GUI.skin.box);
            {
                for(int i = 0; i < value.EnemyTypes.Count; i++)
                {
                    IEnemyClass enemyClass = value.EnemyTypes[i];
                    
                    GUILayout.BeginHorizontal();
                    GUILayout.Label($"{enemyClass.Id} \"{enemyClass.Name}\"");
                    GUI.color = Color.red;
                    if (GUILayout.Button("X", GUILayout.ExpandWidth(false)))
                    {
                        value.EnemyTypes.RemoveAt(i);
                        i--;
                    }
                    GUI.color = Color.white;
                    GUILayout.EndHorizontal();
                }
            }
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
        }
    }
}
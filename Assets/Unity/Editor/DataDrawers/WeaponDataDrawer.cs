using System;
using System.Collections.Generic;
using System.Linq;
using Core.Abilities;
using Core.Database;
using Core.Etc;
using Core.Items.Weapons;
using SadPumpkin.Util.Context;
using UnityEditor;
using UnityEngine;

namespace Unity.Editor.DataDrawers
{
    public class WeaponDataDrawer : IDataDrawer<Weapon>
    {
        private readonly IDataDrawer<IReadOnlyCollection<IAbility>> _abilityListDrawer = null;

        public WeaponDataDrawer()
        {
            _abilityListDrawer = DataDrawerFinder.Find<IReadOnlyCollection<IAbility>>();
        }

        public void DrawGUI(Weapon value, IContext context, Action<Weapon> lateRef)
        {
            value.Id = (uint) EditorGUILayout.IntField("Id:", (int) value.Id);
            value.Name = EditorGUILayout.TextField("Name:", value.Name);
            value.Desc = EditorGUILayout.TextField("Description:", value.Desc);
            value.Rarity = (RarityCategory) EditorGUILayout.EnumPopup("Rarity:", value.Rarity);
            value.WeaponType = (WeaponType) EditorGUILayout.EnumPopup("Type:", value.WeaponType);
            value.BaseValue = (uint) EditorGUILayout.IntField("Value:", (int) value.BaseValue);

            GUILayout.BeginHorizontal();
            value.ArtPath = EditorGUILayout.TextField("Art Id:", value.ArtPath);
            Texture2D artTexture = AssetDatabase.LoadAssetAtPath<Texture2D>(value.ArtPath);
            Texture2D newArtTexture = (Texture2D) EditorGUILayout.ObjectField(artTexture, typeof(Texture2D), false, GUILayout.ExpandWidth(false));
            if (artTexture != newArtTexture)
            {
                value.ArtPath = AssetDatabase.GetAssetPath(newArtTexture);
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Attack:", GUILayout.ExpandWidth(false));
            if (value.AttackAbility == null)
            {
                EditorGUILayout.HelpBox("Select an Ability using the button to the right.", MessageType.Error);
            }
            else
            {
                GUILayout.BeginVertical(GUI.skin.box);
                GUILayout.Label($"{value.AttackAbility.Id} {value.AttackAbility.Name}");
                GUILayout.Label($"Cost: {value.AttackAbility.Cost.Description}");
                GUILayout.Label($"Requirement: {value.AttackAbility.Requirements.Description}");
                GUILayout.Label($"Target: {value.AttackAbility.Target.Description}");
                GUILayout.Label($"Effect: {value.AttackAbility.Effect.Description}");
                GUILayout.EndVertical();
            }
            if (GUILayout.Button("CHANGE", GUILayout.ExpandWidth(false)))
            {
                void SelectAbility(object abilityObj)
                {
                    if (abilityObj is IAbility ability)
                    {
                        value.AttackAbility = ability;
                    }
                }
                
                AbilityDatabase abilityDatabase = context.Get<AbilityDatabase>();
                    
                GenericMenu gm = new GenericMenu();
                foreach (IAbility entry in abilityDatabase.EnumerateAll())
                {
                    IAbility entryCache = entry;
                    GUIContent content = new GUIContent($"{entry.Name} ({entry.Id}) - {entry.Desc}");
                    bool active = value.AttackAbility?.Id == entry.Id;

                    if (active)
                        gm.AddDisabledItem(content, true);
                    else
                        gm.AddItem(content, false, SelectAbility, entryCache);
                }
                gm.ShowAsContext();
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Abilities:", GUILayout.ExpandWidth(false));
            if (_abilityListDrawer != null)
            {
                _abilityListDrawer.DrawGUI(value.AddedAbilities, context, newCalc => value.AddedAbilities = newCalc);
            }
            else
            {
                EditorGUILayout.HelpBox(
                    $"No corresponding DataDrawer could be found for {nameof(IReadOnlyCollection<IAbility>)}!",
                    MessageType.Error);
            }
            GUILayout.EndHorizontal();
        }
    }
}
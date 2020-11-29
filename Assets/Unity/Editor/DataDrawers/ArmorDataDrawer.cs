using System;
using System.Collections.Generic;
using Core.Abilities;
using Core.Etc;
using Core.Items.Armors;
using SadPumpkin.Util.Context;
using UnityEditor;
using UnityEngine;

namespace Unity.Editor.DataDrawers
{
    public class ArmorDataDrawer : IDataDrawer<Armor>
    {
        private readonly IDataDrawer<IReadOnlyCollection<IAbility>> _abilityListDrawer = null;
        private readonly IDataDrawer<IReadOnlyDictionary<DamageType, float>> _protectionDrawer = null;

        public ArmorDataDrawer()
        {
            _abilityListDrawer = DataDrawerFinder.Find<IReadOnlyCollection<IAbility>>();
            _protectionDrawer = DataDrawerFinder.Find<IReadOnlyDictionary<DamageType, float>>();
        }

        public void DrawGUI(Armor value, IContext context, Action<Armor> lateRef)
        {
            value.Id = (uint) EditorGUILayout.IntField("Id:", (int) value.Id);
            value.Name = EditorGUILayout.TextField("Name:", value.Name);
            value.Desc = EditorGUILayout.TextField("Description:", value.Desc);
            value.Rarity = (RarityCategory) EditorGUILayout.EnumPopup("Rarity:", value.Rarity);
            value.ArmorType = (ArmorType) EditorGUILayout.EnumPopup("Type:", value.ArmorType);
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
            GUILayout.Label("Protection:", GUILayout.ExpandWidth(false));
            if (_protectionDrawer != null)
            {
                _protectionDrawer.DrawGUI(value.DamageModifiers, context, newCalc => value.DamageModifiers = newCalc);
            }
            else
            {
                EditorGUILayout.HelpBox(
                    $"No corresponding DataDrawer could be found for {nameof(IReadOnlyDictionary<DamageType, float>)}!",
                    MessageType.Error);
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
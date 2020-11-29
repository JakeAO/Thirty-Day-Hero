using System;
using System.Collections.Generic;
using Core.Abilities;
using Core.Etc;
using Core.Items;
using SadPumpkin.Util.Context;
using UnityEditor;
using UnityEngine;

namespace Unity.Editor.DataDrawers
{
    public class ItemDataDrawer : IDataDrawer<Item>
    {
        private readonly IDataDrawer<IReadOnlyCollection<IAbility>> _abilityListDrawer = null;

        public ItemDataDrawer()
        {
            _abilityListDrawer = DataDrawerFinder.Find<IReadOnlyCollection<IAbility>>();
        }

        public void DrawGUI(Item value, IContext context, Action<Item> lateRef)
        {
            value.Id = (uint) EditorGUILayout.IntField("Id:", (int) value.Id);
            value.Name = EditorGUILayout.TextField("Name:", value.Name);
            value.Desc = EditorGUILayout.TextField("Description:", value.Desc);
            value.Rarity = (RarityCategory) EditorGUILayout.EnumPopup("Rarity:", value.Rarity);
            value.ItemType = (ItemType) EditorGUILayout.EnumPopup("Type:", value.ItemType);
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
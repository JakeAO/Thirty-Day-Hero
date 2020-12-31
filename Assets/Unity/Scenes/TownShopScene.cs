using System.Collections.Generic;
using Core.Abilities;
using Core.Etc;
using Core.Items;
using Core.Items.Armors;
using Core.Items.Weapons;
using Core.States.Town;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Unity.Scenes
{
    public class TownShopScene : SceneRootBase<TownShopState>
    {
        private readonly Dictionary<string, Texture2D> _itemSprites = new Dictionary<string, Texture2D>(10);

        protected override void OnGUIContentForState()
        {
            GUILayout.Label(State.ShopName, new GUIStyle(GUI.skin.label) {fontStyle = FontStyle.Bold, fontSize = GUI.skin.label.fontSize + 10});
        }

        protected override void OnGUIContentForContext(object context)
        {
            void DrawAbility(IAbility ability)
            {
                GUILayout.BeginVertical(GUI.skin.box);
                {
                    GUILayout.Label(ability.Desc, new GUIStyle(GUI.skin.label) {fontStyle = FontStyle.Italic});
                    GUILayout.Label($"Effect: {ability.Effect.Description}");
                    GUILayout.Label($"Target: {ability.Target.Description}");
                    GUILayout.Label($"Requires: {ability.Requirements.Description}");
                    GUILayout.Label($"Cost: {ability.Cost.Description}");
                }
                GUILayout.EndVertical();
            }
            
            if (context is IItem baseItem)
            {
                if (!_itemSprites.ContainsKey(baseItem.ArtPath))
                {
                    _itemSprites[baseItem.ArtPath] = null;
                    Addressables.LoadAssetAsync<Texture2D>(baseItem.ArtPath).Completed +=
                        handle => _itemSprites[baseItem.ArtPath] = handle.Result;
                }

                GUILayout.BeginVertical();
                {
                    // Icon and Name
                    GUILayout.BeginHorizontal(GUI.skin.box);
                    {
                        if (!_itemSprites.TryGetValue(baseItem.ArtPath, out Texture2D artTexture))
                            artTexture = Texture2D.whiteTexture;

                        GUILayout.Label(artTexture, GUILayout.MaxHeight(50), GUILayout.MaxWidth(50));

                        Color initColor = GUI.color;
                        switch (baseItem.Rarity)
                        {
                            case RarityCategory.Common:
                                GUI.color = Color.Lerp(initColor, Color.white, 0.75f);
                                break;
                            case RarityCategory.Uncommon:
                                GUI.color = Color.Lerp(initColor, Color.yellow, 0.75f);
                                break;
                            case RarityCategory.Rare:
                                GUI.color = Color.Lerp(initColor, Color.blue, 0.75f);
                                break;
                            case RarityCategory.Legendary:
                                GUI.color = Color.Lerp(initColor, Color.magenta, 0.75f);
                                break; 
                        }
                        GUILayout.Label(baseItem.Name, new GUIStyle(GUI.skin.label) {fontStyle = FontStyle.Bold});
                        GUI.color = initColor;
                    }
                    GUILayout.EndHorizontal();
                    
                    // Type-specific details
                    GUILayout.BeginVertical(GUI.skin.box);
                    {
                        switch (context)
                        {
                            case IArmor armor:
                            {
                                GUILayout.Label($"Type: {armor.ArmorType}");
                                foreach (var damageKvp in armor.DamageModifiers)
                                {
                                    float reduction = 1f - damageKvp.Value;
                                    if (reduction > 0)
                                    {
                                        GUILayout.Label($"{damageKvp.Key} -{reduction:P0}");
                                    }
                                    else
                                    {
                                        GUI.color = Color.red;
                                        GUILayout.Label($"{damageKvp.Key} +{reduction:P0}");
                                        GUI.color = Color.white;
                                    }
                                }

                                break;
                            }
                            case IWeapon weapon:
                            {
                                GUILayout.Label($"Type: {weapon.WeaponType}");
                                GUILayout.Label($"Action: {weapon.AttackAbility.Name}");
                                DrawAbility(weapon.AttackAbility);
                                break;
                            }

                            default:
                            {
                                GUILayout.Label($"Type: {baseItem.ItemType}");
                                break;
                            }
                        }
                    }
                    GUILayout.EndVertical();
                    
                    // Base Value
                    GUILayout.Label($"Value: {baseItem.BaseValue}G");
                }
                GUILayout.EndVertical();
            }
        }
    }
}
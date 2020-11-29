using System;
using System.Collections.Generic;
using Core.Abilities;
using Core.Classes.Player;
using Core.Database;
using Core.EquipMap;
using Core.Etc;
using Core.Naming;
using Core.StatMap;
using SadPumpkin.Util.Context;
using UnityEditor;
using UnityEngine;

namespace Unity.Editor.DataDrawers
{
    public class PlayerClassDataDrawer : IDataDrawer<PlayerClass>
    {
        private readonly IDataDrawer<INameGenerator> _nameDrawer = null;
        private readonly IDataDrawer<IStatMapBuilder> _statBuilderDrawer = null;
        private readonly IDataDrawer<IStatMapIncrementor> _statIncDrawer = null;
        private readonly IDataDrawer<IEquipMapBuilder> _equipBuilderDrawer = null;
        private readonly IDataDrawer<IReadOnlyDictionary<DamageType, float>> _protectionDrawer = null;
        private readonly IDataDrawer<IReadOnlyDictionary<uint, IReadOnlyCollection<IAbility>>> _abilitiesDrawer = null;
        
        public PlayerClassDataDrawer()
        {
            _nameDrawer = DataDrawerFinder.Find<INameGenerator>();
            _statBuilderDrawer = DataDrawerFinder.Find<IStatMapBuilder>();
            _statIncDrawer = DataDrawerFinder.Find<IStatMapIncrementor>();
            _equipBuilderDrawer = DataDrawerFinder.Find<IEquipMapBuilder>();
            _protectionDrawer = DataDrawerFinder.Find<IReadOnlyDictionary<DamageType, float>>();
            _abilitiesDrawer = DataDrawerFinder.Find<IReadOnlyDictionary<uint, IReadOnlyCollection<IAbility>>>();
        }
        
        public void DrawGUI(PlayerClass value, IContext context, Action<PlayerClass> lateRef)
        {
            value.Id = (uint) EditorGUILayout.IntField("Id:", (int) value.Id);
            value.Name = EditorGUILayout.TextField("Name:", value.Name);
            value.Desc = EditorGUILayout.TextField("Description:", value.Desc);
            value.Rarity = (RarityCategory) EditorGUILayout.EnumPopup("Rarity:", value.Rarity);
            value.WeaponProficiency = (WeaponType) EditorGUILayout.EnumFlagsField("Weapons:", value.WeaponProficiency);
            value.ArmorProficiency = (ArmorType) EditorGUILayout.EnumFlagsField("Armor:", value.ArmorProficiency);
            
            GUILayout.BeginHorizontal();
            GUILayout.Label("Naming:", GUILayout.ExpandWidth(false));
            if (_nameDrawer != null)
            {
                _nameDrawer.DrawGUI(value.NameGenerator, context, newCalc => value.NameGenerator = newCalc);
            }
            else
            {
                EditorGUILayout.HelpBox(
                    "No corresponding DataDrawer could be found for INameGenerator!",
                    MessageType.Error);
            }
            GUILayout.EndHorizontal();
            
            GUILayout.BeginHorizontal();
            GUILayout.Label("Starting Stats:", GUILayout.ExpandWidth(false));
            if (_statBuilderDrawer != null)
            {
                _statBuilderDrawer.DrawGUI(value.StartingStats, context, newCalc => value.StartingStats = newCalc);
            }
            else
            {
                EditorGUILayout.HelpBox(
                    "No corresponding DataDrawer could be found for IStatMapBuilder!",
                    MessageType.Error);
            }
            GUILayout.EndHorizontal();
            
            GUILayout.BeginHorizontal();
            GUILayout.Label("Level Up Stats:", GUILayout.ExpandWidth(false));
            if (_statIncDrawer != null)
            {
                _statIncDrawer.DrawGUI(value.LevelUpStats, context, newCalc => value.LevelUpStats = newCalc);
            }
            else
            {
                EditorGUILayout.HelpBox(
                    "No corresponding DataDrawer could be found for IStatMapIncrementer!",
                    MessageType.Error);
            }
            GUILayout.EndHorizontal();
            
            GUILayout.BeginHorizontal();
            GUILayout.Label("Starting Equipment:", GUILayout.ExpandWidth(false));
            if (_equipBuilderDrawer != null)
            {
                _equipBuilderDrawer.DrawGUI(value.StartingEquipment, context, newCalc => value.StartingEquipment = newCalc);
            }
            else
            {
                EditorGUILayout.HelpBox(
                    "No corresponding DataDrawer could be found for IEquipMapBuilder!",
                    MessageType.Error);
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Protection:", GUILayout.ExpandWidth(false));
            if (_protectionDrawer != null)
            {
                _protectionDrawer.DrawGUI(value.IntrinsicDamageModification, context, newCalc => value.IntrinsicDamageModification = newCalc);
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
            if (_abilitiesDrawer != null)
            {
                _abilitiesDrawer.DrawGUI(value.AbilitiesPerLevel, context, newCalc => value.AbilitiesPerLevel = newCalc);
            }
            else
            {
                EditorGUILayout.HelpBox(
                    $"No corresponding DataDrawer could be found for {nameof(IReadOnlyDictionary<uint, IReadOnlyCollection<IAbility>>)}!",
                    MessageType.Error);
            }
            GUILayout.EndHorizontal();
        }
    }
}
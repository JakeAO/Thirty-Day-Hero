using System.Collections.Generic;
using Core.Abilities;
using Core.EquipMap;
using Core.Etc;
using Core.Naming;
using Core.StatMap;

namespace Core.Classes.Player
{
    public class PlayerClass : IPlayerClass
    {
        public uint Id { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public RarityCategory Rarity { get; set; }
        public INameGenerator NameGenerator { get; set; }
        public IStatMapBuilder StartingStats { get; set; }
        public IStatMapIncrementor LevelUpStats { get; set; }
        public IReadOnlyDictionary<uint, IReadOnlyCollection<IAbility>> AbilitiesPerLevel { get; set; }
        public IReadOnlyDictionary<DamageType, float> IntrinsicDamageModification { get; set; }
        public string ArtPath { get; set; }
        public WeaponType WeaponProficiency { get; set; }
        public ArmorType ArmorProficiency { get; set; }
        public IEquipMapBuilder StartingEquipment { get; set; }

        public PlayerClass()
            : this(
                0u,
                string.Empty,
                string.Empty,
                string.Empty,
                NullNameGenerator.Instance,
                NullStatMapBuilder.Instance,
                NullStatMapIncrementor.Instance,
                new Dictionary<uint, IReadOnlyCollection<IAbility>>(),
                new Dictionary<DamageType, float>(),
                WeaponType.Invalid,
                ArmorType.Invalid,
                NullEquipMapBuilder.Instance)
        {
        }

        public PlayerClass(
            uint id,
            string name,
            string desc,
            string artPath,
            INameGenerator nameGenerator,
            IStatMapBuilder startingStats,
            IStatMapIncrementor levelUpStats,
            IReadOnlyDictionary<uint, IReadOnlyCollection<IAbility>> abilitiesPerLevel,
            IReadOnlyDictionary<DamageType, float> intrinsicDamageModification,
            WeaponType weaponProficiency,
            ArmorType armorProficiency,
            IEquipMapBuilder startingEquipment)
        {
            Id = id;
            Name = name;
            Desc = desc;
            ArtPath = artPath;
            NameGenerator = nameGenerator;
            StartingStats = startingStats;
            LevelUpStats = levelUpStats;
            AbilitiesPerLevel = abilitiesPerLevel;
            IntrinsicDamageModification = intrinsicDamageModification;
            WeaponProficiency = weaponProficiency;
            ArmorProficiency = armorProficiency;
            StartingEquipment = startingEquipment;
        }

        public IReadOnlyCollection<IAbility> GetAllAbilities(uint level)
        {
            List<IAbility> abilities = new List<IAbility>(10);

            for (uint i = 0; i <= level; i++)
            {
                if (AbilitiesPerLevel.TryGetValue(i, out var abilitiesForLevel))
                {
                    abilities.AddRange(abilitiesForLevel);
                }
            }

            return abilities;
        }
    }
}
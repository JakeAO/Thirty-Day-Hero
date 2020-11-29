using System.Collections.Generic;
using Core.Abilities;
using Core.Etc;
using Core.Naming;
using Core.StatMap;

namespace Core.Classes.Calamity
{
    public class CalamityClass : ICalamityClass
    {
        public uint Id { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public string ArtPath { get; set; }
        public RarityCategory Rarity { get; set; }
        public INameGenerator NameGenerator { get; set; }
        public IReadOnlyDictionary<DamageType, float> IntrinsicDamageModification { get; set; }
        public IStatMapBuilder StartingStats { get; set; }
        public IStatMapIncrementor LevelUpStats { get; set; }
        public IReadOnlyDictionary<uint, IReadOnlyCollection<IAbility>> AbilitiesPerLevel { get; set; }

        public CalamityClass()
            : this(
                0u,
                string.Empty,
                string.Empty,
                string.Empty,
                NullNameGenerator.Instance,
                new Dictionary<DamageType, float>(),
                NullStatMapBuilder.Instance,
                NullStatMapIncrementor.Instance,
                new Dictionary<uint, IReadOnlyCollection<IAbility>>())
        {
        }

        public CalamityClass(
            uint id,
            string name,
            string desc,
            string artPath,
            INameGenerator nameGenerator,
            IDictionary<DamageType, float> intrinsicDamageModification,
            IStatMapBuilder startingStats,
            IStatMapIncrementor levelUpStats,
            IDictionary<uint, IReadOnlyCollection<IAbility>> abilitiesPerLevel)
        {
            Id = id;
            Name = name;
            Desc = desc;
            ArtPath = artPath;
            NameGenerator = nameGenerator;
            StartingStats = startingStats;
            LevelUpStats = levelUpStats;
            AbilitiesPerLevel = new Dictionary<uint, IReadOnlyCollection<IAbility>>(abilitiesPerLevel);
            IntrinsicDamageModification = new Dictionary<DamageType, float>(intrinsicDamageModification);
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
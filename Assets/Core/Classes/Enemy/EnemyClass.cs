using System.Collections.Generic;
using Core.Abilities;
using Core.Etc;
using Core.Naming;
using Core.StatMap;

namespace Core.Classes.Enemy
{
    public class EnemyClass : IEnemyClass
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

        public EnemyClass()
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

        public EnemyClass(
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

    public class Dorp : IEnemyClass
    {
        public string Name => "Dorp";

        public string Desc => "You're not sure it can be considered intelligent";
        public string ArtPath => "";

        public RarityCategory Rarity => RarityCategory.Common;

        private DorpNames _nameGen = new DorpNames();
        public INameGenerator NameGenerator => _nameGen;

        public IStatMapBuilder StartingStats => NullStatMapBuilder.Instance;

        public IStatMapIncrementor LevelUpStats => NullStatMapIncrementor.Instance;

        public IReadOnlyDictionary<uint, IReadOnlyCollection<IAbility>> AbilitiesPerLevel => new Dictionary<uint, IReadOnlyCollection<IAbility>>();

        public IReadOnlyDictionary<DamageType, float> IntrinsicDamageModification => new Dictionary<DamageType, float>();

        public uint Id => 0;

        public IReadOnlyCollection<IAbility> GetAllAbilities(uint level)
        {
            return new IAbility[0];
        }

        private class DorpNames : INameGenerator
        {
            private System.Random _rng = new System.Random();

            public string GetName()
            {
                switch(_rng.Next(0, 4))
                {
                    case 0:
                    default:
                        return "Dorp";
                    case 1:
                        return "Durp";
                    case 2:
                        return "Derp";
                    case 3:
                        return "Dirp";
                    case 4:
                        return "Darp";
                }
            }
        }
    }
}
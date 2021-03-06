using System.Collections.Generic;
using Core.Abilities;
using Core.Etc;
using Core.Naming;
using Core.StatMap;

namespace Core.Classes.Calamity
{
    public class NullCalamityClass : ICalamityClass
    {
        public static readonly NullCalamityClass Instance = new NullCalamityClass();
        
        private NullCalamityClass()
        {
        }
        
        public uint Id => 0u;
        public string Name => string.Empty;
        public string Desc => string.Empty;
        public string ArtPath => string.Empty;
        public RarityCategory Rarity => RarityCategory.Invalid;
        public INameGenerator NameGenerator { get; } = NullNameGenerator.Instance;
        public IReadOnlyDictionary<DamageType, float> IntrinsicDamageModification { get; } = new Dictionary<DamageType, float>();
        public IStatMapBuilder StartingStats => NullStatMapBuilder.Instance;
        public IStatMapIncrementor LevelUpStats => NullStatMapIncrementor.Instance;
        public IReadOnlyDictionary<uint, IReadOnlyCollection<IAbility>> AbilitiesPerLevel { get; } = new Dictionary<uint, IReadOnlyCollection<IAbility>>();

        public IReadOnlyCollection<IAbility> GetAllAbilities(uint level)
        {
            return new IAbility[] { };
        }
    }
}
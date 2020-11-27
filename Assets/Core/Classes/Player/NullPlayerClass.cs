using System.Collections.Generic;
using Core.Abilities;
using Core.EquipMap;
using Core.Etc;
using Core.Naming;
using Core.StatMap;

namespace Core.Classes.Player
{
    public class NullPlayerClass : IPlayerClass
    {
        public static readonly NullPlayerClass Instance = new NullPlayerClass();

        private NullPlayerClass()
        {
        }
        
        public uint Id => 0u;
        public string Name => string.Empty;
        public string Desc => string.Empty;
        public RarityCategory Rarity => RarityCategory.Invalid;
        public INameGenerator NameGenerator { get; } = new TxtNameGenerator(new[] {"Null"});
        public IReadOnlyDictionary<DamageType, float> IntrinsicDamageModification { get; } = new Dictionary<DamageType, float>();
        public IStatMapBuilder StartingStats => NullStatMapBuilder.Instance;
        public IStatMapIncrementor LevelUpStats => NullStatMapIncrementor.Instance;
        public WeaponType WeaponProficiency => WeaponType.Invalid;
        public ArmorType ArmorProficiency => ArmorType.Invalid;
        public IEquipMapBuilder StartingEquipment => NullEquipMapBuilder.Instance;
        public IReadOnlyDictionary<uint, IReadOnlyCollection<IAbility>> AbilitiesPerLevel { get; } = new Dictionary<uint, IReadOnlyCollection<IAbility>>();

        public IReadOnlyCollection<IAbility> GetAllAbilities(uint level)
        {
            return new IAbility[] { };
        }
    }
}
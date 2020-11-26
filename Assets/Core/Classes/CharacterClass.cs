using System.Collections.Generic;
using Core.Etc;
using Core.StatMap;
using SadPumpkin.Util.CombatEngine.Abilities;

namespace Core.Classes
{
    public class CharacterClass : ICharacterClass
    {
        public uint Id { get; }
        public string Name { get; }
        public string Desc { get; }
        public IReadOnlyDictionary<DamageType, float> IntrinsicDamageModification { get; }
        public IStatMapBuilder StartingStats { get; }
        public IStatMapIncrementor LevelUpStats { get; }
        public IReadOnlyDictionary<uint, IReadOnlyCollection<IAbility>> AbilitiesPerLevel { get; }

        public CharacterClass(
            uint id,
            string name,
            string desc,
            IDictionary<DamageType, float> intrinsicDamageModification,
            IStatMapBuilder startingStats,
            IStatMapIncrementor levelUpStats,
            IDictionary<uint, IReadOnlyCollection<IAbility>> abilitiesPerLevel)
        {
            Id = id;
            Name = name;
            Desc = desc;
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
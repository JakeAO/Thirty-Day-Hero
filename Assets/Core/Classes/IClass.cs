using System.Collections.Generic;
using Core.Abilities;
using Core.Etc;
using Core.Naming;
using Core.StatMap;
using SadPumpkin.Util.CombatEngine;

namespace Core.Classes
{
    public interface IClass : IIdTracked, INamed
    {
        RarityCategory Rarity { get; }
        INameGenerator NameGenerator { get; }
        IStatMapBuilder StartingStats { get; }
        IStatMapIncrementor LevelUpStats { get; }
        IReadOnlyDictionary<uint, IReadOnlyCollection<IAbility>> AbilitiesPerLevel { get; }
        IReadOnlyCollection<IAbility> GetAllAbilities(uint level);
        IReadOnlyDictionary<DamageType, float> IntrinsicDamageModification { get; }
    }
}
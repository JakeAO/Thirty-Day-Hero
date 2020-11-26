using System.Collections.Generic;
using Core.Etc;
using Core.Items.Weapons;
using Core.StatMap;
using SadPumpkin.Util.CombatEngine;
using SadPumpkin.Util.CombatEngine.Abilities;

namespace Core.Classes
{
    public interface ICharacterClass : IIdTracked
    {
        string Name { get; }
        string Desc { get; }
        IReadOnlyDictionary<DamageType, float> IntrinsicDamageModification { get; }
        IStatMapBuilder StartingStats { get; }
        IStatMapIncrementor LevelUpStats { get; }
        IReadOnlyDictionary<uint, IReadOnlyCollection<IAbility>> AbilitiesPerLevel { get; }

        IReadOnlyCollection<IAbility> GetAllAbilities(uint level);
    }
}
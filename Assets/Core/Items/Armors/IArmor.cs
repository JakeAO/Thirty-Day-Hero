using System.Collections.Generic;
using Core.Etc;
using SadPumpkin.Util.CombatEngine;

namespace Core.Items.Armors
{
    public interface IArmor : IItem, ICopyable<IArmor>
    {
        ArmorType ArmorType { get; }
        IReadOnlyDictionary<DamageType, float> DamageModifiers { get; }
        float GetReducedDamage(float damageAmount, DamageType damageType);
    }
}
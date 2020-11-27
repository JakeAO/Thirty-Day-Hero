using System.Collections.Generic;
using Core.Etc;

namespace Core.Items.Armors
{
    public interface IArmor : IItem
    {
        ArmorType ArmorType { get; }
        IReadOnlyDictionary<DamageType, float> DamageModifiers { get; }
        float GetReducedDamage(float damageAmount, DamageType damageType);
    }
}
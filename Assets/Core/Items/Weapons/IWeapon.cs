using Core.Abilities;
using Core.Etc;

namespace Core.Items.Weapons
{
    public interface IWeapon : IItem
    {
        WeaponType WeaponType { get; }
        IAbility AttackAbility { get; }
    }
}
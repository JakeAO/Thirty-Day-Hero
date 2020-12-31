using Core.Abilities;
using Core.Etc;
using SadPumpkin.Util.CombatEngine;

namespace Core.Items.Weapons
{
    public interface IWeapon : IItem, ICopyable<IWeapon>
    {
        WeaponType WeaponType { get; }
        IAbility AttackAbility { get; }
    }
}
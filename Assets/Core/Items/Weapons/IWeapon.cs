using Core.Etc;
using SadPumpkin.Util.CombatEngine.Abilities;

namespace Core.Items.Weapons
{
    public interface IWeapon : IItem
    {
        WeaponType WeaponType { get; }
        IAbility AttackAbility { get; }
    }
}
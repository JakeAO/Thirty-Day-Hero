using Core.Etc;
using Core.Items.Weapons;
using SadPumpkin.Util.CombatEngine.Actor;
using SadPumpkin.Util.CombatEngine.RequirementCalculators;
using IPlayerCharacterActor = Core.Actors.Player.IPlayerCharacterActor;

namespace Core.Requirements
{
    public class EquippedWeaponRequirement : IRequirementCalc
    {
        private WeaponType _weaponType = WeaponType.Invalid;

        public EquippedWeaponRequirement(WeaponType weaponType)
        {
            _weaponType = weaponType;
        }

        public bool MeetsRequirement(IInitiativeActor actor)
        {
            if (actor is IPlayerCharacterActor playerCharacter)
            {
                WeaponType equippedType = playerCharacter.Equipment.Weapon?.WeaponType ?? WeaponType.Invalid;
                return (equippedType & _weaponType) == equippedType;
            }

            return false;
        }
    }
}
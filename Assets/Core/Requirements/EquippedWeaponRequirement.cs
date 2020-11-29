using Core.Etc;
using SadPumpkin.Util.CombatEngine.Actor;
using SadPumpkin.Util.CombatEngine.RequirementCalculators;

using IPlayerCharacterActor = Core.Actors.Player.IPlayerCharacterActor;

namespace Core.Requirements
{
    public class EquippedWeaponRequirement : IRequirementCalc
    {
        public WeaponType RequiredType { get; set; }

        public string Description => $"{RequiredType} Equipped";

        public EquippedWeaponRequirement(WeaponType requiredType)
        {
            RequiredType = requiredType;
        }

        public bool MeetsRequirement(IInitiativeActor actor)
        {
            if (actor is IPlayerCharacterActor playerCharacter)
            {
                WeaponType equippedType = playerCharacter.Equipment.Weapon?.WeaponType ?? WeaponType.Invalid;
                return RequiredType.HasFlag(equippedType);
            }

            return false;
        }
    }
}
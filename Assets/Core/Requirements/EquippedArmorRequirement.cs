using Core.Etc;
using Core.Items.Armors;
using SadPumpkin.Util.CombatEngine.Actor;
using SadPumpkin.Util.CombatEngine.RequirementCalculators;
using IPlayerCharacterActor = Core.Actors.Player.IPlayerCharacterActor;

namespace Core.Requirements
{
    public class EquippedArmorRequirement : IRequirementCalc
    {
        private readonly ArmorType _armorType = ArmorType.Invalid;

        public EquippedArmorRequirement(ArmorType armorType)
        {
            _armorType = armorType;
        }

        public bool MeetsRequirement(IInitiativeActor actor)
        {
            if (actor is IPlayerCharacterActor playerCharacter)
            {
                ArmorType equippedType = playerCharacter.Equipment.Armor?.ArmorType ?? ArmorType.Invalid;
                return (equippedType & _armorType) == equippedType;
            }

            return false;
        }
    }
}
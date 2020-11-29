using Core.Etc;
using SadPumpkin.Util.CombatEngine.Actor;
using SadPumpkin.Util.CombatEngine.RequirementCalculators;

using IPlayerCharacterActor = Core.Actors.Player.IPlayerCharacterActor;

namespace Core.Requirements
{
    public class EquippedArmorRequirement : IRequirementCalc
    {
        public ArmorType RequiredType { get; set; }

        public string Description => $"{RequiredType} Armor Equipped";

        public EquippedArmorRequirement()
            : this(ArmorType.Invalid)
        {
            
        }
        
        public EquippedArmorRequirement(ArmorType requiredType)
        {
            RequiredType = requiredType;
        }

        public bool MeetsRequirement(IInitiativeActor actor)
        {
            if (actor is IPlayerCharacterActor playerCharacter)
            {
                ArmorType equippedType = playerCharacter.Equipment.Armor?.ArmorType ?? ArmorType.Invalid;
                return RequiredType.HasFlag(equippedType);
            }

            return false;
        }
    }
}
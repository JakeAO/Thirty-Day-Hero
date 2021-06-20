using Core.EquipMap;
using Core.Etc;

namespace Unity.Scenes.Shared.Entities
{
    public readonly struct ActorUpdateContext
    {
        public static readonly ActorUpdateContext DEFAULT = new ActorUpdateContext();
        
        public readonly StatType ChangedStat;
        public readonly EquipmentSlot ChangedEquipment;

        public ActorUpdateContext(
            StatType changedStat = StatType.Invalid,
            EquipmentSlot changedEquipment = EquipmentSlot.Invalid)
        {
            ChangedStat = changedStat;
            ChangedEquipment = changedEquipment;
        }
    }
}
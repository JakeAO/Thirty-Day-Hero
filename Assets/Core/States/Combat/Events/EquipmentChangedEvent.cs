using Core.EquipMap;
using SadPumpkin.Util.CombatEngine.Events;

namespace Core.States.Combat.Events
{
    public class EquipmentChangedEvent : ICombatEventData
    {
        public readonly uint ActorId;
        public readonly EquipmentSlot Slot;
        public readonly uint OldItemId;
        public readonly uint NewItemId;

        public EquipmentChangedEvent(uint actorId, EquipmentSlot slot, uint oldItemId, uint newItemId)
        {
            ActorId = actorId;
            Slot = slot;
            OldItemId = oldItemId;
            NewItemId = newItemId;
        }
    }
}
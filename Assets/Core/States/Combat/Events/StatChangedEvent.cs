using Core.Etc;
using SadPumpkin.Util.CombatEngine.Events;

namespace Core.States.Combat.Events
{
    public class StatChangedEvent : ICombatEventData
    {
        public readonly uint ActorId;
        public readonly StatType Stat;
        public readonly uint OldValue;
        public readonly uint NewValue;

        public StatChangedEvent(uint actorId, StatType stat, uint oldValue, uint newValue)
        {
            ActorId = actorId;
            Stat = stat;
            OldValue = oldValue;
            NewValue = newValue;
        }
    }
}
using SadPumpkin.Util.CombatEngine.Events;

namespace Core.States.Combat.Events
{
    public class PartyChangedEvent : ICombatEventData
    {
        public readonly uint ActorId;
        public readonly uint OldPartyId;
        public readonly uint NewPartyId;

        public PartyChangedEvent(uint actorId, uint oldPartyId, uint newPartyId)
        {
            ActorId = actorId;
            OldPartyId = oldPartyId;
            NewPartyId = newPartyId;
        }
    }
}
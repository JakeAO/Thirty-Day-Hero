using SadPumpkin.Util.CombatEngine.Events;

namespace Core.States.Combat.Events
{
    public class ActorDiedEvent : ICombatEventData
    {
        public readonly uint ActorId;

        public ActorDiedEvent(uint actorId)
        {
            ActorId = actorId;
        }
    }
}
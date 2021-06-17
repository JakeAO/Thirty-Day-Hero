using SadPumpkin.Util.CombatEngine.Events;

namespace Core.States.Combat.Events
{
    public class ActorResurrectedEvent : ICombatEventData
    {
        public readonly uint ActorId;

        public ActorResurrectedEvent(uint actorId)
        {
            ActorId = actorId;
        }
    }
}
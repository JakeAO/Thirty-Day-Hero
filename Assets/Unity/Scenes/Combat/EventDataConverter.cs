using System;
using System.Collections.Generic;
using Core.States.Combat.Events;
using SadPumpkin.Util.CombatEngine.Events;
using SadPumpkin.Util.Context;
using SadPumpkin.Util.Events;
using SadPumpkin.Util.UXEventQueue;
using Unity.Scenes.Combat.UxEventConverters;

namespace Unity.Scenes.Combat
{
    public class EventDataConverter
    {
        private readonly Dictionary<Type, IUxEventConverter> _eventConverters = new Dictionary<Type, IUxEventConverter>(10);

        public EventDataConverter(IContext activeContext)
        {
            _eventConverters[typeof(ActiveActorChangedEvent)] = new ActiveActorChangedConverter(activeContext);
            _eventConverters[typeof(ActorActionTakenEvent)] = new ActorActionTakenConverter(activeContext);
            _eventConverters[typeof(ActorDiedEvent)] = new ActorDiedConverter(activeContext);
            _eventConverters[typeof(ActorResurrectedEvent)] = new ActorResurrectedConverter(activeContext);
            _eventConverters[typeof(CombatCompletedEvent)] = new CombatCompletedConverter(activeContext);
            _eventConverters[typeof(CombatResultsEvent)] = new CombatResultsConverter(activeContext);
            _eventConverters[typeof(CombatStartedEvent)] = new CombatStartedConverter(activeContext);
            _eventConverters[typeof(CombatStateChangedEvent)] = new CombatStateChangedConverter(activeContext);
            _eventConverters[typeof(EquipmentChangedEvent)] = new EquipmentChangedConverter(activeContext);
            _eventConverters[typeof(PartyChangedEvent)] = new PartyChangedConverter(activeContext);
            _eventConverters[typeof(StatChangedEvent)] = new StatChangedConverter(activeContext);
        }

        public IEnumerable<IUXEvent> ConvertEvent(IEventData eventData)
        {
            Type eventType = eventData.GetType();
            if (_eventConverters.TryGetValue(eventType, out IUxEventConverter converter))
            {
                foreach (IUXEvent uxEvent in converter.Convert(eventData))
                {
                    yield return uxEvent;
                }
            }
        }
    }
}
using System.Collections.Generic;
using Core.States.Combat.Events;
using SadPumpkin.Util.Context;
using SadPumpkin.Util.Events;
using SadPumpkin.Util.UXEventQueue;
using Unity.Scenes.Combat.UxEvents;

namespace Unity.Scenes.Combat.UxEventConverters
{
    public class PartyChangedConverter : IUxEventConverter
    {
        private readonly IContext _activeContext = null;

        public PartyChangedConverter(IContext activeContext)
        {
            _activeContext = activeContext;
        }

        public IEnumerable<IUXEvent> Convert(IEventData eventData)
        {
            if (eventData is PartyChangedEvent typedEventData)
            {
                yield return new PartyChangedUxEvent(typedEventData, _activeContext);
            }
        }
    }
}
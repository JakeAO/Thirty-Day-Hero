using System.Collections.Generic;
using Core.States.Combat.Events;
using SadPumpkin.Util.Context;
using SadPumpkin.Util.Events;
using SadPumpkin.Util.UXEventQueue;
using Unity.Scenes.Combat.UxEvents;

namespace Unity.Scenes.Combat.UxEventConverters
{
    public class StatChangedConverter : IUxEventConverter
    {
        private readonly IContext _activeContext = null;

        public StatChangedConverter(IContext activeContext)
        {
            _activeContext = activeContext;
        }

        public IEnumerable<IUXEvent> Convert(IEventData eventData)
        {
            if (eventData is StatChangedEvent typedEventData)
            {
                yield return new StatChangedUxEvent(typedEventData, _activeContext);
            }
        }
    }
}
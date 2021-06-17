using System.Collections.Generic;
using SadPumpkin.Util.CombatEngine.Events;
using SadPumpkin.Util.Context;
using SadPumpkin.Util.Events;
using SadPumpkin.Util.UXEventQueue;
using Unity.Scenes.Combat.UxEvents;

namespace Unity.Scenes.Combat.UxEventConverters
{
    public class ActiveActorChangedConverter : IUxEventConverter
    {
        private readonly IContext _activeContext = null;

        public ActiveActorChangedConverter(IContext activeContext)
        {
            _activeContext = activeContext;
        }

        public IEnumerable<IUXEvent> Convert(IEventData eventData)
        {
            if (eventData is ActiveActorChangedEvent typedEventData)
            {
                yield return new ActiveActorChangedUxEvent(typedEventData, _activeContext);
            }
        }
    }
}
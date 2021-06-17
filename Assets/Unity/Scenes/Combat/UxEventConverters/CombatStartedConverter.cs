using System.Collections.Generic;
using SadPumpkin.Util.CombatEngine.Events;
using SadPumpkin.Util.Context;
using SadPumpkin.Util.Events;
using SadPumpkin.Util.UXEventQueue;
using Unity.Scenes.Combat.UxEvents;

namespace Unity.Scenes.Combat.UxEventConverters
{
    public class CombatStartedConverter : IUxEventConverter
    {
        private readonly IContext _activeContext = null;

        public CombatStartedConverter(IContext activeContext)
        {
            _activeContext = activeContext;
        }

        public IEnumerable<IUXEvent> Convert(IEventData eventData)
        {
            if (eventData is CombatStartedEvent typedEventData)
            {
                yield return new CombatStartedUxEvent(typedEventData, _activeContext);
            }
        }
    }
}
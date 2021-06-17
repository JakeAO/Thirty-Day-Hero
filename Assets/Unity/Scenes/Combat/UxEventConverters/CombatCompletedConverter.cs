using System.Collections.Generic;
using SadPumpkin.Util.CombatEngine.Events;
using SadPumpkin.Util.Context;
using SadPumpkin.Util.Events;
using SadPumpkin.Util.UXEventQueue;
using Unity.Scenes.Combat.UxEvents;

namespace Unity.Scenes.Combat.UxEventConverters
{
    public class CombatCompletedConverter : IUxEventConverter
    {
        private readonly IContext _activeContext = null;

        public CombatCompletedConverter(IContext activeContext)
        {
            _activeContext = activeContext;
        }

        public IEnumerable<IUXEvent> Convert(IEventData eventData)
        {
            if (eventData is CombatCompletedEvent typedEventData)
            {
                yield return new CombatCompletedUxEvent(typedEventData, _activeContext);
            }
        }
    }
}
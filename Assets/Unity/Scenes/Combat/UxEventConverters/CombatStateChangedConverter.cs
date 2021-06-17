using System.Collections.Generic;
using SadPumpkin.Util.CombatEngine.Events;
using SadPumpkin.Util.Context;
using SadPumpkin.Util.Events;
using SadPumpkin.Util.UXEventQueue;
using Unity.Scenes.Combat.UxEvents;

namespace Unity.Scenes.Combat.UxEventConverters
{
    public class CombatStateChangedConverter : IUxEventConverter
    {
        private readonly IContext _activeContext = null;

        public CombatStateChangedConverter(IContext activeContext)
        {
            _activeContext = activeContext;
        }

        public IEnumerable<IUXEvent> Convert(IEventData eventData)
        {
            if (eventData is CombatStateChangedEvent typedEventData)
            {
                yield return new CombatStateChangedUxEvent(typedEventData, _activeContext);
            }
        }
    }
}
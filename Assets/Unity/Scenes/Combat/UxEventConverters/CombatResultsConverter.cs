using System.Collections.Generic;
using Core.States.Combat.Events;
using SadPumpkin.Util.Context;
using SadPumpkin.Util.Events;
using SadPumpkin.Util.UXEventQueue;
using Unity.Scenes.Combat.UxEvents;

namespace Unity.Scenes.Combat.UxEventConverters
{
    public class CombatResultsConverter : IUxEventConverter
    {
        private readonly IContext _activeContext = null;

        public CombatResultsConverter(IContext activeContext)
        {
            _activeContext = activeContext;
        }

        public IEnumerable<IUXEvent> Convert(IEventData eventData)
        {
            if (eventData is CombatResultsEvent typedEventData)
            {
                yield return new CombatResultsUxEvent(typedEventData, _activeContext);
            }
        }
    }
}
using System.Collections.Generic;
using Core.States.Combat.Events;
using SadPumpkin.Util.Context;
using SadPumpkin.Util.Events;
using SadPumpkin.Util.UXEventQueue;
using Unity.Scenes.Combat.UxEvents;

namespace Unity.Scenes.Combat.UxEventConverters
{
    public class EquipmentChangedConverter : IUxEventConverter
    {
        private readonly IContext _activeContext = null;

        public EquipmentChangedConverter(IContext activeContext)
        {
            _activeContext = activeContext;
        }

        public IEnumerable<IUXEvent> Convert(IEventData eventData)
        {
            if (eventData is EquipmentChangedEvent typedEventData)
            {
                yield return new EquipmentChangedUxEvent(typedEventData, _activeContext);
            }
        }
    }
}
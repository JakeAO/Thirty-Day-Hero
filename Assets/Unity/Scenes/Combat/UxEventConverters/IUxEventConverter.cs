using System.Collections.Generic;
using SadPumpkin.Util.Events;
using SadPumpkin.Util.UXEventQueue;

namespace Unity.Scenes.Combat.UxEventConverters
{
    public interface IUxEventConverter
    {
        IEnumerable<IUXEvent> Convert(IEventData eventData);
    }
}
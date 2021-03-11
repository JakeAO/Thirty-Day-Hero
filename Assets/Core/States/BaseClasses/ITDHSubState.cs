using System.Collections.Generic;
using Core.EventOptions;
using SadPumpkin.Util.Context;

namespace Core.States.BaseClasses
{
    public interface ITDHSubState
    {
        IContext SharedContext { get; }

        IEnumerable<IEventOption> GetOptions();
    }
}
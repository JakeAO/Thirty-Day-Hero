using System.Collections.Generic;
using System.Linq;
using Core.EventOptions;
using SadPumpkin.Util.Context;

namespace Core.States.BaseClasses
{
    public abstract class TDHSubStateBase : ITDHSubState
    {
        public IContext SharedContext { get; protected set; }
        
        public IReadOnlyDictionary<string, List<IEventOption>> CurrentOptions => _currentOptions;

        protected readonly Dictionary<string, List<IEventOption>> _currentOptions = new Dictionary<string, List<IEventOption>>(10);

        public IEnumerable<IEventOption> GetOptions() => CurrentOptions.Values.SelectMany(x => x);
    }
}
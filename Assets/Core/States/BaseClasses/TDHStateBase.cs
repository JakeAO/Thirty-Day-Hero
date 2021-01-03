using System.Collections.Generic;
using System.Linq;
using Core.EventOptions;
using Core.Signals;
using SadPumpkin.Util.Context;
using SadPumpkin.Util.StateMachine.States;

namespace Core.States.BaseClasses
{
    public abstract class TDHStateBase : ITDHState
    {
        public IContext SharedContext { get; private set; }
        public StateOptionsChangedSignal OptionsChangedSignal { get; private set; }

        public IReadOnlyDictionary<string, List<IEventOption>> CurrentOptions => _currentOptions;

        protected readonly Dictionary<string, List<IEventOption>> _currentOptions = new Dictionary<string, List<IEventOption>>(10);

        public IEnumerable<IEventOption> GetOptions() => CurrentOptions.Values.SelectMany(x => x);

        public void PerformSetup(IContext context, IState previousState)
        {
            SharedContext = context;
            OptionsChangedSignal = context.TryGet(out StateOptionsChangedSignal existingSignal)
                ? existingSignal
                : new StateOptionsChangedSignal();

            OnEnter(previousState);
        }

        public void PerformContent(IContext context)
        {
            SharedContext = context;

            OnContent();
        }

        public void PerformTeardown(IContext context, IState nextState)
        {
            SharedContext = context;

            OnExit(nextState);
        }

        public virtual void OnEnter(IState fromState)
        {
            // Intentionally left blank.
        }

        public virtual void OnContent()
        {
            // Intentionally left blank.
        }

        public virtual void OnExit(IState toState)
        {
            // Intentionally left blank.
        }
    }
}
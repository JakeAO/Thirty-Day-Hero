using System.Collections.Generic;
using Core.Etc;
using Core.EventOptions;
using Core.Signals;
using SadPumpkin.Util.Context;
using SadPumpkin.Util.StateMachine.States;

namespace Core.States
{
    public abstract class TDHStateBase : ITDHState
    {
        public IContext SharedContext { get; private set; }
        public StateOptionsChangedSignal OptionsChangedSignal { get; private set; }

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

        public abstract IEnumerable<IEventOption> GetOptions();
    }
}
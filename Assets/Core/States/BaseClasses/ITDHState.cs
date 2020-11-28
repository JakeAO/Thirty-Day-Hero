using System.Collections.Generic;
using Core.Etc;
using Core.EventOptions;
using Core.Signals;
using SadPumpkin.Util.Context;
using SadPumpkin.Util.StateMachine.States;

namespace Core.States
{
    public interface ITDHState : IState
    {
        IContext SharedContext { get; }
        StateOptionsChangedSignal OptionsChangedSignal { get; }

        IEnumerable<IEventOption> GetOptions();

        void OnEnter(IState fromState);
        void OnContent();
        void OnExit(IState toState);
    }
}
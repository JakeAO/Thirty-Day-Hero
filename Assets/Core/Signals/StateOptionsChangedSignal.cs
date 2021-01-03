using System;
using Core.States;
using Core.States.BaseClasses;
using SadPumpkin.Util.Signals;

namespace Core.Signals
{
    public class StateOptionsChangedSignal : ISignal<ITDHState>
    {
        private event Action<ITDHState> EventHandler;

        public void Fire(ITDHState value)
        {
            EventHandler?.Invoke(value);
        }

        public void Listen(Action<ITDHState> callback)
        {
            EventHandler += callback;
        }

        public void Unlisten(Action<ITDHState> callback)
        {
            EventHandler -= callback;
        }
    }
}
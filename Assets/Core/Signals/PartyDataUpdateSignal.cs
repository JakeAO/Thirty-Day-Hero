using System;
using SadPumpkin.Util.Signals;

namespace Core.Signals
{
    public class PartyDataUpdatedSignal : ISignal
    {
        private event Action EventHandler;

        public void Fire()
        {
            EventHandler?.Invoke();
        }

        public void Listen(Action callback)
        {
            EventHandler += callback;
        }

        public void Unlisten(Action callback)
        {
            EventHandler -= callback;
        }
    }
}
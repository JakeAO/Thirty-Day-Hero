using System;
using SadPumpkin.Util.CombatEngine.Actor;
using SadPumpkin.Util.Signals;

namespace Core.Signals
{
    public class ActiveCharacterChangedSignal : ISignal<IInitiativeActor>
    {
        private event Action<IInitiativeActor> EventHandler;
        
        public void Fire(IInitiativeActor value)
        {
            EventHandler?.Invoke(value);
        }

        public void Listen(Action<IInitiativeActor> callback)
        {
            EventHandler += callback;
        }

        public void Unlisten(Action<IInitiativeActor> callback)
        {
            EventHandler -= callback;
        }
    }
}
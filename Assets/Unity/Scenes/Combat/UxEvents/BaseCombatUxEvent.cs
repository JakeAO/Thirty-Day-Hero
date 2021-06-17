using System;
using System.Collections.Generic;
using SadPumpkin.Util.UXEventQueue;

namespace Unity.Scenes.Combat.UxEvents
{
    public abstract class BaseCombatUxEvent : IUpdateUXEvent
    {
        public event Action<IUXEvent> Commenced;
        public event Action<IUXEvent> Completed;

        public virtual bool CanRun(IReadOnlyCollection<IUXEvent> runningEvents) => true;

        public void Run()
        {
            OnRun();

            Commenced?.Invoke(this);
        }

        public void TickUpdate(float deltaTimeMs)
        {
            OnTickUpdate(deltaTimeMs);
        }

        protected virtual void Complete()
        {
            OnCompleted();

            Completed?.Invoke(this);
        }

        protected abstract void OnRun();
        protected abstract void OnTickUpdate(float deltaTimeMs);

        protected virtual void OnCompleted()
        {

        }
    }
}
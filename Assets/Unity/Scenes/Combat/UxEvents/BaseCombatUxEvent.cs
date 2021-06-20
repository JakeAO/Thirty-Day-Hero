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

        private float? _completeInSeconds = null;
        
        public void Run()
        {
            OnRun();

            Commenced?.Invoke(this);
        }

        public void TickUpdate(float deltaTimeMs)
        {
            if (_completeInSeconds.HasValue)
            {
                _completeInSeconds -= deltaTimeMs;
                if (_completeInSeconds.Value <= 0f)
                {
                    _completeInSeconds = null;
                    Complete();
                    return;
                }
            }

            OnTickUpdate(deltaTimeMs);
        }

        protected void Complete()
        {
            OnCompleted();

            Completed?.Invoke(this);
        }
        
        protected void CompleteAfterDelay(float delay)
        {
            if (delay <= 0f)
            {
                Complete();
            }
            else
            {
                _completeInSeconds = delay;
            }
        }

        protected abstract void OnRun();

        protected virtual void OnTickUpdate(float deltaTimeMs) { }
        
        protected virtual void OnCompleted() { }
    }
}
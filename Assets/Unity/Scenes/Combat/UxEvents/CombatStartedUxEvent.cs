using SadPumpkin.Util.CombatEngine.Events;
using SadPumpkin.Util.Context;
using UnityEngine;

namespace Unity.Scenes.Combat.UxEvents
{
    public class CombatStartedUxEvent : BaseCombatUxEvent
    {
        private readonly CombatStartedEvent _eventData;

        public CombatStartedUxEvent(CombatStartedEvent eventData, IContext activeContext)
        {
            _eventData = eventData;
        }
        
        protected override void OnRun()
        {
            // TODO
            Debug.Log($"{nameof(CombatStartedUxEvent)} executed. Combat has begun.");

            CompleteAfterDelay(1f);
        }
    }
}
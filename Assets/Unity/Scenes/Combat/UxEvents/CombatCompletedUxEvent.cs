using SadPumpkin.Util.CombatEngine.Events;
using SadPumpkin.Util.Context;
using UnityEngine;
using UnityEngine.Assertions;

namespace Unity.Scenes.Combat.UxEvents
{
    public class CombatCompletedUxEvent : BaseCombatUxEvent
    {
        private readonly CombatCompletedEvent _eventData;

        public CombatCompletedUxEvent(CombatCompletedEvent eventData, IContext activeContext)
        {
            _eventData = eventData;

            Assert.AreNotEqual(_eventData, default);
        }
        
        protected override void OnRun()
        {
            // TODO
            Debug.Log($"{nameof(CombatCompletedUxEvent)} executed. Combat is finished.");

            CompleteAfterDelay(1f);
        }
    }
}
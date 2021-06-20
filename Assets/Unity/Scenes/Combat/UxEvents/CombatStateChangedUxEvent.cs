using SadPumpkin.Util.CombatEngine.Events;
using SadPumpkin.Util.Context;
using UnityEngine;
using UnityEngine.Assertions;

namespace Unity.Scenes.Combat.UxEvents
{
    public class CombatStateChangedUxEvent : BaseCombatUxEvent
    {
        private readonly CombatStateChangedEvent _eventData;

        public CombatStateChangedUxEvent(CombatStateChangedEvent eventData, IContext activeContext)
        {
            _eventData = eventData;

            Assert.AreNotEqual(_eventData, default);
        }

        protected override void OnRun()
        {
            // TODO
            Debug.Log($"{nameof(CombatStateChangedUxEvent)} executed. Combat state changed to {_eventData.NewState}");

            CompleteAfterDelay(1f);
        }
    }
}
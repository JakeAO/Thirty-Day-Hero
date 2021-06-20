using Core.States.Combat.Events;
using SadPumpkin.Util.Context;
using UnityEngine;
using UnityEngine.Assertions;

namespace Unity.Scenes.Combat.UxEvents
{
    public class ActorResurrectedUxEvent : BaseCombatUxEvent
    {
        private readonly ActorResurrectedEvent _eventData;

        public ActorResurrectedUxEvent(ActorResurrectedEvent eventData, IContext activeContext)
        {
            _eventData = eventData;

            Assert.IsNotNull(_eventData);
        }

        protected override void OnRun()
        {
            // TODO
            Debug.Log($"{nameof(ActorResurrectedUxEvent)} executed. Actor id {_eventData.ActorId} has returned from death.");

            CompleteAfterDelay(1f);
        }
    }
}
using Core.States.Combat.Events;
using SadPumpkin.Util.Context;
using UnityEngine;
using UnityEngine.Assertions;

namespace Unity.Scenes.Combat.UxEvents
{
    public class ActorDiedUxEvent : BaseCombatUxEvent
    {
        private readonly ActorDiedEvent _eventData;

        public ActorDiedUxEvent(ActorDiedEvent eventData, IContext activeContext)
        {
            _eventData = eventData;

            Assert.IsNotNull(_eventData);
        }
        
        protected override void OnRun()
        {
            // TODO
            Debug.Log($"{nameof(ActorDiedUxEvent)} executed. Actor id {_eventData.ActorId} has been killed.");

            CompleteAfterDelay(1f);
        }
    }
}
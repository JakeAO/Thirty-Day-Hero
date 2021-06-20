using Core.States.Combat.Events;
using SadPumpkin.Util.Context;
using UnityEngine;
using UnityEngine.Assertions;

namespace Unity.Scenes.Combat.UxEvents
{
    public class PartyChangedUxEvent : BaseCombatUxEvent
    {
        private readonly PartyChangedEvent _eventData;

        public PartyChangedUxEvent(PartyChangedEvent eventData, IContext activeContext)
        {
            _eventData = eventData;

            Assert.IsNotNull(_eventData);
        }
        
        protected override void OnRun()
        {
            // TODO
            Debug.Log($"{nameof(PartyChangedUxEvent)} executed. Actor id {_eventData.ActorId} became member of party id {_eventData.NewPartyId}");

            CompleteAfterDelay(1f);
        }
    }
}
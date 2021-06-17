using System;
using Core.States.Combat;
using Core.States.Combat.Events;
using SadPumpkin.Util.Context;
using Unity.Scenes.Shared.Entities;
using UnityEngine;
using UnityEngine.Assertions;

namespace Unity.Scenes.Combat.UxEvents
{
    public class PartyChangedUxEvent : BaseCombatUxEvent
    {
        private readonly PartyChangedEvent _eventData;
        private readonly IActorViewManager _actorViewManager;
        private readonly CombatDataWrapper _combatDataWrapper;

        public PartyChangedUxEvent(PartyChangedEvent eventData, IContext activeContext)
        {
            _eventData = eventData;
            _actorViewManager = activeContext.Get<IActorViewManager>();
            _combatDataWrapper = activeContext.Get<CombatDataWrapper>();

            Assert.IsNotNull(_eventData);
            Assert.IsNotNull(_actorViewManager);
            Assert.IsNotNull(_combatDataWrapper);
        }
        
        protected override void OnRun()
        {
            // TODO
            Debug.Log($"{nameof(PartyChangedUxEvent)} executed. Actor id {_eventData.ActorId} became member of party id {_eventData.NewPartyId}");

            Complete();
        }

        protected override void OnTickUpdate(float deltaTimeMs)
        {
        }
    }
}
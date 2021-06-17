using System;
using Core.States.Combat;
using Core.States.Combat.Events;
using SadPumpkin.Util.Context;
using Unity.Scenes.Shared.Entities;
using UnityEngine;
using UnityEngine.Assertions;

namespace Unity.Scenes.Combat.UxEvents
{
    public class ActorDiedUxEvent : BaseCombatUxEvent
    {
        private readonly ActorDiedEvent _eventData;
        private readonly IActorViewManager _actorViewManager;
        private readonly CombatDataWrapper _combatDataWrapper;

        public ActorDiedUxEvent(ActorDiedEvent eventData, IContext activeContext)
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
            Debug.Log($"{nameof(ActorDiedUxEvent)} executed. Actor id {_eventData.ActorId} has been killed.");

            Complete();
        }

        protected override void OnTickUpdate(float deltaTimeMs)
        {
        }
    }
}
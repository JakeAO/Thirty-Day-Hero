using System;
using Core.States.Combat;
using Core.States.Combat.Events;
using SadPumpkin.Util.Context;
using Unity.Scenes.Shared.Entities;
using UnityEngine;
using UnityEngine.Assertions;

namespace Unity.Scenes.Combat.UxEvents
{
    public class ActorResurrectedUxEvent : BaseCombatUxEvent
    {
        private readonly ActorResurrectedEvent _eventData;
        private readonly IActorViewManager _actorViewManager;
        private readonly CombatDataWrapper _combatDataWrapper;

        public ActorResurrectedUxEvent(ActorResurrectedEvent eventData, IContext activeContext)
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
            Debug.Log($"{nameof(ActorResurrectedUxEvent)} executed. Actor id {_eventData.ActorId} has returned from death.");

            Complete();
        }

        protected override void OnTickUpdate(float deltaTimeMs)
        {
        }
    }
}
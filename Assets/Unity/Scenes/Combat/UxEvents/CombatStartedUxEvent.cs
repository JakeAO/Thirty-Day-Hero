using System;
using Core.States.Combat;
using SadPumpkin.Util.CombatEngine.Events;
using SadPumpkin.Util.Context;
using Unity.Scenes.Shared.Entities;
using UnityEngine;
using UnityEngine.Assertions;

namespace Unity.Scenes.Combat.UxEvents
{
    public class CombatStartedUxEvent : BaseCombatUxEvent
    {
        private readonly CombatStartedEvent _eventData;
        private readonly IActorViewManager _actorViewManager;
        private readonly CombatDataWrapper _combatDataWrapper;

        public CombatStartedUxEvent(CombatStartedEvent eventData, IContext activeContext)
        {
            _eventData = eventData;
            _actorViewManager = activeContext.Get<IActorViewManager>();
            _combatDataWrapper = activeContext.Get<CombatDataWrapper>();

            Assert.IsNotNull(_actorViewManager);
            Assert.IsNotNull(_combatDataWrapper);
        }
        
        protected override void OnRun()
        {
            // TODO
            Debug.Log($"{nameof(CombatStartedUxEvent)} executed. Combat has begun.");

            Complete();
        }

        protected override void OnTickUpdate(float deltaTimeMs)
        {
        }
    }
}
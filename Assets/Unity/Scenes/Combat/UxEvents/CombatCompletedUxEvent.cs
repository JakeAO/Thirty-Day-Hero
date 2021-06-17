using System;
using Core.States.Combat;
using SadPumpkin.Util.CombatEngine.Events;
using SadPumpkin.Util.Context;
using Unity.Scenes.Shared.Entities;
using UnityEngine;
using UnityEngine.Assertions;

namespace Unity.Scenes.Combat.UxEvents
{
    public class CombatCompletedUxEvent : BaseCombatUxEvent
    {
        private readonly CombatCompletedEvent _eventData;
        private readonly IActorViewManager _actorViewManager;
        private readonly CombatDataWrapper _combatDataWrapper;

        public CombatCompletedUxEvent(CombatCompletedEvent eventData, IContext activeContext)
        {
            _eventData = eventData;
            _actorViewManager = activeContext.Get<IActorViewManager>();
            _combatDataWrapper = activeContext.Get<CombatDataWrapper>();

            Assert.AreNotEqual(_eventData, default);
            Assert.IsNotNull(_actorViewManager);
            Assert.IsNotNull(_combatDataWrapper);
        }
        
        protected override void OnRun()
        {
            // TODO
            Debug.Log($"{nameof(CombatCompletedUxEvent)} executed. Combat is finished.");

            Complete();
        }

        protected override void OnTickUpdate(float deltaTimeMs)
        {
        }
    }
}
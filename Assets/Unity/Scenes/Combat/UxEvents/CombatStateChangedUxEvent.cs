using System;
using Core.States.Combat;
using SadPumpkin.Util.CombatEngine.Events;
using SadPumpkin.Util.Context;
using Unity.Scenes.Shared.Entities;
using UnityEngine;
using UnityEngine.Assertions;

namespace Unity.Scenes.Combat.UxEvents
{
    public class CombatStateChangedUxEvent : BaseCombatUxEvent
    {
        private readonly CombatStateChangedEvent _eventData;
        private readonly IActorViewManager _actorViewManager;
        private readonly CombatDataWrapper _combatDataWrapper;

        public CombatStateChangedUxEvent(CombatStateChangedEvent eventData, IContext activeContext)
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
            Debug.Log($"{nameof(CombatStateChangedUxEvent)} executed. Combat state changed to {_eventData.NewState}");

            Complete();
        }

        protected override void OnTickUpdate(float deltaTimeMs)
        {
        }
    }
}
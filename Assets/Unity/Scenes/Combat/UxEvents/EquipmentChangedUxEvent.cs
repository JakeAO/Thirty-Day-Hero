using System;
using Core.States.Combat;
using Core.States.Combat.Events;
using SadPumpkin.Util.Context;
using Unity.Scenes.Shared.Entities;
using UnityEngine;
using UnityEngine.Assertions;

namespace Unity.Scenes.Combat.UxEvents
{
    public class EquipmentChangedUxEvent : BaseCombatUxEvent
    {
        private readonly EquipmentChangedEvent _eventData;
        private readonly IActorViewManager _actorViewManager;
        private readonly CombatDataWrapper _combatDataWrapper;
        
        public EquipmentChangedUxEvent(EquipmentChangedEvent eventData, IContext activeContext)
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
            Debug.Log($"{nameof(ActiveActorChangedUxEvent)} executed. Actor id {_eventData.ActorId}'s equipment slot {_eventData.Slot} changed to item id {_eventData.NewItemId}");

            Complete();
        }

        protected override void OnTickUpdate(float deltaTimeMs)
        {
        }
    }
}
using System;
using Core.Actors;
using Core.States.Combat;
using Core.States.Combat.Events;
using SadPumpkin.Util.Context;
using Unity.Scenes.Shared.Entities;
using UnityEngine.Assertions;

namespace Unity.Scenes.Combat.UxEvents
{
    public class StatChangedUxEvent : BaseCombatUxEvent
    {
        private readonly StatChangedEvent _eventData;
        private readonly IActorViewManager _actorViewManager;
        private readonly CombatDataWrapper _combatDataWrapper;
        
        public StatChangedUxEvent(StatChangedEvent eventData, IContext activeContext)
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
            if (!_combatDataWrapper.TryGetActor(_eventData.ActorId, out ICharacterActor actorData) || actorData == null)
            {
                throw new InvalidOperationException($"{nameof(StatChangedUxEvent)} executed with actor id {_eventData.ActorId}, but no data could be found!");
            }

            if (!_actorViewManager.TryGetView(_eventData.ActorId, out IActorView actorView) || actorView == null)
            {
                throw new InvalidOperationException($"{nameof(StatChangedUxEvent)} executed with actor id {_eventData.ActorId}, but no view could be found!");
            }

            actorView.UpdateModel(actorData);

            Complete();
        }

        protected override void OnTickUpdate(float deltaTimeMs)
        {
            // no op
        }
    }
}
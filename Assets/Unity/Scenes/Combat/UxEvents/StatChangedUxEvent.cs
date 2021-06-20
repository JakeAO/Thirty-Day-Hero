using System;
using Core.Actors;
using Core.Actors.Calamity;
using Core.Actors.Enemy;
using Core.Actors.Player;
using Core.States.Combat.Events;
using Core.States.Combat.GameState;
using SadPumpkin.Util.Context;
using Unity.Scenes.Combat.Battlefield;
using Unity.Scenes.Shared.Entities;
using UnityEngine;
using UnityEngine.Assertions;

namespace Unity.Scenes.Combat.UxEvents
{
    public class StatChangedUxEvent : BaseCombatUxEvent
    {
        private readonly StatChangedEvent _eventData;
        private readonly IActorViewManager<CombatPlayerView> _playerViewManager;
        private readonly IActorViewManager<CombatEnemyView> _enemyViewManager;
        private readonly IGameState _gameState;

        public StatChangedUxEvent(StatChangedEvent eventData, IContext activeContext)
        {
            _eventData = eventData;
            _playerViewManager = activeContext.Get<IActorViewManager<CombatPlayerView>>();
            _enemyViewManager = activeContext.Get<IActorViewManager<CombatEnemyView>>();
            _gameState = activeContext.Get<IGameState>();

            Assert.IsNotNull(_eventData);
            Assert.IsNotNull(_playerViewManager);
            Assert.IsNotNull(_enemyViewManager);
            Assert.IsNotNull(_gameState);
        }

        protected override void OnRun()
        {
            // Get Actor Data
            if (!_gameState.TryGetActor(_eventData.ActorId, out ICharacterActor actorData) || actorData == null)
            {
                throw new InvalidOperationException($"{nameof(StatChangedUxEvent)} executed with actor id {_eventData.ActorId}, but no data could be found!");
            }

            // Get Actor View
            IActorView actorView = null;
            switch (actorData)
            {
                case IPlayerCharacterActor _:
                    _playerViewManager.TryGetView(_eventData.ActorId, out actorView);
                    break;
                case IEnemyCharacterActor _:
                case ICalamityCharacterActor _:
                    _enemyViewManager.TryGetView(_eventData.ActorId, out actorView);
                    break;
            }

            // Update Actor View
            if (actorView != null)
            {
                actorView.UpdateModel(actorData, new ActorUpdateContext(changedStat: _eventData.Stat));

                Debug.Log($"Actor \"{actorData.Name}\" {_eventData.Stat} changed from {_eventData.OldValue} to {_eventData.NewValue}.)");
            }
            else
            {
                throw new InvalidOperationException($"{nameof(StatChangedUxEvent)} executed with actor id {_eventData.ActorId}, but no view could be found!");
            }

            CompleteAfterDelay(1f);
        }
    }
}
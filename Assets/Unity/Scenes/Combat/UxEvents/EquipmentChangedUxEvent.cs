using System;
using Core.Actors;
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
    public class EquipmentChangedUxEvent : BaseCombatUxEvent
    {
        private readonly EquipmentChangedEvent _eventData;
        private readonly IActorViewManager<CombatPlayerView> _playerViewManager;
        private readonly IGameState _gameState;

        public EquipmentChangedUxEvent(EquipmentChangedEvent eventData, IContext activeContext)
        {
            _eventData = eventData;
            _playerViewManager = activeContext.Get<IActorViewManager<CombatPlayerView>>();
            _gameState = activeContext.Get<IGameState>();

            Assert.IsNotNull(_eventData);
            Assert.IsNotNull(_playerViewManager);
            Assert.IsNotNull(_gameState);
        }

        protected override void OnRun()
        {
            // Get Actor Data
            if (!_gameState.TryGetActor(_eventData.ActorId, out ICharacterActor actorData) ||
                !(actorData is IPlayerCharacterActor playerData))
            {
                throw new InvalidOperationException($"{nameof(EquipmentChangedUxEvent)} executed with actor id {_eventData.ActorId}, but no data could be found!");
            }

            // Get Actor View
            _playerViewManager.TryGetView(_eventData.ActorId, out IActorView playerView);
            
            // Update Actor View
            if (playerView != null)
            {
                playerView.UpdateModel(actorData, new ActorUpdateContext(changedEquipment: _eventData.Slot));

                Debug.Log($"Actor \"{playerData.Name}\" {_eventData.Slot} changed to {playerData.Equipment[_eventData.Slot]?.Name ?? "EMPTY"}.)");
            }
            else
            {
                throw new InvalidOperationException($"{nameof(EquipmentChangedUxEvent)} executed with actor id {_eventData.ActorId}, but no view could be found!");
            }

            CompleteAfterDelay(1f);
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using Core.States.Combat.GameState;
using SadPumpkin.Util.CombatEngine.Events;
using SadPumpkin.Util.CombatEngine.Initiatives;
using SadPumpkin.Util.Context;
using Unity.Scenes.Combat.ActionSelection;
using Unity.Scenes.Combat.Battlefield;
using Unity.Scenes.Combat.Etc;
using UnityEngine;
using UnityEngine.Assertions;

namespace Unity.Scenes.Combat.UxEvents
{
    public class ActiveActorChangedUxEvent : BaseCombatUxEvent
    {
        private readonly ActiveActorChangedEvent _eventData;
        private readonly HighlightManager _highlightManager;
        private readonly InitiativePanel _initiativePanel;
        private readonly IInitiativeQueue _initiativeQueue;
        private readonly IGameState _gameState;
        private readonly ActionSelectionWorkflow _actionSelectionWorkflow;

        public ActiveActorChangedUxEvent(ActiveActorChangedEvent eventData, IContext activeContext)
        {
            _eventData = eventData;
            _highlightManager = activeContext.Get<HighlightManager>();
            _initiativePanel = activeContext.Get<InitiativePanel>();
            _initiativeQueue = activeContext.Get<IInitiativeQueue>();
            _gameState = activeContext.Get<IGameState>();
            _actionSelectionWorkflow = activeContext.Get<ActionSelectionWorkflow>();

            Assert.AreNotEqual(_eventData, default);
            Assert.IsNotNull(_highlightManager);
            Assert.IsNotNull(_initiativePanel);
            Assert.IsNotNull(_initiativeQueue);
            Assert.IsNotNull(_gameState);
            Assert.IsNotNull(_actionSelectionWorkflow);
        }

        protected override void OnRun()
        {
            // TODO Remove?
            Debug.Log($"{nameof(ActiveActorChangedUxEvent)} executed. New active actor id: {_eventData.NewActorId}");

            // Update Initiative Panel
            _initiativePanel.UpdateInitiativePreview(_initiativeQueue
                .GetPreview(10)
                .Select(x => _gameState.GetActor(x))
                .Where(x => x != null)
                .ToArray());

            // Update Highlights
            _highlightManager.UpdateHighlights(
                new Dictionary<uint, HighlightType>()
                {
                    [_eventData.NewActorId] = HighlightType.Active
                });

            // Kick Action Workflow
            _actionSelectionWorkflow.ApplyWorkflow();

            CompleteAfterDelay(1f);
        }
    }
}
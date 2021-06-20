using System.Collections.Generic;
using Core.Actors;
using Core.CharacterControllers;
using Core.States.Combat.GameState;
using SadPumpkin.Util.CombatEngine.Action;
using SadPumpkin.Util.CombatEngine.Actor;
using Unity.Scenes.Combat.Battlefield;
using Unity.Scenes.Combat.Etc;
using Unity.Scenes.Shared.Entities;

namespace Unity.Scenes.Combat.ActionSelection
{
    public class ActionSelectionWorkflow
    {
        private readonly PlayerCharacterController _characterController;
        private readonly IActionPromptDisplay _actionPromptDisplay;
        private readonly HighlightManager _highlightManager;
        private readonly IGameState _gameState;
        private readonly IActorViewManager<CombatPlayerView> _playerViewManager;
        private readonly IActorViewManager<CombatEnemyView> _enemyViewManager;

        private readonly Dictionary<uint, List<IAction>> _actionsBySource = new Dictionary<uint, List<IAction>>(10);
        private readonly List<(IAction, bool)> _actionsWithContext = new List<(IAction, bool)>(10);
        private readonly Dictionary<uint, HighlightType> _workflowHighlights = new Dictionary<uint, HighlightType>(10);
        private readonly Dictionary<uint, IAction> _actionsByTargetId = new Dictionary<uint, IAction>(10);

        public ActionSelectionWorkflow(
            PlayerCharacterController characterController,
            IActionPromptDisplay actionPromptDisplay,
            HighlightManager highlightManager,
            IGameState gameState,
            IActorViewManager<CombatPlayerView> playerViewManager,
            IActorViewManager<CombatEnemyView> enemyViewManager)
        {
            _characterController = characterController;
            _actionPromptDisplay = actionPromptDisplay;
            _highlightManager = highlightManager;
            _gameState = gameState;
            _playerViewManager = playerViewManager;
            _enemyViewManager = enemyViewManager;
        }

        public void ApplyWorkflow()
        {
            if (_characterController.ActiveCharacter != null)
            {
                ActionPrompt();
            }
            else
            {
                _actionPromptDisplay.HideActionPrompt();
            }
        }

        private void ActionPrompt()
        {
            uint actorId = _characterController.ActiveCharacter.Id;

            // Group actions by source
            foreach (var actionKvp in _characterController.AvailableActions)
            {
                IAction action = actionKvp.Value;
                uint actionSourceId = action.ActionSource.Id;
                if (!_actionsBySource.TryGetValue(actionSourceId, out var actionList))
                    _actionsBySource[actionSourceId] = actionList = new List<IAction>(10);

                actionList.Add(action);
            }

            // Create action list for prompt
            foreach (var actionKvp in _actionsBySource)
            {
                _actionsWithContext.Add((actionKvp.Value[0], actionKvp.Value.Count != 1));
            }

            // Setup highlights
            foreach (var actor in _gameState.AllActors)
            {
                _workflowHighlights[actorId] = actor.Id == _characterController.ActiveCharacter.Id ? HighlightType.Active : HighlightType.None;
            }

            _actionPromptDisplay.ShowActionPrompt(actorId, _actionsWithContext, OnActionSelected);
            _highlightManager.UpdateHighlights(_workflowHighlights);
        }

        private void OnActionSelected(IAction selectedAction)
        {
            _actionPromptDisplay.HideActionPrompt();

            foreach ((IAction action, bool targetSelectionRequired) in _actionsWithContext)
            {
                if (selectedAction.Id == action.Id)
                {
                    if (targetSelectionRequired)
                    {
                        TargetingPrompt(selectedAction.ActionSource.Id);
                    }
                    else
                    {
                        Cleanup();
                        _characterController.SubmitActionResponse(selectedAction.Id);
                    }

                    return;
                }
            }
        }

        private void TargetingPrompt(uint targetSourceId)
        {
            List<IAction> actionsForSource = _actionsBySource[targetSourceId];

            // Setup target mapping
            foreach (IAction action in actionsForSource)
            {
                foreach (ITargetableActor target in action.Targets)
                {
                    _actionsByTargetId[target.Id] = action;
                }
            }

            // Set interaction listeners
            foreach (ICharacterActor actorData in _gameState.AllActors)
            {
                IInteractableView<IActorView> actorView = GetViewForActorId(actorData.Id);
                if (actorView != null)
                {
                    actorView.Interacted += OnTargetInteracted;
                }
            }

            // Setup highlights
            _workflowHighlights.Clear();
            foreach (IAction action in actionsForSource)
            {
                foreach (ITargetableActor target in action.Targets)
                {
                    _workflowHighlights[target.Id] = HighlightType.Selected;
                }
            }

            _highlightManager.UpdateHighlights(_workflowHighlights);
        }

        private void OnTargetInteracted(IActorView selectedActor, InteractionType interactionType)
        {
            if (interactionType == InteractionType.LeftClick &&
                _actionsByTargetId.TryGetValue(selectedActor.Model.Id, out IAction targetedAction))
            {
                Cleanup();
                _characterController.SubmitActionResponse(targetedAction.Id);
            }
        }

        private void Cleanup()
        {
            _actionsBySource.Clear();
            _actionsWithContext.Clear();
            _workflowHighlights.Clear();
            _actionsByTargetId.Clear();

            foreach (ICharacterActor actorData in _gameState.AllActors)
            {
                IInteractableView<IActorView> actorView = GetViewForActorId(actorData.Id);
                if (actorView != null)
                {
                    actorView.Interacted -= OnTargetInteracted;
                }
            }
        }

        private IInteractableView<IActorView> GetViewForActorId(uint actorId)
        {
            if (_enemyViewManager.TryGetView(actorId, out var actorView))
                return (IInteractableView<IActorView>) actorView;
            if (_playerViewManager.TryGetView(actorId, out actorView))
                return (IInteractableView<IActorView>) actorView;
            return null;
        }
    }
}
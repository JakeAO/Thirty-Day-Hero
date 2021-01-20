using System.Collections.Generic;
using System.Linq;
using Core.CharacterControllers;
using Core.CombatSettings;
using Core.Etc;
using Core.EventOptions;
using Core.States.BaseClasses;
using Core.Wrappers;
using SadPumpkin.Util.CombatEngine;
using SadPumpkin.Util.CombatEngine.Action;
using SadPumpkin.Util.CombatEngine.Actor;
using SadPumpkin.Util.CombatEngine.GameState;
using SadPumpkin.Util.StateMachine;
using SadPumpkin.Util.StateMachine.States;

namespace Core.States.Combat
{
    public class CombatMainState : TDHStateBase
    {
        public const string CATEGORY_DEFAULT = "";
        public const string CATEGORY_DEBUG = "Debug";

        private readonly CombatManager _combatManager = null;
        private readonly PlayerCharacterController _controller = null;

        private IStateMachine _stateMachine = null;
        private PartyDataWrapper _partyData = null;

        public CombatSettings.CombatSettings Settings { get; private set; }

        public IGameState CurrentGameState { get; private set; }
        public IInitiativeActor ActiveActor => CurrentGameState != null ? CurrentGameState.ActiveActor : null; //probably use a null obj here

        private CombatResults _results;

        public CombatMainState(
            CombatManager combatManager,
            CombatSettings.CombatSettings settings,
            PlayerCharacterController controller)
        {
            _combatManager = combatManager;
            Settings = settings;
            _controller = controller;

            CurrentGameState = _combatManager.CurrentGameState;

            _controller.ActiveCharacterChanged.Listen(newActor => UpdateOptions());
        }

        public override void OnEnter(IState fromState)
        {
            _stateMachine = SharedContext.Get<IStateMachine>();
            _partyData = SharedContext.Get<PartyDataWrapper>();
            _combatManager.GameStateUpdated.Listen(OnGamestateUpdated);
            _combatManager.CombatComplete.Listen(OnCombatCompleted);
        }

        public override void OnContent()
        {
            UpdateOptions();

            _combatManager.Start(true);
        }

        public override void OnExit(IState toState)
        {
            _combatManager.GameStateUpdated.Unlisten(OnGamestateUpdated);
            _combatManager.CombatComplete.Unlisten(OnCombatCompleted);
        }

        private void OnGamestateUpdated(IGameState gameState)
        {
            CurrentGameState = gameState;
        }

        private void OnCombatCompleted(uint winningPartyId)
        {
            if (_controller.PartyId == winningPartyId)
            {
                _results = CombatResults.CreateSuccess(
                    Settings.Enemies,
                    _partyData);
            }
            else
            {
                _results = CombatResults.CreateFailure();
            }

            UpdateOptions();
        }

        private void UpdateOptions()
        {
            _currentOptions.Clear();

            if (_results != null)
            {
                if (!_currentOptions.TryGetValue(CATEGORY_DEFAULT, out var defaultList))
                    _currentOptions[CATEGORY_DEFAULT] = defaultList = new List<IEventOption>(1);

                defaultList.Add(new EventOption(
                    "Exit Combat",
                    GoToCombatEnd,
                    CATEGORY_DEFAULT));
                return;
            }

            if (_controller?.AvailableActions != null)
            {
                foreach (var kvp in _controller.AvailableActions)
                {
                    IAction action = kvp.Value;

                    string category = (action.ActionProvider as INamed)?.Name ?? "Other";
                    if (!_currentOptions.TryGetValue(category, out var categoryList))
                        _currentOptions[category] = categoryList = new List<IEventOption>(5);

                    if (action.Available)
                    {
                        categoryList.Add(new EventOption(
                            GetActionTextFromAction(action),
                            () => OnActionSelected(action),
                            category,
                            0u,
                            !action.Available,
                            action));
                    }
                }
            }

            if (!_currentOptions.TryGetValue(CATEGORY_DEBUG, out var debugList))
                _currentOptions[CATEGORY_DEBUG] = debugList = new List<IEventOption>(2);

            debugList.Add(new EventOption(
                "Win Combat",
                () =>
                {
                    _results = CombatResults.CreateSuccess(Settings.Enemies, _partyData);
                    GoToCombatEnd();
                },
                CATEGORY_DEBUG));
            debugList.Add(new EventOption(
                "Lose Combat",
                () =>
                {
                    _results = CombatResults.CreateFailure();
                    GoToCombatEnd();
                },
                CATEGORY_DEBUG));

            OptionsChangedSignal?.Fire(this);
        }

        private string GetActionTextFromAction(IAction action)
        {
            if (action?.ActionSource is INamed named)
            {
                return $"{named.Name}: {string.Join(", ", action.Targets.Select(x => x.Name))}";
            }
            else
            {
                return $"ACTION {action.Id}: {string.Join(", ", action.Targets.Select(x => x.Name))}";
            }
        }

        private void OnActionSelected(IAction action)
        {
            _controller.SubmitActionResponse(action.Id);
        }

        private void GoToCombatEnd()
        {
            _stateMachine.ChangeState(
                new CombatEndState(
                    Settings,
                    _results));
        }
    }
}
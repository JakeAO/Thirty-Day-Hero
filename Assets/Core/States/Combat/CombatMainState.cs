using System.Collections.Generic;
using System.Linq;
using Core.CharacterControllers;
using Core.Etc;
using Core.EventOptions;
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
        private readonly CombatManager _combatManager = null;
        private readonly PlayerCharacterController _controller = null;
        
        private IStateMachine _stateMachine = null;

        public CombatSettings.CombatSettings Settings { get; private set; }

        public IGameState CurrentGameState { get; private set; }
        public IInitiativeActor ActiveActor => CurrentGameState != null ? CurrentGameState.ActiveActor : null; //probably use a null obj here

        public CombatMainState(
            CombatManager combatManager,
            CombatSettings.CombatSettings settings,
            PlayerCharacterController controller)
        {
            _combatManager = combatManager;
            Settings = settings;
            _controller = controller;

            CurrentGameState = _combatManager.CurrentGameState;
        }

        public override void OnEnter(IState fromState)
        {
            _stateMachine = SharedContext.Get<IStateMachine>();
            _combatManager.GameStateUpdated.Listen(OnGamestateUpdated);
            _combatManager.CombatComplete.Listen(OnCombatComplete);
        }

        public override void OnContent()
        {
            _combatManager.Start(true);
        }

        public override void OnExit(IState toState)
        {
            _combatManager.GameStateUpdated.Unlisten(OnGamestateUpdated);
            _combatManager.CombatComplete.Unlisten(OnCombatComplete);
        }

        public void OnGamestateUpdated(IGameState gameState)
        {
            CurrentGameState = gameState;
        }

        public void OnCombatComplete(uint winningPartyId)
        {
            GoToCombatEnd();
        }

        public override IEnumerable<IEventOption> GetOptions()
        {
            const string ACTION_CATEGORY_DEBUG = "Debug";

            if (_controller != null && _controller.AvailableActions != null)
            {
                foreach (var kvp in _controller.AvailableActions)
                {
                    IAction action = kvp.Value;

                    if (action.Available)
                    {
                        yield return new EventOption(
                            GetActionTextFromAction(action),
                            () => OnActionSelected(action),
                            (action.ActionProvider as INamed)?.Name ?? "Other",
                            context: action);
                    }
                }
            }

            yield return new EventOption(
                "Win Combat",
                GoToCombatEnd,
                ACTION_CATEGORY_DEBUG);
            yield return new EventOption(
                "Lose Combat",
                GoToCombatEnd,
                ACTION_CATEGORY_DEBUG);
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
            _stateMachine.ChangeState<CombatEndState>();
        }
    }
}
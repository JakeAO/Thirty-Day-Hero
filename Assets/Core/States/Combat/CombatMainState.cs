using System.Collections.Generic;
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
        private readonly IStateMachine _stateMachine = null;

        public CombatSettings.CombatSettings Settings { get; private set; }

        public IGameState CurrentGameState { get; private set; }
        public IInitiativeActor ActiveActor => CurrentGameState != null ? CurrentGameState.ActiveActor : null; //probably use a null obj here

        public CombatMainState(
            CombatManager combatManager,
            CombatSettings.CombatSettings settings,
            PlayerCharacterController controller,
            IStateMachine stateMachine)
        {
            _combatManager = combatManager;
            Settings = settings;
            _controller = controller;
            _stateMachine = stateMachine;

            CurrentGameState = _combatManager.CurrentGameState;
        }

        public override void OnEnter(IState fromState)
        {
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

        private Dictionary<uint, IAction> _tmpActions = new Dictionary<uint, IAction>();
        public override IEnumerable<IEventOption> GetOptions()
        {
            const string ACTION_CATEGORY_DEBUG = "Debug";
            const string ACTION_CATEGORY_PLAYER = "Actions";

            if(_controller != null && _controller.AvailableActions != null)
            {
                _tmpActions.Clear();

                foreach(var kvp in _controller.AvailableActions)
                {
                    _tmpActions[kvp.Key] = kvp.Value;
                }

                foreach (var kvp in _tmpActions)
                {
                    IAction action = kvp.Value;

                    if (action.Available)
                    {
                        yield return new EventOption(
                            GetActionTextFromAction(action),
                            () => OnActionSelected(action),
                            ACTION_CATEGORY_PLAYER);
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
            if(action is INamed named)
            {
                return named.Name;
            }
            else
            {
                return $"ACTION ID {action.Id}";
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
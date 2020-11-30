using System;
using System.Collections.Generic;
using Core.Actions;
using Core.Actors.Player;
using Core.CharacterControllers;
using Core.EventOptions;
using SadPumpkin.Util.CombatEngine;
using SadPumpkin.Util.CombatEngine.Action;
using SadPumpkin.Util.CombatEngine.Party;
using SadPumpkin.Util.CombatEngine.Signals;
using SadPumpkin.Util.StateMachine;
using SadPumpkin.Util.StateMachine.States;

namespace Core.States.Combat
{
    public class CombatMainState : TDHStateBase
    {
        private CombatManager _combatManager;
        private CombatSettings.CombatSettings _combatSettings;
        private IStateMachine _stateMachine;
        private PlayerCharacterController _characterController;
        
        private GameStateUpdated _gameStateUpdated = null;
        private CombatComplete _combatComplete = null;

        public IPlayerCharacterActor ActivePlayer => _characterController.ActiveCharacter;

        public override void OnEnter(IState fromState)
        {
            _stateMachine = SharedContext.Get<IStateMachine>();
            if (_stateMachine == null)
                throw new ArgumentException("Entered CombatMain without an active StateMachine!");

            _combatManager = SharedContext.Get<CombatManager>();
            if (_combatManager == null)
                throw new ArgumentException("Entered CombatMain without an active CombatManager!");

            _combatSettings = SharedContext.Get<CombatSettings.CombatSettings>();
            if (_combatSettings == null)
                throw new ArgumentException("Entered CombatMain without an active CombatSettings!");

            _characterController = SharedContext.Get<PlayerCharacterController>();
            if (_characterController == null)
                throw new ArgumentException("Entered CombatMain without an active CharacterController!");

            _gameStateUpdated = SharedContext.Get<GameStateUpdated>();
            if (_gameStateUpdated == null)
                throw new ArgumentException("Entered CombatMain without a GameStateUpdated signal!");

            _combatComplete = SharedContext.Get<CombatComplete>();
            if (_combatComplete == null)
                throw new ArgumentException("Entered CombatMain without a CombatComplete signal!");
        }

        public override void OnContent()
        {
            _combatManager.Start(true);
        }

        public override IEnumerable<IEventOption> GetOptions()
        { 
            
            
            // Debug
            yield return new EventOption(
                "Win Combat",
                GoToCombatEnd,
                "DEBUG");
            yield return new EventOption(
                "Lose Combat",
                GoToCombatEnd,
                "DEBUG");
        }

        private void GoToCombatEnd()
        {
            _stateMachine.ChangeState<CombatEndState>();
        }
    }
}
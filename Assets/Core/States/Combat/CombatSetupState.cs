using System;
using System.Collections.Generic;
using Core.CharacterControllers;
using Core.EventOptions;
using Core.Signals;
using Core.Wrappers;
using SadPumpkin.Util.CombatEngine;
using SadPumpkin.Util.CombatEngine.Party;
using SadPumpkin.Util.StateMachine;
using SadPumpkin.Util.StateMachine.States;

namespace Core.States.Combat
{
    public class CombatSetupState : TDHStateBase
    {
        public readonly CombatSettings.CombatSettings CombatSettings = null;

        private PartyDataWrapper _partyDataWrapper = null;

        private CombatManager _combatManager = null;
        private PlayerCharacterController _playerController = null;

        public CombatSetupState(CombatSettings.CombatSettings combatSettings)
        {
            CombatSettings = combatSettings;
        }

        public override void OnEnter(IState fromState)
        {
            _partyDataWrapper = SharedContext.Get<PartyDataWrapper>();
            if (_partyDataWrapper == null)
                throw new ArgumentException("Entered CombatSetupState without setting PartyDataWrapper in Context!");
        }

        public override void OnContent()
        {
            // Create Player Party
            _playerController = new PlayerCharacterController(_partyDataWrapper, SharedContext.Get<ActiveCharacterChangedSignal>());
            IParty playerParty = new Party.Party(
                _partyDataWrapper.PartyId,
                _playerController,
                _partyDataWrapper.Characters);

            // Create Enemy Party
            IParty enemyParty = new Party.Party(
                CombatSettings.EnemyPartyId,
                CombatSettings.Controller,
                CombatSettings.Enemies);

            // Create CombatManager
            _combatManager = new CombatManager(
                new[] {playerParty, enemyParty},
                new Actions.StandardActionGenerator(),
                _playerController.GameStateUpdatedSignal,
                _playerController.CombatCompleteSignal);
        }

        public override IEnumerable<IEventOption> GetOptions()
        {
            yield return new EventOption("Begin Combat", BeginCombat);
        }

        private void BeginCombat()
        {
            SharedContext.Get<IStateMachine>().ChangeState(
                new CombatMainState(
                    _combatManager,
                    CombatSettings,
                    _playerController));
        }
    }
}
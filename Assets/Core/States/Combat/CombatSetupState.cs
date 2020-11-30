using System.Collections.Generic;
using Core.Actors;
using Core.Actors.Enemy;
using Core.CharacterControllers;
using Core.Classes.Enemy;
using Core.CombatSettings;
using Core.EventOptions;
using Core.Signals;
using Core.StatMap;
using Core.Wrappers;
using SadPumpkin.Util.CombatEngine;
using SadPumpkin.Util.CombatEngine.CharacterControllers;
using SadPumpkin.Util.CombatEngine.Party;
using SadPumpkin.Util.StateMachine;
using SadPumpkin.Util.StateMachine.States;

namespace Core.States.Combat
{
    public class CombatSetupState : TDHStateBase
    {
        private IStateMachine _stateMachine = null;
        private PartyDataWrapper _partyDataWrapper = null;

        public override IEnumerable<IEventOption> GetOptions()
        {
            yield return new EventOption("Begin Fight", GoToCombatMain);
        }

        public override void OnEnter(IState fromState)
        {
            _stateMachine = SharedContext.Get<IStateMachine>();
            if(_stateMachine == null)
                throw new System.ArgumentException("Entered CombatSetupState without setting IStateMachine in Context!");

            _partyDataWrapper = SharedContext.Get<PartyDataWrapper>();
            if (_partyDataWrapper == null)
                throw new System.ArgumentException("Entered CombatSetupState without setting PartyDataWrapper in Context!");
        }

        private void GoToCombatMain()
        {
            uint enemyPartyId = (uint) System.Guid.NewGuid().GetHashCode();

            List<EnemyCharacter> enemies = GenerateEnemies(_partyDataWrapper, enemyPartyId);
            SetupEnemies(enemies);

            PlayerCharacterController playerCharacterController = new PlayerCharacterController(_partyDataWrapper, SharedContext.Get<ActiveCharacterChangedSignal>());

            CombatSettings.CombatSettings combatSettings = new CombatSettings.CombatSettings(enemies, playerCharacterController);

            IParty[] parties = new IParty[2]
            {
                new Party.Party(
                    _partyDataWrapper.PartyId,
                    playerCharacterController,
                    _partyDataWrapper.Characters),
                new Party.Party(
                    enemyPartyId,
                    new RandomCharacterController(), 
                    enemies)
            };

            CombatManager combatManager = new CombatManager(
                parties,
                new Actions.StandardActionGenerator(),
                playerCharacterController.GameStateUpdatedSignal,
                playerCharacterController.CombatCompleteSignal);

            CombatMainState combatState = new CombatMainState(combatManager, combatSettings, playerCharacterController, _stateMachine);

            SharedContext.Get<IStateMachine>().ChangeState(combatState);
        }

        private List<EnemyCharacter> GenerateEnemies(PartyDataWrapper partyDataWrapper, uint partyId)
        {
            EnemyGroupWrapper debugEnemyGroup = new EnemyGroupWrapper(0, Etc.RarityCategory.Common, new Dorp());

            IReadOnlyCollection<IEnemyClass> enemyClasses = CombatSettingsGenerator.EnemyClassFromGroup(debugEnemyGroup, Etc.CombatDifficulty.Easy, partyDataWrapper);
            return CombatSettingsGenerator.GenerateEnemies(enemyClasses, partyId);
        }

        private void SetupEnemies(List<EnemyCharacter> enemies)
        {
            //TODO: Address Combat Difficulty
            //CombatSettingsGenerator.SetEnemyDifficulty(Etc.CombatDifficulty.Easy, partyDataWrapper, enemyCharacters);
        }
    }
}
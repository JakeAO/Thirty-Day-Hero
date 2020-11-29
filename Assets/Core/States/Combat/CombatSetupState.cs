using System.Collections.Generic;
using Core.Actors.Enemy;
using Core.Actors.Player;
using Core.CharacterControllers;
using Core.Classes.Enemy;
using Core.CombatSettings;
using Core.EventOptions;
using Core.Signals;
using Core.Wrappers;
using SadPumpkin.Util.CombatEngine;
using SadPumpkin.Util.CombatEngine.CharacterControllers;
using SadPumpkin.Util.CombatEngine.Party;
using SadPumpkin.Util.CombatEngine.Signals;
using SadPumpkin.Util.StateMachine;

namespace Core.States.Combat
{
    public class CombatSetupState : TDHStateBase
    {
        public override IEnumerable<IEventOption> GetOptions()
        {
            yield return new EventOption("Begin Fight", GoToCombatMain);
        }

        private void GoToCombatMain()
        {
            PartyDataWrapper partyDataWrapper = SharedContext.Get<PartyDataWrapper>();

            uint enemyPartyId = (uint)System.Guid.NewGuid().GetHashCode();

            List<EnemyCharacter> enemies = GenerateEnemies(partyDataWrapper, enemyPartyId);
            SetupEnemies(enemies);
            PlayerCharacterController playerCharacterController = GenerateCharacterController(partyDataWrapper);
            SharedContext.Set(playerCharacterController);
            SetupCombatSettings(enemies, playerCharacterController);

            List<IParty> parties = new List<IParty>(2)
            {
                new Party.Party(
                    partyDataWrapper.PartyId,
                    playerCharacterController,
                    partyDataWrapper.Characters),
                new Party.Party(
                    enemyPartyId,
                    null,
                    enemies)
            };

            SetupCombatManager(parties);

            SharedContext.Get<IStateMachine>().ChangeState<CombatMainState>();
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

        private PlayerCharacterController GenerateCharacterController(PartyDataWrapper partyDataWrapper)
        {
            return new PlayerCharacterController(partyDataWrapper, SharedContext.Get<ActiveCharacterChangedSignal>());
        }

        private void SetupCombatSettings(List<EnemyCharacter> enemies, PlayerCharacterController playerCharacterController)
        {
            SharedContext.Set(new CombatSettings.CombatSettings(enemies, playerCharacterController));
            SharedContext.Set(playerCharacterController);
        }

        private void SetupCombatManager(List<IParty> parties)
        {
            GameStateUpdated gameStateUpdated = new GameStateUpdated();
            CombatComplete combatComplete = new CombatComplete();

            CombatManager combatManager = new CombatManager(
                parties,
                new Actions.StandardActionGenerator(),
                gameStateUpdated,
                combatComplete);

            SharedContext.Set(gameStateUpdated);
            SharedContext.Set(combatComplete);
            SharedContext.Set(combatManager);
        }
    }
}
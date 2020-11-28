using System.Collections.Generic;
using Core.Actors.Enemy;
using Core.CharacterControllers;
using Core.Classes.Enemy;
using Core.CombatSettings;
using Core.EventOptions;
using Core.Signals;
using Core.Wrappers;
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

            EnemyGroupWrapper debugEnemyGroup = new EnemyGroupWrapper(0, Etc.RarityCategory.Common, new Dorp());

            IReadOnlyCollection<IEnemyClass> enemyClasses = CombatSettingsGenerator.EnemyClassFromGroup(debugEnemyGroup, Etc.CombatDifficulty.Easy, partyDataWrapper);
            List<EnemyCharacter> enemyCharacters = CombatSettingsGenerator.GenerateEnemies(enemyClasses, (uint)System.Guid.NewGuid().GetHashCode());
            
            //TODO: Address Combat Difficulty
            //CombatSettingsGenerator.SetEnemyDifficulty(Etc.CombatDifficulty.Easy, partyDataWrapper, enemyCharacters);

            PlayerCharacterController playerCharacterController = new PlayerCharacterController(partyDataWrapper, SharedContext.Get<ActiveCharacterChangedSignal>());

            SharedContext.Set(new CombatSettings.CombatSettings(enemyCharacters, playerCharacterController));
            SharedContext.Get<IStateMachine>().ChangeState<CombatMainState>();
        }
    }
}
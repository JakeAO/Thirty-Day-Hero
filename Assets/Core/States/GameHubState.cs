using System;
using System.Collections.Generic;
using Core.Actors.Player;
using Core.CombatSettings;
using Core.Etc;
using Core.EventOptions;
using Core.States.Combat;
using Core.States.Town;
using Core.Wrappers;
using SadPumpkin.Util.StateMachine;
using SadPumpkin.Util.StateMachine.States;

namespace Core.States
{
    public class GameHubState : TDHStateBase
    {
        public PartyDataWrapper PartyData { get; private set; }

        public override void OnEnter(IState fromState)
        {
            PartyData = SharedContext.Get<PartyDataWrapper>();

            if (!(fromState is PreGameState))
            {
                PartyData.IncrementTime();
                SaveLoadHelper.SavePartyData(SharedContext);
            }
        }

        public override IEnumerable<IEventOption> GetOptions()
        {
            if (PartyData.Day >= Constants.DAYS_TO_PREPARE)
            {
                yield return new EventOption(
                    "Face the Calamity",
                    DebugGoToCalamity);
                yield break;
            }

            yield return new EventOption(
                "Rest at Camp",
                GoToRest,
                priority: 0);
            yield return new EventOption(
                "Enter Town",
                GoToTownHub,
                priority: 1);
            yield return new EventOption(
                "Patrol Area",
                GoToPatrol,
                priority: 2);
            yield return new EventOption(
                "Search Area (Encounter)",
                GoToEncounter,
                priority: 4);
            
            yield return new EventOption(
                "Face the Calamity EARLY",
                DebugGoToCalamity,
                "DEBUG");
            yield return new EventOption(
                "Gain EXP + 25",
                DebugGainExp,
                "DEBUG");
        }

        private void GoToRest()
        {
            SharedContext.Get<IStateMachine>().ChangeState<RestState>();
        }

        private void GoToTownHub()
        {
            SharedContext.Get<IStateMachine>().ChangeState<TownHubState>();
        }

        private void GoToPatrol()
        {
            SharedContext.Get<IStateMachine>().ChangeState<PatrolState>();
        }

        private void GoToEncounter()
        {
            SharedContext.Get<IStateMachine>().ChangeState<EncounterState>();
        }

        private void DebugGoToCalamity()
        {
            SharedContext.Get<IStateMachine>().ChangeState(
                new CombatSetupState(
                    SharedContext.Get<CombatSettingsGenerator>().CreateFromEnemies(
                        PartyData.Calamity.Party,
                        new[] {PartyData.Calamity})));
        }

        private void DebugGainExp()
        {
            Random random = new Random();
            foreach (PlayerCharacter playerCharacter in PartyData.Characters)
            {
                uint level = playerCharacter.Stats[StatType.LVL];
                playerCharacter.Stats.ModifyStat(StatType.EXP, 25);
                uint newLevel = playerCharacter.Stats[StatType.LVL];
                while (level < newLevel)
                {
                    playerCharacter.Stats = playerCharacter.Class.LevelUpStats.Increment(playerCharacter.Stats, random);
                    level++;
                }
            }
        }
    }
}
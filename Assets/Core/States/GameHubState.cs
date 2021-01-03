using System;
using System.Collections.Generic;
using Core.Actors.Player;
using Core.CombatSettings;
using Core.Etc;
using Core.EventOptions;
using Core.States.BaseClasses;
using Core.States.Combat;
using Core.States.Town;
using Core.Wrappers;
using SadPumpkin.Util.StateMachine;
using SadPumpkin.Util.StateMachine.States;

namespace Core.States
{
    public class GameHubState : TDHStateBase
    {
        public const string CATEGORY_DEFAULT = "";
        public const string CATEGORY_DEBUG = "Debug";
        
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

        public override void OnContent()
        {
            if (!_currentOptions.TryGetValue(CATEGORY_DEBUG, out var debugList))
                _currentOptions[CATEGORY_DEBUG] = debugList = new List<IEventOption>(2);

            debugList.Add(new EventOption(
                "Face the Calamity EARLY",
                DebugGoToCalamity,
                CATEGORY_DEBUG));
            debugList.Add(new EventOption(
                "Gain EXP + 25",
                DebugGainExp,
                CATEGORY_DEBUG));

            if (PartyData.Day >= Constants.DAYS_TO_PREPARE)
            {
                _currentOptions[CATEGORY_DEFAULT] = new List<IEventOption>()
                {
                    new EventOption(
                        "Face the Calamity",
                        DebugGoToCalamity,
                        CATEGORY_DEFAULT)
                };
            }
            else
            {
                if (!_currentOptions.TryGetValue(CATEGORY_DEFAULT, out var defaultList))
                    _currentOptions[CATEGORY_DEFAULT] = defaultList = new List<IEventOption>(5);

                defaultList.Clear();
                defaultList.Add(new EventOption(
                    "Rest at Camp",
                    GoToRest,
                    CATEGORY_DEFAULT,
                    0));
                defaultList.Add(new EventOption(
                    "Enter Town",
                    GoToTownHub,
                    CATEGORY_DEFAULT,
                    1));
                defaultList.Add(new EventOption(
                    "Patrol Area",
                    GoToPatrol,
                    CATEGORY_DEFAULT,
                    2));
                defaultList.Add(new EventOption(
                    "Search Area (Encounter)",
                    GoToEncounter,
                    CATEGORY_DEFAULT,
                    4));
            }
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
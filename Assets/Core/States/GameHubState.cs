using System;
using System.Collections.Generic;
using Core.Actors.Player;
using Core.CombatSettings;
using Core.Etc;
using Core.EventOptions;
using Core.States.BaseClasses;
using Core.States.Combat;
using Core.States.SubStates;
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

        public EquipmentSubState EquipmentSubState { get; private set; }

        public override void OnEnter(IState fromState)
        {
            PartyData = SharedContext.Get<PartyDataWrapper>();

            if (!(fromState is PreGameState))
            {
                PartyData.IncrementTime();
                SaveLoadHelper.SavePartyData(SharedContext);
            }

            EquipmentSubState = new EquipmentSubState(SharedContext);
        }

        public override void OnContent()
        {
            SetupOptions();
        }

        private void SetupOptions()
        {
            _currentOptions.Clear();
            if (EquipmentSubState.Active)
            {
                SetupOptions_ChangeEquipment();
            }
            else if (PartyData.Day >= Constants.DAYS_TO_PREPARE)
            {
                SetupOptions_Calamity();
            }
            else
            {
                SetupOptions_Default();
            }

            OptionsChangedSignal?.Fire(this);
        }

        private void SetupOptions_Default()
        {
            var debugList = _currentOptions[CATEGORY_DEBUG] = new List<IEventOption>(5);
            debugList.Add(new EventOption(
                "Day + 1",
                DebugSkipDay,
                CATEGORY_DEBUG));
            debugList.Add(new EventOption(
                "EXP + 50",
                DebugGainExp,
                CATEGORY_DEBUG));

            var defaultList = _currentOptions[CATEGORY_DEFAULT] = new List<IEventOption>(5);
            defaultList.Add(new EventOption(
                "Change Equipment",
                OpenEquipment,
                CATEGORY_DEFAULT,
                0));
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

        private void SetupOptions_Calamity()
        {
            var defaultList = _currentOptions[CATEGORY_DEFAULT] = new List<IEventOption>(5);
            defaultList.Add(new EventOption(
                "Face the Calamity",
                GoToCalamity,
                CATEGORY_DEFAULT));
        }

        private void SetupOptions_ChangeEquipment()
        {
            var defaultList = _currentOptions[CATEGORY_DEFAULT] = new List<IEventOption>(5);
            foreach (var optionsKvp in EquipmentSubState.CurrentOptions)
            {
                _currentOptions[optionsKvp.Key] = optionsKvp.Value;
            }

            defaultList.Clear();
            defaultList.Add(new EventOption(
                "Stop Equipping",
                CloseEquipment,
                CATEGORY_DEFAULT));
        }

        private void OpenEquipment()
        {
            EquipmentSubState.Active = true;
            SetupOptions();
        }

        private void CloseEquipment()
        {
            EquipmentSubState.Active = false;
            SetupOptions();
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

        private void GoToCalamity()
        {
            SharedContext.Get<IStateMachine>().ChangeState(
                new CombatSetupState(
                    SharedContext.Get<CombatSettingsGenerator>().CreateFromEnemies(
                        PartyData.Calamity.Party,
                        new[] {PartyData.Calamity})));
        }

        private void DebugSkipDay()
        {
            PartyData.Day += 1;
            SetupOptions();
        }

        private void DebugGainExp()
        {
            Random random = new Random();
            foreach (PlayerCharacter playerCharacter in PartyData.Characters)
            {
                uint level = playerCharacter.Stats[StatType.LVL];
                playerCharacter.Stats.ModifyStat(StatType.EXP, 50);
                uint newLevel = playerCharacter.Stats[StatType.LVL];
                while (level < newLevel)
                {
                    playerCharacter.Stats = playerCharacter.Class.LevelUpStats.Increment(playerCharacter.Stats, random);
                    level++;
                }
            }

            SetupOptions();
        }
    }
}
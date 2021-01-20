using System;
using System.Collections.Generic;
using System.IO;
using Core.CombatSettings;
using Core.Database;
using Core.Etc;
using Core.EventOptions;
using Core.States.BaseClasses;
using Core.Wrappers;
using Newtonsoft.Json;
using SadPumpkin.Util.Context;
using SadPumpkin.Util.StateMachine;
using SadPumpkin.Util.StateMachine.States;

namespace Core.States
{
    /// <summary>
    /// Initial state of the game flow.
    /// <remarks>
    /// - Initialize global utilities and add them to context
    /// - Load JSON databases from disk
    /// - Attempt to load player JSON from disk, otherwise create
    /// - Attempt to load party JSON from disk
    /// - Transition to next state:
    ///   - If party JSON exists, transition to PreGameState
    ///   - Else transition to CreatePartyState
    /// </remarks>
    /// </summary>

    public class StartupState : TDHStateBase
    {
        public const string CATEGORY_CONTINUE = "Continue";
        public const string CATEGORY_DEBUG = "Debug";
        
        private bool _isFullyLoaded = false;

        public override void OnEnter(IState fromState)
        {
            if (!(fromState is NullState))
                throw new ArgumentException($"{nameof(StartupState)} should never be entered from any other state.");
        }

        public override void OnContent()
        {
            SetupOptions();

            // Load JsonSettings
            DataLoadHelper.LoadJsonSettings(SharedContext);

            // Pull PathUtility and begin loading process
            DataLoadHelper.LoadDatabases(SharedContext);

            // Create and insert CombatSettingsGenerator
            CombatSettingsGenerator combatSettingsGenerator = new CombatSettingsGenerator(SharedContext.Get<EnemyGroupWrapperDatabase>());
            SharedContext.Set(combatSettingsGenerator);

            // Load/Create PlayerData
            PlayerDataWrapper playerDataWrapper = LoadPlayerData(SharedContext);

            // Load PartyData
            PartyDataWrapper partyDataWrapper = LoadPartyData(SharedContext, playerDataWrapper);

            _isFullyLoaded = true;
            OptionsChangedSignal?.Fire(this);

            SetupOptions();
        }

        private void SetupOptions()
        {
            // LOADING (NULL) OPTION
            if (!_isFullyLoaded)
            {
                _currentOptions[CATEGORY_CONTINUE] = new List<IEventOption>()
                {
                    new EventOption("Play", () => { }, CATEGORY_CONTINUE, disabled: true)
                };
                return;
            }

            // CONTINUE OPTION
            if (SharedContext.TryGet(out PartyDataWrapper _))
            {
                _currentOptions[CATEGORY_CONTINUE] = new List<IEventOption>()
                {
                    new EventOption("Play", GoToPreGame, CATEGORY_CONTINUE)
                };
            }
            else
            {
                _currentOptions[CATEGORY_CONTINUE] = new List<IEventOption>()
                {
                    new EventOption("Play", GoToCreateParty, CATEGORY_CONTINUE)
                };
            }

            // DEBUG OPTIONS
            _currentOptions[CATEGORY_DEBUG] = new List<IEventOption>(2);
            if (SharedContext.TryGet(out PlayerDataWrapper _))
            {
                _currentOptions[CATEGORY_DEBUG] = new List<IEventOption>()
                {
                    new EventOption("Reset Player", ResetPlayerData, CATEGORY_DEBUG)
                };
            }
            if (SharedContext.TryGet(out PartyDataWrapper _))
            {
                _currentOptions[CATEGORY_DEBUG] = new List<IEventOption>()
                {
                    new EventOption("Retire Party", ResetPartyData, CATEGORY_DEBUG)
                };
            }
        }

        private void GoToPreGame()
        {
            SharedContext.Get<IStateMachine>().ChangeState<PreGameState>();
        }

        private void GoToCreateParty()
        {
            SharedContext.Get<IStateMachine>().ChangeState<CreatePartyState>();
        }

        private void ResetPlayerData()
        {
            SharedContext.Clear<PlayerDataWrapper>();
            SharedContext.Clear<PartyDataWrapper>();

            PathUtility pathUtility = SharedContext.Get<PathUtility>();

            string filePath = pathUtility.GetPlayerDataPath();
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            string folderPath = filePath.Replace(".json", "");
            if (Directory.Exists(folderPath))
            {
                Directory.Delete(folderPath, true);
            }

            OnContent();
        }

        private void ResetPartyData()
        {
            JsonSerializerSettings jsonSettings = SharedContext.Get<JsonSerializerSettings>();
            PathUtility pathUtility = SharedContext.Get<PathUtility>();
            PlayerDataWrapper playerDataWrapper = SharedContext.Get<PlayerDataWrapper>();

            playerDataWrapper.SetActiveParty(0);
            File.WriteAllText(pathUtility.GetPlayerDataPath(), JsonConvert.SerializeObject(playerDataWrapper, jsonSettings));

            SharedContext.Clear<PartyDataWrapper>();
            
            OnContent();
        }

        private static PlayerDataWrapper LoadPlayerData(IContext context)
        {
            PlayerDataWrapper playerDataWrapper = SaveLoadHelper.LoadPlayerData(context) ?? new PlayerDataWrapper();

            context.Set(playerDataWrapper);

            SaveLoadHelper.SavePlayerData(context);

            return playerDataWrapper;
        }

        private static PartyDataWrapper LoadPartyData(IContext context, PlayerDataWrapper playerData)
        {
            PartyDataWrapper partyDataWrapper = SaveLoadHelper.LoadPartyData(context);

            if (partyDataWrapper == null && playerData.ActivePartyId != 0u)
            {
                throw new InvalidDataException($"Error loading Party data for id {playerData.ActivePartyId}");
            }

            context.Set(partyDataWrapper);
            
            return partyDataWrapper;
        }
    }
}
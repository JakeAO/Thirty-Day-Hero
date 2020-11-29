using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Core.CombatSettings;
using Core.Database;
using Core.Etc;
using Core.EventOptions;
using Core.JSON;
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
        private bool _isFullyLoaded = false;

        public override void OnEnter(IState fromState)
        {
            if (!(fromState is NullState))
                throw new ArgumentException($"{nameof(StartupState)} should never be entered from any other state.");
        }

        public override void OnContent()
        {
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
        }

        public override IEnumerable<IEventOption> GetOptions()
        {
            if (!_isFullyLoaded)
            {
                yield return new EventOption(
                    "Continue",
                    () => { },
                    disabled: true);
                yield break;
            }

            if (SharedContext.TryGet(out PartyDataWrapper _))
            {
                yield return new EventOption(
                    "Continue",
                    GoToPreGame);
            }
            else
            {
                yield return new EventOption(
                    "Create Party",
                    GoToCreateParty);
            }

            if (SharedContext.TryGet(out PlayerDataWrapper _))
            {
                yield return new EventOption(
                    "Reset All Data",
                    ResetPlayerData,
                    "DEBUG");
            }

            if (SharedContext.TryGet(out PartyDataWrapper _))
            {
                yield return new EventOption(
                    "Retire Current Party",
                    ResetPartyData,
                    "DEBUG");
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
            File.Delete(pathUtility.GetPlayerDataPath());
            Directory.Delete(pathUtility.GetPlayerDataPath().Replace(".json", ""));

            SharedContext.Get<IStateMachine>().ChangeState<StartupState>();
        }

        private void ResetPartyData()
        {
            JsonSerializerSettings jsonSettings = SharedContext.Get<JsonSerializerSettings>();
            PathUtility pathUtility = SharedContext.Get<PathUtility>();
            PlayerDataWrapper playerDataWrapper = SharedContext.Get<PlayerDataWrapper>();

            playerDataWrapper.SetActiveParty(0);
            File.WriteAllText(pathUtility.GetPlayerDataPath(), JsonConvert.SerializeObject(playerDataWrapper, jsonSettings));

            SharedContext.Clear<PartyDataWrapper>();

            OptionsChangedSignal?.Fire(this);
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
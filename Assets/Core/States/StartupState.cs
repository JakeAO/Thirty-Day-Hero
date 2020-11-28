using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Core.CombatSettings;
using Core.Database;
using Core.Etc;
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

    public class StartupState : IState
    {
        private IContext _context = null;

        public void PerformSetup(IContext context, IState previousState)
        {
            _context = context;

            if (!(previousState is NullState))
                throw new ArgumentException($"{nameof(StartupState)} should never be entered from any other state.");
        }

        public void PerformContent(IContext context)
        {
            // Setup JSON Settings
            JsonSerializerSettings jsonSettings = new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.Auto,
                ContractResolver = new NewtonsoftContractResolver(),
                Converters = FindAllParameterlessJsonConverters(),
                Error = (sender, args) => Console.WriteLine($"[ERROR] {sender} -> {args.ErrorContext} ({args.CurrentObject})")
            };
            context.Set(jsonSettings);

            // Pull PathUtility and begin loading process
            PathUtility pathUtility = context.Get<PathUtility>();
            LoadDatabases(context, pathUtility);

            // Create and insert CombatSettingsGenerator
            CombatSettingsGenerator combatSettingsGenerator = new CombatSettingsGenerator(context.Get<EnemyGroupWrapperDatabase>());
            context.Set(combatSettingsGenerator);

            // Load/Create PlayerData
            PlayerDataWrapper playerDataWrapper = LoadPlayerData(context, pathUtility);

            // Load PartyData
            PartyDataWrapper partyDataWrapper = LoadPartyData(context, pathUtility, playerDataWrapper);
        }

        public void PerformTeardown(IContext context, IState nextState)
        {

        }

        public void Continue()
        {
            // Transition to next state
            if (_context.TryGet(out PartyDataWrapper _))
            {
                // goto PreGameState
                _context.Get<IStateMachine>().ChangeState<PreGameState>();
            }
            else
            {
                // goto CreatePartyState
                _context.Get<IStateMachine>().ChangeState<CreatePartyState>();
            }
        }

        private IList<JsonConverter> FindAllParameterlessJsonConverters()
        {
            return Assembly.GetAssembly(GetType())
                .GetTypes()
                .Where(x =>
                    !x.IsAbstract &&
                    x.IsClass &&
                    typeof(JsonConverter).IsAssignableFrom(x) &&
                    x.GetConstructor(Type.EmptyTypes) != null)
                .Select(x => (JsonConverter) x.GetConstructor(Type.EmptyTypes)?.Invoke(new object[0]))
                .Where(x => x != null)
                .ToList();
        }

        private static void LoadDatabases(IContext context, PathUtility pathUtility)
        {
            JsonSerializerSettings jsonSettings = context.Get<JsonSerializerSettings>();

            // Load Abilities first, since they're the most relied upon
            AbilityDatabase abilityDatabase = AbilityDatabase.LoadFromDisk(pathUtility.AbilityPath, jsonSettings);
            context.Set(abilityDatabase);
            jsonSettings.Converters.Add(new AbilityJsonConverter(abilityDatabase));

            // Then load Items, since they're relied upon fairly often
            ItemDatabase itemDatabase = ItemDatabase.LoadFromDisk(pathUtility.ItemDefinitionPath, jsonSettings);
            WeaponDatabase weaponDatabase = WeaponDatabase.LoadFromDisk(pathUtility.WeaponDefinitionPath, jsonSettings);
            ArmorDatabase armorDatabase = ArmorDatabase.LoadFromDisk(pathUtility.ArmorDefinitionPath, jsonSettings);
            context.Set(itemDatabase);
            context.Set(weaponDatabase);
            context.Set(armorDatabase);
            jsonSettings.Converters.Add(new ItemJsonConverter(itemDatabase));
            jsonSettings.Converters.Add(new WeaponJsonConverter(weaponDatabase));
            jsonSettings.Converters.Add(new ArmorJsonConverter(armorDatabase));

            // Then load Classes, which are rarely relied upon
            PlayerClassDatabase playerClassDatabase = PlayerClassDatabase.LoadFromDisk(pathUtility.PlayerClassPath, jsonSettings);
            EnemyClassDatabase enemyClassDatabase = EnemyClassDatabase.LoadFromDisk(pathUtility.EnemyClassPath, jsonSettings);
            CalamityClassDatabase calamityClassDatabase = CalamityClassDatabase.LoadFromDisk(pathUtility.CalamityClassPath, jsonSettings);
            context.Set(playerClassDatabase);
            context.Set(enemyClassDatabase);
            context.Set(calamityClassDatabase);
            jsonSettings.Converters.Add(new PlayerClassConverter(playerClassDatabase));
            jsonSettings.Converters.Add(new EnemyClassConverter(enemyClassDatabase, calamityClassDatabase));

            // Finally load EnemyGroups, which rely upon Enemies
            EnemyGroupWrapperDatabase enemyGroupWrapperDatabase = EnemyGroupWrapperDatabase.LoadFromDisk(pathUtility.EnemyGroupPath, jsonSettings);
            context.Set(enemyGroupWrapperDatabase);
        }

        private static PlayerDataWrapper LoadPlayerData(IContext context, PathUtility pathUtility)
        {
            JsonSerializerSettings jsonSettings = context.Get<JsonSerializerSettings>();

            PlayerDataWrapper playerDataWrapper;

            Directory.CreateDirectory(pathUtility.SavePath);
            string playerDataPath = pathUtility.GetPlayerDataPath();
            if (File.Exists(playerDataPath))
            {
                string playerDataText = File.ReadAllText(playerDataPath);
                playerDataWrapper = JsonConvert.DeserializeObject<PlayerDataWrapper>(playerDataText, jsonSettings);
            }
            else
            {
                playerDataWrapper = new PlayerDataWrapper();
                File.WriteAllText(playerDataPath, JsonConvert.SerializeObject(playerDataWrapper, jsonSettings));
            }

            if (playerDataWrapper == null)
            {
                throw new InvalidDataException($"Content of PlayerData file could not be read properly: {playerDataPath}");
            }

            context.Set(playerDataWrapper);
            return playerDataWrapper;
        }

        private static PartyDataWrapper LoadPartyData(IContext context, PathUtility pathUtility, PlayerDataWrapper playerData)
        {
            JsonSerializerSettings jsonSettings = context.Get<JsonSerializerSettings>();

            if (playerData.ActivePartyId == 0u)
                return null;

            Directory.CreateDirectory(pathUtility.SavePath);
            string partyDataPath = pathUtility.GetPartyDataPath(playerData.ActivePartyId);
            if (!File.Exists(partyDataPath))
            {
                throw new InvalidDataException($"Active PartyData file does not exist: {partyDataPath}");
            }

            string partyDataText = File.ReadAllText(partyDataPath);
            PartyDataWrapper partyDataWrapper = JsonConvert.DeserializeObject<PartyDataWrapper>(partyDataText, jsonSettings);

            if (partyDataWrapper == null)
            {
                throw new InvalidDataException($"Content of PartyData file could not be read properly: {partyDataPath}");
            }

            context.Set(partyDataWrapper);

            return partyDataWrapper;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Core.CombatSettings;
using Core.Database;
using Core.Etc;
using Core.JSON;
using Newtonsoft.Json;
using SadPumpkin.Util.Context;
using SadPumpkin.Util.StateMachine.States;

namespace Core.States
{
    public class StartupState : IState
    {
        public void PerformSetup(IContext context, IState previousState)
        {
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
            LoadDatabases(context);

            // Create and insert CombatSettingsGenerator
            CombatSettingsGenerator combatSettingsGenerator = new CombatSettingsGenerator(context.Get<EnemyGroupWrapperDatabase>());
            context.Set(combatSettingsGenerator);
        }

        public void PerformTeardown(IContext context, IState nextState)
        {

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

        private void LoadDatabases(IContext context)
        {
            PathUtility pathUtility = context.Get<PathUtility>();
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
    }
}
using System;
using System.Linq;
using System.Reflection;
using Core.Database;
using Core.JSON;
using Newtonsoft.Json;
using SadPumpkin.Util.Context;

namespace Core.Etc
{
    public class DataLoadHelper
    {
        public static void LoadJsonSettings(IContext context)
        {
            // Setup JSON Settings
            JsonSerializerSettings jsonSettings = new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented,
                TypeNameHandling = TypeNameHandling.Auto,
                ContractResolver = new NewtonsoftContractResolver(),
                Error = (sender, args) => Console.WriteLine($"[ERROR] {sender} -> {args.ErrorContext} ({args.CurrentObject})"),
                Converters =
                    Assembly.GetAssembly(typeof(NewtonsoftContractResolver))
                        .GetTypes()
                        .Where(x =>
                            !x.IsAbstract &&
                            x.IsClass &&
                            typeof(JsonConverter).IsAssignableFrom(x) &&
                            x.GetConstructor(Type.EmptyTypes) != null)
                        .Select(x => (JsonConverter) x.GetConstructor(Type.EmptyTypes)?.Invoke(new object[0]))
                        .Where(x => x != null)
                        .ToList()
            };
            context.Set(jsonSettings);
        }

        public static void LoadDatabases(IContext context)
        {
            LoadJsonSettings(context);
            
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
            jsonSettings.Converters.Add(new ItemJsonConverter(itemDatabase, weaponDatabase, armorDatabase));

            // Then load Classes, which are rarely relied upon
            PlayerClassDatabase playerClassDatabase = PlayerClassDatabase.LoadFromDisk(pathUtility.PlayerClassPath, jsonSettings);
            EnemyClassDatabase enemyClassDatabase = EnemyClassDatabase.LoadFromDisk(pathUtility.EnemyClassPath, jsonSettings);
            CalamityClassDatabase calamityClassDatabase = CalamityClassDatabase.LoadFromDisk(pathUtility.CalamityClassPath, jsonSettings);
            context.Set(playerClassDatabase);
            context.Set(enemyClassDatabase);
            context.Set(calamityClassDatabase);
            jsonSettings.Converters.Add(new PlayerClassConverter(playerClassDatabase));
            jsonSettings.Converters.Add(new EnemyClassConverter(enemyClassDatabase));
            jsonSettings.Converters.Add(new CalamityClassConverter( calamityClassDatabase));

            // Finally load EnemyGroups, which rely upon Enemies
            EnemyGroupWrapperDatabase enemyGroupWrapperDatabase = EnemyGroupWrapperDatabase.LoadFromDisk(pathUtility.EnemyGroupPath, jsonSettings);
            context.Set(enemyGroupWrapperDatabase);
        }
    }
}
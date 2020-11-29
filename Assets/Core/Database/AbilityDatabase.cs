using System.Collections.Generic;
using System.IO;
using Core.Abilities;
using Core.Effects;
using Core.Etc;
using Newtonsoft.Json;
using SadPumpkin.Util.CombatEngine.CostCalculators;
using SadPumpkin.Util.CombatEngine.RequirementCalculators;
using SadPumpkin.Util.CombatEngine.TargetCalculators;

namespace Core.Database
{
    public class AbilityDatabase : IDatabase<IAbility>
    {
        public static AbilityDatabase LoadFromDisk(string directoryPath, JsonSerializerSettings jsonSettings)
        {
            List<IAbility> data = new List<IAbility>();

            if (Directory.Exists(directoryPath))
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(directoryPath);
                foreach (FileInfo fileInfo in directoryInfo.EnumerateFiles("*.json", SearchOption.AllDirectories))
                {
                    using (StreamReader streamReader = fileInfo.OpenText())
                    {
                        string allText = streamReader.ReadToEnd();
                        IAbility item = JsonConvert.DeserializeObject<Ability>(allText, jsonSettings);
                        if (item != null)
                        {
                            data.Add(item);
                        }
                    }
                }
            }

            if (data.Count == 0)
            {
                data.AddRange(HackDefinitions.Get());
            }

            return new AbilityDatabase(data);
        }

        private readonly SortedDictionary<uint, IAbility> _allData = new SortedDictionary<uint, IAbility>();

        public AbilityDatabase(IReadOnlyCollection<IAbility> allData)
        {
            foreach (var data in allData)
            {
                _allData[data.Id] = data;
            }
        }

        public IAbility GetRandom()
        {
            return null;
        }

        public IAbility GetSpecific(uint id)
        {
            return _allData.TryGetValue(id, out var result)
                ? result
                : null;
        }

        private static class HackDefinitions
        {
            public static IEnumerable<IAbility> Get()
            {
                yield return
                    new Ability(
                        Constants.ABILITY_ATTACK,
                        "Attack",
                        "Swing that sword.",
                        100,
                        NoRequirements.Instance,
                        NoCost.Instance,
                        SingleEnemyTargetCalculator.Instance,
                        new StatEffect(
                            StatType.HP,
                            10,
                            5));
            }
        }
    }
}
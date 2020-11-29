using System.Collections.Generic;
using System.IO;
using System.Linq;
using Core.Abilities;
using Core.Classes.Enemy;
using Core.Effects;
using Core.Etc;
using Core.Naming;
using Core.StatMap;
using Newtonsoft.Json;
using SadPumpkin.Util.CombatEngine.CostCalculators;
using SadPumpkin.Util.CombatEngine.RequirementCalculators;
using SadPumpkin.Util.CombatEngine.TargetCalculators;
using SadPumpkin.Util.LootTable;

namespace Core.Database
{
    public class EnemyClassDatabase : IDatabase<IEnemyClass>
    {
        public static EnemyClassDatabase LoadFromDisk(string directoryPath, JsonSerializerSettings jsonSettings)
        {
            List<IEnemyClass> data = new List<IEnemyClass>();

            if (Directory.Exists(directoryPath))
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(directoryPath);
                foreach (FileInfo fileInfo in directoryInfo.EnumerateFiles("*.json", SearchOption.AllDirectories))
                {
                    using (StreamReader streamReader = fileInfo.OpenText())
                    {
                        string allText = streamReader.ReadToEnd();
                        IEnemyClass enemyClass = JsonConvert.DeserializeObject<EnemyClass>(allText, jsonSettings);
                        if (enemyClass != null)
                        {
                            data.Add(enemyClass);
                        }
                    }
                }
            }

            if (data.Count == 0)
            {
                data.AddRange(HackDefinitions.Get());
            }

            return new EnemyClassDatabase(data);
        }

        private readonly LootTable _lootTable;
        private readonly SortedDictionary<uint, IEnemyClass> _allData = new SortedDictionary<uint, IEnemyClass>();

        public EnemyClassDatabase(IReadOnlyCollection<IEnemyClass> allData)
        {
            foreach (var data in allData)
            {
                _allData[data.Id] = data;
            }

            _lootTable = new LootTable(1,
                allData
                    .Select(
                        x => new ValueLootEntry<IEnemyClass>(
                            x,
                            Constants.RARITY_WEIGHT[x.Rarity]))
                    .ToArray());
        }

        public IEnemyClass GetRandom()
        {
            foreach (ILootEntry lootEntry in _lootTable.GetLoot())
            {
                if (lootEntry is IValueLootEntry<IEnemyClass> valueEntry)
                {
                    return valueEntry.Value;
                }
            }

            return null;
        }

        public IEnemyClass GetSpecific(uint id)
        {
            return _allData.TryGetValue(id, out var result)
                ? result
                : null;
        }

        private static class HackDefinitions
        {
            public static IEnumerable<IEnemyClass> Get()
            {
                yield return new EnemyClass(
                    Constants.CLASS_MONSTER,
                    "Goblin",
                    "I'm a goblin",
                    new TxtNameGenerator(new string[] {"Doug"}),
                    new Dictionary<DamageType, float>(),
                    new StatMapBuilder(
                        RankPriority.A,
                        RankPriority.C,
                        RankPriority.B,
                        RankPriority.D,
                        RankPriority.F,
                        RankPriority.D),
                    new StatMapIncrementor(
                        RankPriority.A,
                        RankPriority.C,
                        RankPriority.B,
                        RankPriority.D,
                        RankPriority.F,
                        RankPriority.C),
                    new Dictionary<uint, IReadOnlyCollection<IAbility>>()
                    {
                        {
                            0, new IAbility[]
                            {
                                new Ability(
                                    Constants.ABILITY_ATTACK + 1,
                                    "Goblin Punch",
                                    "The goblin does a punch.",
                                    100u,
                                    NoRequirements.Instance,
                                    NoCost.Instance,
                                    SingleEnemyTargetCalculator.Instance,
                                    new StatEffect(
                                        StatType.HP,
                                        5,
                                        5))
                            }
                        }
                    }
                );
            }
        }
    }
}
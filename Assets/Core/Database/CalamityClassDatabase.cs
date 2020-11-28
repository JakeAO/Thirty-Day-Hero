using System.Collections.Generic;
using System.IO;
using System.Linq;
using Core.Abilities;
using Core.Classes.Enemy;
using Core.Costs;
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
    public class CalamityClassDatabase : IDatabase<IEnemyClass>
    {
        public static CalamityClassDatabase LoadFromDisk(string directoryPath, JsonSerializerSettings jsonSettings)
        {
            List<IEnemyClass> data = new List<IEnemyClass>();

            if (Directory.Exists(directoryPath))
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(directoryPath);
                foreach (FileInfo fileInfo in directoryInfo.EnumerateFiles(".json", SearchOption.AllDirectories))
                {
                    using (StreamReader streamReader = fileInfo.OpenText())
                    {
                        string allText = streamReader.ReadToEnd();
                        IEnemyClass enemyClass = JsonConvert.DeserializeObject<IEnemyClass>(allText, jsonSettings);
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

            return new CalamityClassDatabase(data);
        }

        private readonly LootTable _lootTable;
        private readonly SortedDictionary<uint, IEnemyClass> _allData = new SortedDictionary<uint, IEnemyClass>();

        public CalamityClassDatabase(IReadOnlyCollection<IEnemyClass> allData)
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
                    Constants.CLASS_CALAMITY,
                    "Double Goblin",
                    "I'm a goblin plus a goblin",
                    new TxtNameGenerator(new string[] {"Franklin"}),
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
                                    Constants.ABILITY_MONSTER,
                                    "Goblin Punch",
                                    "The goblin does a punch.",
                                    100u,
                                    NoRequirements.Instance,
                                    NoCost.Instance,
                                    SingleEnemyTargetCalculator.Instance,
                                    new DamageEffect(
                                        DamageType.Normal,
                                        source => 5 + source.Stats[StatType.STR] / source.Stats[StatType.LVL],
                                        "[5 + STR/LVL] Normal Damage"))
                            }
                        },
                        {
                            3, new IAbility[]
                            {
                                new Ability(
                                    Constants.ABILITY_MONSTER + 1,
                                    "Goblin Twirl",
                                    "The goblin does a twirl.",
                                    120u,
                                    NoRequirements.Instance,
                                    new StatCost(StatType.STA, 15),
                                    AllEnemyTargetCalculator.Instance,
                                    new DamageEffect(
                                        DamageType.Normal,
                                        source => 10 + source.Stats[StatType.STR] / source.Stats[StatType.LVL],
                                        "[10 + STR/LVL] Normal Damage"))
                            }
                        }
                    }
                );
            }
        }
    }
}
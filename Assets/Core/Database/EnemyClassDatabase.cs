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
    public class EnemyClassDatabase : IDatabase<IEnemyClass>, IRandomItemProvider<IEnemyClass>
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

        public IReadOnlyCollection<IEnemyClass> GetRandom(uint count)
        {
            List<IEnemyClass> results = new List<IEnemyClass>((int) count);

            _lootTable.Count = (int) count;
            foreach (ILootEntry lootEntry in _lootTable.GetLoot())
            {
                if (lootEntry is IValueLootEntry<IEnemyClass> valueEntry)
                {
                    results.Add(valueEntry.Value);
                }
            }

            return results;
        }

        public IEnemyClass GetSpecific(uint id)
        {
            return _allData.TryGetValue(id, out var result)
                ? result
                : null;
        }

        public IEnumerable<IEnemyClass> EnumerateAll()
        {
            return _allData.Values;
        }
    }
}
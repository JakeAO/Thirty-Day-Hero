using System.Collections.Generic;
using System.IO;
using System.Linq;
using Core.Etc;
using Core.Wrappers;
using Newtonsoft.Json;
using SadPumpkin.Util.LootTable;

namespace Core.Database
{
    public class EnemyGroupWrapperDatabase : IDatabase<EnemyGroupWrapper>, IRandomItemProvider<EnemyGroupWrapper>
    {
        public static EnemyGroupWrapperDatabase LoadFromDisk(string directoryPath, JsonSerializerSettings jsonSettings)
        {
            List<EnemyGroupWrapper> data = new List<EnemyGroupWrapper>();

            if (Directory.Exists(directoryPath))
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(directoryPath);
                foreach (FileInfo fileInfo in directoryInfo.EnumerateFiles("*.json", SearchOption.AllDirectories))
                {
                    using (StreamReader streamReader = fileInfo.OpenText())
                    {
                        string allText = streamReader.ReadToEnd();
                        EnemyGroupWrapper enemyGroup = JsonConvert.DeserializeObject<EnemyGroupWrapper>(allText, jsonSettings);
                        if (enemyGroup != null)
                        {
                            data.Add(enemyGroup);
                        }
                    }
                }
            }

            return new EnemyGroupWrapperDatabase(data);
        }

        private readonly LootTable _lootTable;
        private readonly SortedDictionary<uint, EnemyGroupWrapper> _allData = new SortedDictionary<uint, EnemyGroupWrapper>();

        public EnemyGroupWrapperDatabase(IReadOnlyCollection<EnemyGroupWrapper> allData)
        {
            foreach (var data in allData)
            {
                _allData[data.Id] = data;
            }

            _lootTable = new LootTable(1,
                allData
                    .Select(
                        x => new ValueLootEntry<EnemyGroupWrapper>(
                            x,
                            Constants.RARITY_WEIGHT[x.Rarity]))
                    .ToArray());
        }

        public EnemyGroupWrapper GetRandom()
        {
            IReadOnlyCollection<ILootEntry> lootResults = _lootTable.GetLoot();
            foreach (ILootEntry lootEntry in lootResults)
            {
                if (lootEntry is IValueLootEntry<EnemyGroupWrapper> enemyEntry)
                {
                    return enemyEntry.Value;
                }
            }

            return null;
        }

        public IReadOnlyCollection<EnemyGroupWrapper> GetRandom(uint count)
        {
            List<EnemyGroupWrapper> results = new List<EnemyGroupWrapper>((int) count);

            _lootTable.Count = (int) count;
            foreach (ILootEntry lootEntry in _lootTable.GetLoot())
            {
                if (lootEntry is IValueLootEntry<EnemyGroupWrapper> valueEntry)
                {
                    results.Add(valueEntry.Value);
                }
            }

            return results;
        }

        public EnemyGroupWrapper GetSpecific(uint id)
        {
            return _allData.TryGetValue(id, out var result)
                ? result
                : null;
        }

        public IEnumerable<EnemyGroupWrapper> EnumerateAll()
        {
            return _allData.Values;
        }
    }
}
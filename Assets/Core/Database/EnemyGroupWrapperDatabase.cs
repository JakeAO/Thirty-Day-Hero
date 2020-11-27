using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Core.Etc;
using Core.Wrappers;
using Newtonsoft.Json;
using SadPumpkin.Util.LootTable;

namespace Core.Database
{
    public class EnemyGroupWrapperDatabase : IDatabase<EnemyGroupWrapper>
    {
        public static EnemyGroupWrapperDatabase LoadFromDisk(string directoryPath, JsonSerializerSettings jsonSettings)
        {
            if (string.IsNullOrWhiteSpace(directoryPath))
                throw new ArgumentException("Provided directory path was null or empty.");
            if (!Directory.Exists(directoryPath))
                throw new ArgumentException($"Provided directory path does not exist: {directoryPath}");

            List<EnemyGroupWrapper> data = new List<EnemyGroupWrapper>();

            DirectoryInfo directoryInfo = new DirectoryInfo(directoryPath);
            foreach (FileInfo fileInfo in directoryInfo.EnumerateFiles(".json", SearchOption.AllDirectories))
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

        public EnemyGroupWrapper GetSpecific(uint id)
        {
            return _allData.TryGetValue(id, out var result)
                ? result
                : null;
        }
    }
}
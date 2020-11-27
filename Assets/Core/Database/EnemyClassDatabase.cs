using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Core.Classes.Enemy;
using Core.Etc;
using Newtonsoft.Json;
using SadPumpkin.Util.LootTable;

namespace Core.Database
{
    public class EnemyClassDatabase : IDatabase<IEnemyClass>
    {
        public static EnemyClassDatabase LoadFromDisk(string directoryPath, JsonSerializerSettings jsonSettings)
        {
            if (string.IsNullOrWhiteSpace(directoryPath))
                throw new ArgumentException("Provided directory path was null or empty.");
            if (!Directory.Exists(directoryPath))
                throw new ArgumentException($"Provided directory path does not exist: {directoryPath}");

            List<IEnemyClass> data = new List<IEnemyClass>();

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
    }
}
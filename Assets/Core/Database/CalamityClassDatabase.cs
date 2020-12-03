using System.Collections.Generic;
using System.IO;
using System.Linq;
using Core.Classes.Calamity;
using Core.Etc;
using Newtonsoft.Json;
using SadPumpkin.Util.LootTable;

namespace Core.Database
{
    public class CalamityClassDatabase : IDatabase<ICalamityClass>, IRandomItemProvider<ICalamityClass>
    {
        public static CalamityClassDatabase LoadFromDisk(string directoryPath, JsonSerializerSettings jsonSettings)
        {
            List<ICalamityClass> data = new List<ICalamityClass>();

            if (Directory.Exists(directoryPath))
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(directoryPath);
                foreach (FileInfo fileInfo in directoryInfo.EnumerateFiles("*.json", SearchOption.AllDirectories))
                {
                    using (StreamReader streamReader = fileInfo.OpenText())
                    {
                        string allText = streamReader.ReadToEnd();
                        ICalamityClass enemyClass = JsonConvert.DeserializeObject<CalamityClass>(allText, jsonSettings);
                        if (enemyClass != null)
                        {
                            data.Add(enemyClass);
                        }
                    }
                }
            }

            return new CalamityClassDatabase(data);
        }

        private readonly LootTable _lootTable;
        private readonly SortedDictionary<uint, ICalamityClass> _allData = new SortedDictionary<uint, ICalamityClass>();

        public CalamityClassDatabase(IReadOnlyCollection<ICalamityClass> allData)
        {
            foreach (var data in allData)
            {
                _allData[data.Id] = data;
            }

            _lootTable = new LootTable(1,
                allData
                    .Select(
                        x => new ValueLootEntry<ICalamityClass>(
                            x,
                            Constants.RARITY_WEIGHT[x.Rarity]))
                    .ToArray());
        }

        public ICalamityClass GetRandom()
        {
            foreach (ILootEntry lootEntry in _lootTable.GetLoot())
            {
                if (lootEntry is IValueLootEntry<ICalamityClass> valueEntry)
                {
                    return valueEntry.Value;
                }
            }

            return null;
        }

        public IReadOnlyCollection<ICalamityClass> GetRandom(uint count)
        {
            List<ICalamityClass> results = new List<ICalamityClass>((int) count);

            _lootTable.Count = (int) count;
            foreach (ILootEntry lootEntry in _lootTable.GetLoot())
            {
                if (lootEntry is IValueLootEntry<ICalamityClass> valueEntry)
                {
                    results.Add(valueEntry.Value);
                }
            }

            return results;
        }

        public ICalamityClass GetSpecific(uint id)
        {
            return _allData.TryGetValue(id, out var result)
                ? result
                : null;
        }

        public IEnumerable<ICalamityClass> EnumerateAll()
        {
            return _allData.Values;
        }
    }
}
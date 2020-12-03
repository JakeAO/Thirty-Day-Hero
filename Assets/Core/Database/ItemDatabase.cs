using System.Collections.Generic;
using System.IO;
using System.Linq;
using Core.Abilities;
using Core.Etc;
using Core.Items;
using Newtonsoft.Json;
using SadPumpkin.Util.LootTable;

namespace Core.Database
{
    public class ItemDatabase : IDatabase<IItem>, IRandomItemProvider<IItem>
    {
        public static ItemDatabase LoadFromDisk(string directoryPath, JsonSerializerSettings jsonSettings)
        {
            List<IItem> data = new List<IItem>();

            if (Directory.Exists(directoryPath))
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(directoryPath);
                foreach (FileInfo fileInfo in directoryInfo.EnumerateFiles("*.json", SearchOption.AllDirectories))
                {
                    using (StreamReader streamReader = fileInfo.OpenText())
                    {
                        string allText = streamReader.ReadToEnd();
                        IItem item = JsonConvert.DeserializeObject<Item>(allText, jsonSettings);
                        if (item != null)
                        {
                            data.Add(item);
                        }
                    }
                }
            }

            return new ItemDatabase(data);
        }

        private readonly LootTable _lootTable;
        private readonly SortedDictionary<uint, IItem> _allData = new SortedDictionary<uint, IItem>();

        public ItemDatabase(IReadOnlyCollection<IItem> allData)
        {
            foreach (var data in allData)
            {
                _allData[data.Id] = data;
            }

            _lootTable = new LootTable(1,
                allData
                    .Select(
                        x => new ValueLootEntry<IItem>(
                            x,
                            Constants.RARITY_WEIGHT[x.Rarity]))
                    .ToArray());
        }

        public IItem GetRandom()
        {
            IReadOnlyCollection<ILootEntry> lootResults = _lootTable.GetLoot();
            foreach (ILootEntry lootEntry in lootResults)
            {
                if (lootEntry is IValueLootEntry<IItem> valueEntry)
                {
                    return valueEntry.Value;
                }
            }

            return null;
        }

        public IReadOnlyCollection<IItem> GetRandom(uint count)
        {
            List<IItem> results = new List<IItem>((int) count);

            _lootTable.Count = (int) count;
            foreach (ILootEntry lootEntry in _lootTable.GetLoot())
            {
                if (lootEntry is IValueLootEntry<IItem> valueEntry)
                {
                    results.Add(valueEntry.Value);
                }
            }

            return results;
        }

        public IItem GetSpecific(uint id)
        {
            return _allData.TryGetValue(id, out var result)
                ? result
                : null;
        }

        public IEnumerable<IItem> EnumerateAll()
        {
            return _allData.Values;
        }
    }
}
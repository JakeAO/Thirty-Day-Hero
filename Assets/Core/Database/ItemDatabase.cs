using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Core.Etc;
using Core.Items;
using Newtonsoft.Json;
using SadPumpkin.Util.LootTable;

namespace Core.Database
{
    public class ItemDatabase : IDatabase<IItem>
    {
        public static ItemDatabase LoadFromDisk(string directoryPath, JsonSerializerSettings jsonSettings)
        {
            if (string.IsNullOrWhiteSpace(directoryPath))
                throw new ArgumentException("Provided directory path was null or empty.");
            if (!Directory.Exists(directoryPath))
                throw new ArgumentException($"Provided directory path does not exist: {directoryPath}");

            List<IItem> data = new List<IItem>();

            DirectoryInfo directoryInfo = new DirectoryInfo(directoryPath);
            foreach (FileInfo fileInfo in directoryInfo.EnumerateFiles(".json", SearchOption.AllDirectories))
            {
                using (StreamReader streamReader = fileInfo.OpenText())
                {
                    string allText = streamReader.ReadToEnd();
                    IItem item = JsonConvert.DeserializeObject<IItem>(allText, jsonSettings);
                    if (item != null)
                    {
                        data.Add(item);
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

        public IItem GetSpecific(uint id)
        {
            return _allData.TryGetValue(id, out var result)
                ? result
                : null;
        }
    }
}
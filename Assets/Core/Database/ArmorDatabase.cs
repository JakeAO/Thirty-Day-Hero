using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Core.Etc;
using Core.Items.Armors;
using Newtonsoft.Json;
using SadPumpkin.Util.LootTable;

namespace Core.Database
{
    public class ArmorDatabase : IDatabase<IArmor>
    {
        public static ArmorDatabase LoadFromDisk(string directoryPath, JsonSerializerSettings jsonSettings)
        {
            if (string.IsNullOrWhiteSpace(directoryPath))
                throw new ArgumentException("Provided directory path was null or empty.");
            if (!Directory.Exists(directoryPath))
                throw new ArgumentException($"Provided directory path does not exist: {directoryPath}");

            List<IArmor> data = new List<IArmor>();

            DirectoryInfo directoryInfo = new DirectoryInfo(directoryPath);
            foreach (FileInfo fileInfo in directoryInfo.EnumerateFiles(".json", SearchOption.AllDirectories))
            {
                using (StreamReader streamReader = fileInfo.OpenText())
                {
                    string allText = streamReader.ReadToEnd();
                    IArmor armor = JsonConvert.DeserializeObject<IArmor>(allText, jsonSettings);
                    if (armor != null)
                    {
                        data.Add(armor);
                    }
                }
            }

            return new ArmorDatabase(data);
        }

        private readonly LootTable _lootTable;
        private readonly SortedDictionary<uint, IArmor> _allData = new SortedDictionary<uint, IArmor>();

        public ArmorDatabase(IReadOnlyCollection<IArmor> allData)
        {
            foreach (var data in allData)
            {
                _allData[data.Id] = data;
            }

            _lootTable = new LootTable(1,
                allData
                    .Select(
                        x => new ValueLootEntry<IArmor>(
                            x,
                            Constants.RARITY_WEIGHT[x.Rarity]))
                    .ToArray());
        }

        public IArmor GetRandom()
        {
            IReadOnlyCollection<ILootEntry> lootResults = _lootTable.GetLoot();
            foreach (ILootEntry lootEntry in lootResults)
            {
                if (lootEntry is IValueLootEntry<IArmor> valueEntry)
                {
                    return valueEntry.Value;
                }
            }

            return null;
        }

        public IArmor GetSpecific(uint id)
        {
            return _allData.TryGetValue(id, out var result)
                ? result
                : null;
        }
    }
}
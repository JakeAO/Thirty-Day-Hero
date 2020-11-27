using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Core.Classes.Player;
using Core.Etc;
using Newtonsoft.Json;
using SadPumpkin.Util.LootTable;

namespace Core.Database
{
    public class PlayerClassDatabase : IDatabase<IPlayerClass>
    {
        public static PlayerClassDatabase LoadFromDisk(string directoryPath, JsonSerializerSettings jsonSettings)
        {
            if (string.IsNullOrWhiteSpace(directoryPath))
                throw new ArgumentException("Provided directory path was null or empty.");
            if (!Directory.Exists(directoryPath))
                throw new ArgumentException($"Provided directory path does not exist: {directoryPath}");

            List<IPlayerClass> data = new List<IPlayerClass>();

            DirectoryInfo directoryInfo = new DirectoryInfo(directoryPath);
            foreach (FileInfo fileInfo in directoryInfo.EnumerateFiles(".json", SearchOption.AllDirectories))
            {
                using (StreamReader streamReader = fileInfo.OpenText())
                {
                    string allText = streamReader.ReadToEnd();
                    IPlayerClass playerClass = JsonConvert.DeserializeObject<IPlayerClass>(allText, jsonSettings);
                    if (playerClass != null)
                    {
                        data.Add(playerClass);
                    }
                }
            }

            return new PlayerClassDatabase(data);
        }

        private readonly LootTable _lootTable;
        private readonly SortedDictionary<uint, IPlayerClass> _allData = new SortedDictionary<uint, IPlayerClass>();

        public PlayerClassDatabase(IReadOnlyCollection<IPlayerClass> allData)
        {
            foreach (var data in allData)
            {
                _allData[data.Id] = data;
            }

            _lootTable = new LootTable(1,
                allData
                    .Select(
                        x => new ValueLootEntry<IPlayerClass>(
                            x,
                            Constants.RARITY_WEIGHT[x.Rarity]))
                    .ToArray());
        }

        public IPlayerClass GetRandom()
        {
            IReadOnlyCollection<ILootEntry> lootResults = _lootTable.GetLoot();
            foreach (ILootEntry lootEntry in lootResults)
            {
                if (lootEntry is IValueLootEntry<IPlayerClass> valueEntry)
                {
                    return valueEntry.Value;
                }
            }

            return null;
        }

        public IPlayerClass GetSpecific(uint id)
        {
            return _allData.TryGetValue(id, out var result)
                ? result
                : null;
        }
    }
}
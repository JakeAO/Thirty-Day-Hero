using System.Collections.Generic;
using System.IO;
using System.Linq;
using Core.Abilities;
using Core.Classes.Player;
using Core.Effects;
using Core.EquipMap;
using Core.Etc;
using Core.Items;
using Core.Items.Armors;
using Core.Items.Weapons;
using Core.Naming;
using Core.StatMap;
using Newtonsoft.Json;
using SadPumpkin.Util.CombatEngine.CostCalculators;
using SadPumpkin.Util.CombatEngine.RequirementCalculators;
using SadPumpkin.Util.CombatEngine.TargetCalculators;
using SadPumpkin.Util.LootTable;

namespace Core.Database
{
    public class PlayerClassDatabase : IDatabase<IPlayerClass>, IRandomItemProvider<IPlayerClass>
    {
        public static PlayerClassDatabase LoadFromDisk(string directoryPath, JsonSerializerSettings jsonSettings)
        {
            List<IPlayerClass> data = new List<IPlayerClass>();

            if (Directory.Exists(directoryPath))
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(directoryPath);
                foreach (FileInfo fileInfo in directoryInfo.EnumerateFiles("*.json", SearchOption.AllDirectories))
                {
                    using (StreamReader streamReader = fileInfo.OpenText())
                    {
                        string allText = streamReader.ReadToEnd();
                        IPlayerClass playerClass = JsonConvert.DeserializeObject<PlayerClass>(allText, jsonSettings);
                        if (playerClass != null)
                        {
                            data.Add(playerClass);
                        }
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

        public IReadOnlyCollection<IPlayerClass> GetRandom(uint count)
        {
            List<IPlayerClass> results = new List<IPlayerClass>((int) count);

            _lootTable.Count = (int) count;
            foreach (ILootEntry lootEntry in _lootTable.GetLoot())
            {
                if (lootEntry is IValueLootEntry<IPlayerClass> valueEntry)
                {
                    results.Add(valueEntry.Value);
                }
            }

            return results;
        }

        public IPlayerClass GetSpecific(uint id)
        {
            return _allData.TryGetValue(id, out var result)
                ? result
                : null;
        }

        public IEnumerable<IPlayerClass> EnumerateAll()
        {
            return _allData.Values;
        }
    }
}
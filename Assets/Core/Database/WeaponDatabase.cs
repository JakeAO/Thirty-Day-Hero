using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Core.Etc;
using Core.Items.Weapons;
using Newtonsoft.Json;
using SadPumpkin.Util.LootTable;

namespace Core.Database
{
    public class WeaponDatabase : IDatabase<IWeapon>
    {
        public static WeaponDatabase LoadFromDisk(string directoryPath, JsonSerializerSettings jsonSettings)
        {
            if (string.IsNullOrWhiteSpace(directoryPath))
                throw new ArgumentException("Provided directory path was null or empty.");
            if (!Directory.Exists(directoryPath))
                throw new ArgumentException($"Provided directory path does not exist: {directoryPath}");

            List<IWeapon> data = new List<IWeapon>();

            DirectoryInfo directoryInfo = new DirectoryInfo(directoryPath);
            foreach (FileInfo fileInfo in directoryInfo.EnumerateFiles(".json", SearchOption.AllDirectories))
            {
                using (StreamReader streamReader = fileInfo.OpenText())
                {
                    string allText = streamReader.ReadToEnd();
                    IWeapon weapon = JsonConvert.DeserializeObject<IWeapon>(allText, jsonSettings);
                    if (weapon != null)
                    {
                        data.Add(weapon);
                    }
                }
            }

            return new WeaponDatabase(data);
        }

        private readonly LootTable _lootTable;
        private readonly SortedDictionary<uint, IWeapon> _allData = new SortedDictionary<uint, IWeapon>();

        public WeaponDatabase(IReadOnlyCollection<IWeapon> allData)
        {
            foreach (var data in allData)
            {
                _allData[data.Id] = data;
            }

            _lootTable = new LootTable(1,
                allData
                    .Select(
                        x => new ValueLootEntry<IWeapon>(
                            x,
                            Constants.RARITY_WEIGHT[x.Rarity]))
                    .ToArray());
        }

        public IWeapon GetRandom()
        {
            IReadOnlyCollection<ILootEntry> lootResults = _lootTable.GetLoot();
            foreach (ILootEntry lootEntry in lootResults)
            {
                if (lootEntry is IValueLootEntry<IWeapon> valueEntry)
                {
                    return valueEntry.Value;
                }
            }

            return null;
        }

        public IWeapon GetSpecific(uint id)
        {
            return _allData.TryGetValue(id, out var result)
                ? result
                : null;
        }
    }
}
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Core.Abilities;
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
            List<IArmor> data = new List<IArmor>();

            if (Directory.Exists(directoryPath))
            {
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
            }

            if (data.Count == 0)
            {
                data.AddRange(HackDefinitions.Get());
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

        private static class HackDefinitions
        {
            public static IEnumerable<IArmor> Get()
            {
                yield return new Armor(
                    Constants.ARMOR_LIGHT,
                    "Leather Armor",
                    "Simple boiled leather armor.",
                    "Assets/Art/Items/armor/light/chest_08.png",
                    100u,
                    RarityCategory.Common,
                    ArmorType.Light,
                    new Dictionary<DamageType, float>()
                    {
                        {DamageType.Normal, 0.95f},
                        {DamageType.Wind, 0.80f}
                    },
                    new IAbility[0]);
            }
        }
    }
}
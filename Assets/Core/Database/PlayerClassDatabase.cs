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
    public class PlayerClassDatabase : IDatabase<IPlayerClass>
    {
        public static PlayerClassDatabase LoadFromDisk(string directoryPath, JsonSerializerSettings jsonSettings)
        {
            List<IPlayerClass> data = new List<IPlayerClass>();

            if (Directory.Exists(directoryPath))
            {
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
            }

            if (data.Count == 0)
            {
                data.AddRange(HackDefinitions.Get());
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

        private static class HackDefinitions
        {
            public static IEnumerable<IPlayerClass> Get()
            {
                yield return new PlayerClass(
                    Constants.CLASS_PLAYER,
                    "Soldier",
                    "I'm a soldier",
                    new TxtNameGenerator(new string[] {"Gary"}),
                    new StatMapBuilder(
                        RankPriority.A,
                        RankPriority.C,
                        RankPriority.B,
                        RankPriority.D,
                        RankPriority.F,
                        RankPriority.D),
                    new StatMapIncrementor(
                        RankPriority.A,
                        RankPriority.C,
                        RankPriority.B,
                        RankPriority.D,
                        RankPriority.F,
                        RankPriority.C),
                    new Dictionary<uint, IReadOnlyCollection<IAbility>>(),
                    new Dictionary<DamageType, float>(),
                    WeaponType.Sword,
                    ArmorType.Light,
                    new EquipMapBuilder(
                        new Dictionary<IWeapon, RankPriority>()
                        {
                            {
                                new Weapon(
                                    Constants.WEAPON_SWORD,
                                    "Sword",
                                    "I'm a Sword",
                                    "Assets/Art/Items/weapon/sword/sword_01.png",
                                    100u,
                                    RarityCategory.Common,
                                    WeaponType.Sword,
                                    new Ability(
                                        Constants.ABILITY_ATTACK,
                                        "Attack",
                                        "Swing that sword.",
                                        100,
                                        NoRequirements.Instance,
                                        NoCost.Instance,
                                        SingleEnemyTargetCalculator.Instance,
                                        new DamageEffect(
                                            DamageType.Normal,
                                            source => 10 + source.Stats[StatType.STR] / source.Stats[StatType.LVL],
                                            "[10 + STR/LVL] Normal Damage")),
                                    new IAbility[0]),
                                RankPriority.A

                            }
                        },
                        new Dictionary<IArmor, RankPriority>(),
                        new Dictionary<IItem, RankPriority>(),
                        new Dictionary<IItem, RankPriority>())
                );
            }
        }
    }
}
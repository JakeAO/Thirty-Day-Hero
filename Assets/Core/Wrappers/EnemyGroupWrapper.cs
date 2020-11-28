using System.Collections.Generic;
using Core.Classes.Enemy;
using Core.Etc;
using SadPumpkin.Util.CombatEngine;
using SadPumpkin.Util.LootTable;

namespace Core.Wrappers
{
    public class EnemyGroupWrapper : IIdTracked
    {
        public uint Id { get; set; }
        public RarityCategory Rarity { get; set; }
        public LootTable Table { get; set; }

        public EnemyGroupWrapper()
            : this(0u,
                RarityCategory.Invalid)
        {

        }

        public EnemyGroupWrapper(
            uint id,
            RarityCategory rarity,
            params IEnemyClass[] enemyClasses)
        {
            Id = id;
            Rarity = rarity;

            List<ILootEntry> lootEntries = new List<ILootEntry>(enemyClasses.Length);
            foreach (IEnemyClass enemyClass in enemyClasses)
            {
                lootEntries.Add(new ValueLootEntry<IEnemyClass>(
                    enemyClass,
                    Constants.RARITY_WEIGHT[enemyClass.Rarity]));
            }

            Table = new LootTable(3, lootEntries);
        }

        public IReadOnlyCollection<IEnemyClass> GetEnemyClasses(int count)
        {
            List<IEnemyClass> results = new List<IEnemyClass>(count);

            Table.Count = count;
            IReadOnlyCollection<ILootEntry> lootResults = Table.GetLoot();
            foreach (ILootEntry lootEntry in lootResults)
            {
                if (lootEntry is IValueLootEntry<IEnemyClass> enemyEntry)
                {
                    results.Add(enemyEntry.Value);
                }
            }

            return results;
        }
    }
}
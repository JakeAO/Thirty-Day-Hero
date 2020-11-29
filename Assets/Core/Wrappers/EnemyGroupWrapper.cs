using System.Collections.Generic;
using Core.Classes.Enemy;
using Core.Etc;
using SadPumpkin.Util.CombatEngine;
using SadPumpkin.Util.LootTable;

namespace Core.Wrappers
{
    public class EnemyGroupWrapper : IIdTracked, INamed
    {
        public uint Id { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public RarityCategory Rarity { get; set; }
        public List<IEnemyClass> EnemyTypes { get; set; }

        private LootTable _lootTable = null;

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
            EnemyTypes = new List<IEnemyClass>(enemyClasses);
            
            List<ILootEntry> lootEntries = new List<ILootEntry>(enemyClasses.Length);
            foreach (IEnemyClass enemyClass in enemyClasses)
            {
                lootEntries.Add(new ValueLootEntry<IEnemyClass>(
                    enemyClass,
                    Constants.RARITY_WEIGHT[enemyClass.Rarity]));
            }

            _lootTable = new LootTable(3, lootEntries);
        }

        public IReadOnlyCollection<IEnemyClass> GetEnemyClasses(int count)
        {
            List<IEnemyClass> results = new List<IEnemyClass>(count);

            _lootTable.Count = count;
            IReadOnlyCollection<ILootEntry> lootResults = _lootTable.GetLoot();
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
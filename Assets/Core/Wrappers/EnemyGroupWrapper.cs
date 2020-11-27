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
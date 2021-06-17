using System;
using System.Collections.Generic;
using System.Linq;
using Core.Actors;
using Core.Etc;
using Core.Items;
using Core.StatMap;
using Core.Wrappers;

namespace Core.CombatSettings
{
    public class CombatResults
    {
        private static readonly Random RANDOM = new Random();
        
        public bool Success { get; private set; }
        
        public uint ExpReward { get; private set; }
        public uint GoldReward { get; private set; }
        public IReadOnlyCollection<IItem> ItemReward { get; private set; }
        
        public uint ExpPerCharacter { get; private set; }
        public IReadOnlyDictionary<uint, int[]> StatChanges { get; private set; }

        public static CombatResults CreateSuccess(IReadOnlyCollection<ICharacterActor> enemies, PartyDataWrapper party)
        {
            uint GetStatTotal(IReadOnlyCollection<ICharacterActor> actors)
            {
                uint total = 0u;
                foreach (ICharacterActor actor in actors)
                {
                    for (StatType statType = StatType.STR; statType <= StatType.CHA; statType++)
                    {
                        total += actor.Stats[statType];
                    }
                }

                return total;
            }

            uint enemyStatTotal = GetStatTotal(enemies);
            uint playerStatTotal = GetStatTotal(party.Characters);
            float modifier = enemyStatTotal / (float) playerStatTotal;

            float baseExp = enemies.Count * 150f;
            float expReward = baseExp * modifier;

            float goldReward = 250f * modifier;
            party.Gold += (uint) Math.Ceiling(goldReward);

            // TODO this?
            List<IItem> itemRewards = new List<IItem>();
            party.Inventory.AddRange(itemRewards);

            int survivingCharacters = party.Characters.Count(c => c.IsAlive());
            uint expPerCharacter = (uint) Math.Ceiling(expReward / survivingCharacters);
            Dictionary<uint, int[]> statChanges = new Dictionary<uint, int[]>();
            foreach (var pc in party.Characters)
            {
                if (!pc.IsAlive())
                    continue;

                uint currentLevel = pc.Stats[StatType.LVL];
                pc.Stats.ModifyStat(StatType.EXP, (int) expPerCharacter);
                uint newLevel = pc.Stats[StatType.LVL];

                if (currentLevel < newLevel)
                {
                    IStatMap oldStats = pc.Stats.Copy();
                    for (int i = (int) currentLevel; i < newLevel; i++)
                    {
                        pc.Stats = pc.Class.LevelUpStats.Increment(pc.Stats, RANDOM);
                    }

                    int[] statsDiff = GetDiffs(oldStats, pc.Stats);
                    statChanges[pc.Id] = statsDiff;
                }
            }
            
            return new CombatResults()
            {
                Success = true,
                ExpReward = (uint) Math.Ceiling(expReward),
                GoldReward = (uint) Math.Ceiling(goldReward),
                ItemReward = itemRewards,
                ExpPerCharacter = expPerCharacter,
                StatChanges = statChanges
            };
        }

        public static CombatResults CreateFailure()
        {
            return new CombatResults()
            {
                Success = false,
                ExpReward = 0u,
                GoldReward = 0u
            };
        }
        
        private static int[] GetDiffs(IStatMap newMap, IStatMap oldMap)
        {
            int[] diffs = new int[Enum.GetValues(typeof(StatType)).Length];
            foreach (StatType statType in Enum.GetValues(typeof(StatType)))
            {
                diffs[(int) statType] = Math.Max(0, (int) newMap[statType] - (int) oldMap[statType]);
            }

            return diffs;
        }
    }
}
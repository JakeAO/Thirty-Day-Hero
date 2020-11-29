using System;
using System.Collections.Generic;
using Core.Etc;

namespace Core.StatMap
{
    public class StatMapBuilder : IStatMapBuilder
    {
        public uint Total { get; set; }
        public uint MinPerStat { get; set; }
        public Dictionary<StatType, RankPriority> Priorities { get; set; }

        public StatMapBuilder()
            : this(
                RankPriority.C,
                RankPriority.C,
                RankPriority.C,
                RankPriority.C,
                RankPriority.C,
                RankPriority.C,
                Constants.STARTING_STAT_TOTAL,
                Constants.STARTING_STAT_MIN)
        {

        }

        public StatMapBuilder(
            RankPriority strPri,
            RankPriority dexPri,
            RankPriority conPri,
            RankPriority intPri,
            RankPriority magPri,
            RankPriority chaPri,
            uint statTotal = Constants.STARTING_STAT_TOTAL,
            uint minPerStat = Constants.STARTING_STAT_MIN)
        {
            Priorities = new Dictionary<StatType, RankPriority>()
            {
                {StatType.STR, strPri},
                {StatType.DEX, dexPri},
                {StatType.CON, conPri},
                {StatType.INT, intPri},
                {StatType.MAG, magPri},
                {StatType.CHA, chaPri},
            };
            Total = statTotal;
            MinPerStat = minPerStat;
        }

        public IStatMap Generate(Random random)
        {
            Dictionary<StatType, uint> startingStats = new Dictionary<StatType, uint>()
            {
                {StatType.STR, MinPerStat},
                {StatType.DEX, MinPerStat},
                {StatType.CON, MinPerStat},
                {StatType.INT, MinPerStat},
                {StatType.MAG, MinPerStat},
                {StatType.CHA, MinPerStat},
            };

            double totalPriority = 0d;
            foreach (RankPriority statPriority in Priorities.Values)
            {
                totalPriority += Constants.PRIORITY_WEIGHT[statPriority];
            }

            uint totalToAdd = Total - MinPerStat * 6;
            for (int i = 0; i < totalToAdd; i++)
            {
                double randVal = random.NextDouble() * totalPriority;
                foreach (var priorityKvp in Priorities)
                {
                    randVal -= Constants.PRIORITY_WEIGHT[priorityKvp.Value];
                    if (randVal <= 0)
                    {
                        startingStats[priorityKvp.Key] += 1;
                        break;
                    }
                }
            }

            uint startingHp = Constants.STARTING_HP_MULTIPLIER * startingStats[StatType.CON];
            uint maxAtkStat = Math.Max(Math.Max(startingStats[StatType.STR], startingStats[StatType.DEX]), startingStats[StatType.MAG]);
            uint maxAtkStatAvgWithInt = (maxAtkStat + startingStats[StatType.INT]) / 2;
            uint startingSta = Constants.STARTING_STA_MULTIPLIER * maxAtkStatAvgWithInt;

            startingStats[StatType.HP_Max] = startingStats[StatType.HP] = startingHp;
            startingStats[StatType.STA_Max] = startingStats[StatType.STA] = startingSta;

            return new StatMap(startingStats);
        }
    }
}
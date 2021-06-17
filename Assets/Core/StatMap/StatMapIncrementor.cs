using System;
using System.Collections.Generic;
using Core.Etc;

namespace Core.StatMap
{
    public class StatMapIncrementor : IStatMapIncrementor
    {
        public uint Total { get; set; }
        public uint MinPerStat { get; set; }
        public Dictionary<StatType, RankPriority> Priorities { get; set; }

        public StatMapIncrementor()
            : this(
                RankPriority.C,
                RankPriority.C,
                RankPriority.C,
                RankPriority.C,
                RankPriority.C,
                RankPriority.C,
                Constants.LEVEL_STAT_TOTAL,
                Constants.LEVEL_STAT_MIN)
        {

        }

        public StatMapIncrementor(
            RankPriority strPri,
            RankPriority dexPri,
            RankPriority conPri,
            RankPriority intPri,
            RankPriority magPri,
            RankPriority chaPri,
            uint statTotal = Constants.LEVEL_STAT_TOTAL,
            uint minPerStat = Constants.LEVEL_STAT_MIN)
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

        public IStatMap Increment(IStatMap statMap, Random random)
        {
            Dictionary<StatType, uint> statChanges = new Dictionary<StatType, uint>()
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
                        statChanges[priorityKvp.Key] += 1;
                        break;
                    }
                }
            }

            uint hpGain = Constants.LEVEL_HP_MULTIPLIER * statChanges[StatType.CON];
            uint maxAtkStat = Math.Max(Math.Max(statChanges[StatType.STR], statChanges[StatType.DEX]), statChanges[StatType.MAG]);
            uint maxAtkStatAvgWithInt = (maxAtkStat + statChanges[StatType.INT]) / 2;
            uint staGain = Constants.LEVEL_STA_MULTIPLIER * maxAtkStatAvgWithInt;

            statChanges[StatType.HP_Max] = hpGain;
            statChanges[StatType.STA_Max] = staGain;

            Dictionary<StatType, uint> newStats = new Dictionary<StatType, uint>();
            foreach (var enumValue in Enum.GetValues(typeof(StatType)))
            {
                StatType statType = (StatType) enumValue;
                switch (statType)
                {
                    case StatType.Invalid:
                        // nah
                        break;
                    case StatType.HP:
                    case StatType.STA:
                        // gaining a level doesn't magically heal you
                        break;
                    default:
                        newStats[statType] = statMap.GetStat(statType);
                        if (statChanges.TryGetValue(statType, out uint statChange))
                            newStats[statType] += statChange;
                        break;
                }
            }

            return new StatMap(newStats);
        }
    }
}
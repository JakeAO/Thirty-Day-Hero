using System;
using System.Collections.Generic;
using Core.Etc;

namespace Core.StatMap
{
    public class StatMapBuilder : IStatMapBuilder
    {
        private readonly uint _statTotal;
        private readonly uint _statMin;
        private readonly IReadOnlyDictionary<StatType, RankPriority> _statPriorities = null;

        public StatMapBuilder(
            RankPriority strPri,
            RankPriority dexPri,
            RankPriority conPri,
            RankPriority intPri,
            RankPriority magPri,
            RankPriority chaPri,
            uint statTotal = Constants.STARTING_STAT_TOTAL,
            uint statMin = Constants.STARTING_STAT_MIN)
        {
            _statPriorities = new Dictionary<StatType, RankPriority>()
            {
                {StatType.STR, strPri},
                {StatType.DEX, dexPri},
                {StatType.CON, conPri},
                {StatType.INT, intPri},
                {StatType.MAG, magPri},
                {StatType.CHA, chaPri},
            };
            _statTotal = statTotal - statMin * 6;
            _statMin = statMin;
        }

        public IStatMap Generate(Random random)
        {
            Dictionary<StatType, uint> startingStats = new Dictionary<StatType, uint>()
            {
                {StatType.STR, _statMin},
                {StatType.DEX, _statMin},
                {StatType.CON, _statMin},
                {StatType.INT, _statMin},
                {StatType.MAG, _statMin},
                {StatType.CHA, _statMin},
            };

            double totalPriority = 0d;
            foreach (RankPriority statPriority in _statPriorities.Values)
            {
                totalPriority += Constants.PRIORITY_WEIGHT[statPriority];
            }

            for (int i = 0; i < _statTotal; i++)
            {
                double randVal = random.NextDouble() * totalPriority;
                foreach (var priorityKvp in _statPriorities)
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
﻿using Core.Etc;

namespace Core.StatMap
{
    public class NullStatMap : IStatMap
    {
        public static readonly NullStatMap Instance = new NullStatMap();

        private NullStatMap()
        {
        }
        
        public uint this[StatType statType] => 1u;
        public uint GetStat(StatType statType) => 1u;

        public void ModifyStat(StatType statType, int change)
        {
        }

        public IStatMap Copy()
        {
            return Instance;
        }
    }
}
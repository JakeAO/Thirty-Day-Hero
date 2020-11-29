using System;
using System.Collections.Generic;

namespace Core.Etc
{
    public static class Constants
    {
        public static readonly IReadOnlyDictionary<RankPriority, float> PRIORITY_WEIGHT =
            new Dictionary<RankPriority, float>()
            {
                {RankPriority.Invalid, 0f},
                {RankPriority.A, 9f},
                {RankPriority.B, 7f},
                {RankPriority.C, 6f},
                {RankPriority.D, 4.5f},
                {RankPriority.F, 3f},
            };

        public static readonly IReadOnlyDictionary<RarityCategory, float> RARITY_WEIGHT =
            new Dictionary<RarityCategory, float>()
            {
                {RarityCategory.Invalid, 0f},
                {RarityCategory.Common, 100f},
                {RarityCategory.Uncommon, 50f},
                {RarityCategory.Rare, 25f},
                {RarityCategory.Legendary, 10f}
            };

        public const uint BASE_REROLL_RANK_BOUNDARY = 10;
        public const uint REROLL_RANK_BOUNDARY_GROWTH = 5;

        public const uint STARTING_STAT_TOTAL = 30;
        public const uint STARTING_STAT_MIN = 3;

        public const uint STARTING_HP_MULTIPLIER = 5;
        public const uint STARTING_STA_MULTIPLIER = 5;

        public const uint LEVEL_STAT_TOTAL = 7;
        public const uint LEVEL_STAT_MIN = 0;

        public const uint LEVEL_HP_MULTIPLIER = 3;
        public const uint LEVEL_STA_MULTIPLIER = 3;

        public static uint MaxStatAtLevel(uint level) => (uint) Math.Round(STARTING_STAT_MIN + level * LEVEL_STAT_TOTAL * 0.7f);
        
        public const uint PARTY_SIZE_MIN = 1;
        public const uint PARTY_SIZE_MAX = 3;
        public const uint CREATE_PARTY_POOL_SIZE = 5;

        public const uint CALAMITY_LEVEL = 10u;
        public const uint DAYS_TO_PREPARE = 30u;
        
        public const uint ACTION = 10000;
        public const uint ACTION_WAIT = 19999;
    }
}
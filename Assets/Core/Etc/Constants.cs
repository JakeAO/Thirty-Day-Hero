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

        public const uint PARTY_SIZE_MIN = 1;
        public const uint PARTY_SIZE_MAX = 3;
        public const uint CREATE_PARTY_POOL_SIZE = 5;

        public const uint CALAMITY_LEVEL = 20u;
        public const uint DAYS_TO_PREPARE = 30u;
        
        public const uint ACTION = 10000;
        public const uint ACTION_WAIT = 19999;
        
        public const uint WEAPON_SWORD = 60000;
        public const uint WEAPON_GREATSWORD = 61000;
        public const uint WEAPON_AXE = 62000;
        public const uint WEAPON_GREATEAXE = 63000;
        public const uint WEAPON_SPEAR = 64000;
        public const uint WEAPON_STAFF = 65000;
        public const uint WEAPON_ROD = 66000;
        public const uint WEAPON_BOW = 67000;
        public const uint WEAPON_FIST = 68000;

        public const uint ARMOR_LIGHT = 70000;
        public const uint ARMOR_MEDIUM = 71000;
        public const uint ARMOR_HEAVY = 72000;

        public const uint ITEM_CONSUMABLE = 80000;
        public const uint ITEM_TRINKET = 81000;
        public const uint ITEM_LOOT = 82000;

        public const uint CLASS_PLAYER = 90000;
        public const uint CLASS_CALAMITY = 91000;
        public const uint CLASS_MONSTER = 92000;

        public const uint ENEMY_GROUP = 95000;

        public const uint ABILITY_ATTACK = 100000;
        public const uint ABILITY_SKILL = 101000;
        public const uint ABILITY_SPELL = 102000;
        public const uint ABILITY_MONSTER = 103000;
        public const uint ABILITY_ITEM = 104000;
    }
}
using System;
using Core.Actors.Calamity;
using Core.Actors.Enemy;
using Core.Actors.Player;
using Core.Classes.Calamity;
using Core.Classes.Enemy;
using Core.Classes.Player;
using Core.EquipMap;
using Core.Etc;
using Core.StatMap;

namespace Core.Actors
{
    public static class ActorUtil
    {
        private static readonly Random RANDOM = new Random();

        public static EnemyCharacter CreateEnemy(
            uint partyId,
            IEnemyClass enemyClass,
            uint level = 1)
        {
            IStatMap stats = enemyClass.StartingStats.Generate(RANDOM);
            for (uint i = 1; i < level; i++)
            {
                stats.ModifyStat(StatType.LVL, 1);
                stats = enemyClass.LevelUpStats.Increment(stats, RANDOM);
            }

            return new EnemyCharacter(
                (uint) Guid.NewGuid().GetHashCode(),
                partyId,
                enemyClass.NameGenerator.GetName(),
                enemyClass,
                stats);
        }

        public static CalamityCharacter CreateCalamity(
            uint partyId,
            ICalamityClass calamityClass,
            uint level = 1)
        {
            IStatMap stats = calamityClass.StartingStats.Generate(RANDOM);
            for (uint i = 1; i < level; i++)
            {
                stats.ModifyStat(StatType.LVL, 1);
                stats = calamityClass.LevelUpStats.Increment(stats, RANDOM);
            }

            return new CalamityCharacter(
                (uint) Guid.NewGuid().GetHashCode(),
                partyId,
                calamityClass.NameGenerator.GetName(),
                calamityClass,
                stats);
        }

        public static PlayerCharacter CreatePlayer(
            uint partyId,
            IPlayerClass playerClass,
            uint level = 1)
        {
            IStatMap stats = playerClass.StartingStats.Generate(RANDOM);
            for (uint i = 1; i < level; i++)
            {
                stats.ModifyStat(StatType.LVL, 1);
                stats = playerClass.LevelUpStats.Increment(stats, RANDOM);
            }

            IEquipMap equipment = playerClass.StartingEquipment.Generate(RANDOM);

            return new PlayerCharacter(
                (uint) Guid.NewGuid().GetHashCode(),
                partyId,
                playerClass.NameGenerator.GetName(),
                playerClass,
                stats,
                equipment);
        }
    }
}
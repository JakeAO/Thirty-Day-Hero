using System;
using System.Collections.Generic;
using Core.Actors;
using Core.Actors.Enemy;
using Core.Classes.Enemy;
using Core.Database;
using Core.Etc;
using Core.Wrappers;
using SadPumpkin.Util.CombatEngine.CharacterControllers;

namespace Core.CombatSettings
{
    public class CombatSettingsGenerator
    {
        private readonly Random _random = new Random();

        private readonly EnemyGroupWrapperDatabase _enemyGroupDatabase = null;

        public CombatSettingsGenerator(EnemyGroupWrapperDatabase enemyGroupDatabase)
        {
            _enemyGroupDatabase = enemyGroupDatabase;
        }

        public CombatSettings CreateFromEnemies(IReadOnlyCollection<IEnemyCharacterActor> enemies)
        {
            return new CombatSettings(enemies, new RandomCharacterController());
        }

        public CombatSettings CreateFromEnemyTypes(IReadOnlyCollection<IEnemyClass> enemyTypes, CombatDifficulty difficulty, PartyDataWrapper playerParty)
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

            uint GetStatTarget(uint totalStats, CombatDifficulty diff)
            {
                switch (diff)
                {
                    case CombatDifficulty.Hard:
                        return (uint) (totalStats * 1f);
                    case CombatDifficulty.Normal:
                        return (uint) (totalStats * 0.75f);
                    case CombatDifficulty.Easy:
                    default:
                        return (uint) (totalStats * 0.5f);
                }
            }

            uint partyId = (uint) Guid.NewGuid().GetHashCode();

            // Generate Enemies
            List<EnemyCharacter> enemies = new List<EnemyCharacter>(enemyTypes.Count);
            foreach (IEnemyClass enemyDefinition in enemyTypes)
            {
                var newEnemy = ActorUtil.CreateEnemy(
                    partyId,
                    enemyDefinition);
                enemies.Add(newEnemy);
            }

            // Scale Enemies for Difficulty
            uint totalPartyStats = GetStatTotal(playerParty.Characters);
            uint targetTotalEnemyStats = GetStatTarget(totalPartyStats, difficulty);
            uint totalEnemyStats = GetStatTotal(enemies);
            while (totalEnemyStats < targetTotalEnemyStats)
            {
                EnemyCharacter enemy = enemies[_random.Next(enemies.Count)];
                enemy.Stats = enemy.Class.LevelUpStats.Increment(enemy.Stats, _random);
                enemy.Stats.ModifyStat(StatType.LVL, 1);

                totalEnemyStats = GetStatTotal(enemies);
            }

            return CreateFromEnemies(enemies);
        }

        public CombatSettings CreateFromEnemyGroup(EnemyGroupWrapper enemyGroup, CombatDifficulty difficulty, PartyDataWrapper playerParty)
        {
            int enemyCount = 1;
            switch (difficulty)
            {
                case CombatDifficulty.Easy:
                    enemyCount = Math.Max(1, _random.Next(1, playerParty.Characters.Count));
                    break;
                case CombatDifficulty.Normal:
                    enemyCount = Math.Max(2, _random.Next(playerParty.Characters.Count - 1, playerParty.Characters.Count + 1));
                    break;
                case CombatDifficulty.Hard:
                    enemyCount = Math.Max(2, _random.Next(playerParty.Characters.Count - 1, playerParty.Characters.Count + 2));
                    break;
            }

            IReadOnlyCollection<IEnemyClass> enemyTypes = enemyGroup.GetEnemyClasses(enemyCount);

            return CreateFromEnemyTypes(enemyTypes, difficulty, playerParty);
        }

        public CombatSettings CreateFromDifficulty(CombatDifficulty difficulty, PartyDataWrapper playerParty)
        {
            return CreateFromEnemyGroup(
                _enemyGroupDatabase.GetRandom(),
                difficulty,
                playerParty);
        }
    }
}
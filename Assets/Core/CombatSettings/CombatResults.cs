﻿using System;
using System.Collections.Generic;
using Core.Actors;
using Core.Actors.Enemy;
using Core.Etc;
using Core.Wrappers;

namespace Core.CombatSettings
{
    public class CombatResults
    {
        public bool Success { get; private set; }
        public uint ExpReward { get; private set; }
        public uint GoldReward { get; private set; }

        public static CombatResults CreateSuccess(IReadOnlyCollection<IEnemyCharacterActor> enemies, PartyDataWrapper party)
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

            return new CombatResults()
            {
                Success = true,
                ExpReward = (uint) Math.Ceiling(expReward),
                GoldReward = (uint) Math.Ceiling(goldReward)
            };
        }

        public static CombatResults CreateFailure()
        {
            return new CombatResults()
            {
                Success = false,
                ExpReward = 0u
            };
        }
    }
}
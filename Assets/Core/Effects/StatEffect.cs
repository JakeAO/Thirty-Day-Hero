using System;
using System.Collections.Generic;
using Core.Etc;
using Core.StatMap;
using SadPumpkin.Util.CombatEngine.Actor;
using SadPumpkin.Util.CombatEngine.EffectCalculators;

using ICharacterActor = Core.Actors.ICharacterActor;

namespace Core.Effects
{
    public class StatEffect : IEffectCalc
    {
        private static readonly Random RANDOM = new Random();

        public StatType Stat { get; set; }
        public DamageType EffectType { get; set; }
        public int BaseAmount { get; set; }
        public int VariableAmount { get; set; }
        public StatType ScalingStat { get; set; }
        public RankPriority ScalingRank { get; set; }

        public string Description
        {
            get
            {
                if (VariableAmount == 0)
                {
                    uint absBase = (uint) Math.Abs(BaseAmount);
                    char sign = BaseAmount > 0 ? '+' : BaseAmount < 0 ? '-' : (char) 0;
                    return $"{sign}{absBase} {Stat}";
                }

                int minRaw = Math.Min(BaseAmount, BaseAmount + VariableAmount);
                int maxRaw = Math.Max(BaseAmount, BaseAmount + VariableAmount);
                uint absMin = (uint) Math.Abs(minRaw);
                uint absMax = (uint) Math.Abs(maxRaw);
                char minSign = minRaw >= 0 ? '+' : '-';
                char maxSign = maxRaw >= 0 ? '+' : '-';
                
                string desc;
                if (minSign != maxSign)
                {
                    desc = $"[{minSign}{absMin}-{maxSign}{absMax}] {Stat}";
                }
                else if (minRaw >= 0)
                {
                    desc = $"{minSign}[{absMin}-{absMax}] {Stat}";
                }
                else
                {
                    desc = $"{minSign}[{absMax}-{absMin}] {Stat}";
                }

                if (ScalingStat != StatType.Invalid &&
                    ScalingRank != RankPriority.Invalid)
                {
                    desc += $" ({ScalingStat}: {ScalingRank})";
                }

                if (EffectType != DamageType.Invalid)
                {
                    desc += $" ({EffectType})";
                }

                return desc;
            }
        }

        public StatEffect()
            : this(
                StatType.Invalid,
                0,
                0,
                DamageType.Invalid,
                StatType.Invalid,
                RankPriority.Invalid)
        {
        }

        public StatEffect(
            StatType stat,
            int baseAmount,
            int variableAmount = 0,
            DamageType effectType = DamageType.Invalid,
            StatType scalingStat = StatType.Invalid,
            RankPriority scalingRank = RankPriority.Invalid)
        {
            Stat = stat;
            BaseAmount = baseAmount;
            VariableAmount = variableAmount;
            EffectType = effectType;
            ScalingStat = scalingStat;
            ScalingRank = scalingRank;
        }

        public void Apply(IInitiativeActor sourceEntity, IReadOnlyCollection<ITargetableActor> targetActors)
        {
            if (sourceEntity is ICharacterActor sourceCharacter)
            {
                foreach (ITargetableActor targetActor in targetActors)
                {
                    if (targetActor is ICharacterActor targetCharacter)
                    {
                        float effectOnTarget = GetAmount(sourceCharacter.Stats);
                        if (effectOnTarget < 0 &&
                            EffectType != DamageType.Invalid)
                        {
                            effectOnTarget = targetCharacter.GetReducedDamage(effectOnTarget, EffectType);
                        }

                        targetCharacter.Stats.ModifyStat(Stat, (int) Math.Round(effectOnTarget));
                    }
                }
            }
        }

        public int GetAmount(IStatMap sourceStats)
        {
            bool reroll = false;
            uint rerollBoundary = Constants.BASE_REROLL_RANK_BOUNDARY; // Default: 10
            uint rerollGrowth = Constants.REROLL_RANK_BOUNDARY_GROWTH; // Default: 5

            bool highMeansGood = BaseAmount > 0;

            int? result = null;
            do
            {
                int baseValue = BaseAmount;
                int flexValue = VariableAmount;
                int finalValue = (int) Math.Round(baseValue + flexValue * RANDOM.NextDouble());

                if (!result.HasValue)
                {
                    result = finalValue;
                }
                else if (highMeansGood)
                {
                    if (result.Value < finalValue)
                        result = finalValue;
                }
                else
                {
                    if (result.Value > finalValue)
                        result = finalValue;
                }

                double rerollHit = RANDOM.NextDouble() * rerollBoundary;
                rerollBoundary += rerollGrowth;

                double statMultiplier = ScalingStat != StatType.Invalid
                    ? sourceStats[ScalingStat] / (double) Constants.MaxStatAtLevel(sourceStats[StatType.LVL])
                    : 0d;
                double rankMultiplier = ScalingRank != RankPriority.Invalid
                    ? Constants.PRIORITY_WEIGHT[ScalingRank]
                    : 0d;
                double rerollMustBeBelow = 1 + statMultiplier * rankMultiplier; // +1 so every roll has a smol chance

                reroll = rerollHit < rerollMustBeBelow;

            } while (reroll);

            return result.Value;
        }
    }
}
using System;
using System.Collections.Generic;
using Core.Etc;
using Core.StatMap;
using SadPumpkin.Util.CombatEngine.Actor;
using SadPumpkin.Util.CombatEngine.EffectCalculators;
using ICharacterActor = Core.Actors.ICharacterActor;

namespace Core.Effects
{
    public class RawDamageEffect : IEffectCalc
    {
        private static readonly Random RANDOM = new Random();

        public Func<ICharacterActor, uint> MinCalculation { get; }
        public Func<ICharacterActor, uint> MaxCalculation { get; }

        public string Description { get; }

        public RawDamageEffect(Func<ICharacterActor, uint> calculation, string description)
        {
            MinCalculation = MaxCalculation = calculation;
            Description = description;
        }

        public RawDamageEffect(Func<ICharacterActor, uint> minCalculation, Func<ICharacterActor, uint> maxCalculation, string description)
        {
            MinCalculation = minCalculation;
            MaxCalculation = maxCalculation;
            Description = description;
        }

        public void Apply(IInitiativeActor sourceEntity, IReadOnlyCollection<ITargetableActor> targetActors)
        {
            if (sourceEntity is ICharacterActor sourceCharacter)
            {
                int damage = (int) -GetRawAmount(sourceCharacter);
                foreach (ITargetableActor targetActor in targetActors)
                {
                    if (targetActor is ICharacterActor characterActor)
                    {
                        characterActor.Stats.ModifyStat(StatType.HP, damage);
                    }
                }
            }
        }

        private uint GetRawAmount(ICharacterActor sourceCharacter)
        {
            if (MinCalculation == MaxCalculation)
                return MinCalculation(sourceCharacter);

            uint min = MinCalculation(sourceCharacter);
            uint max = MaxCalculation(sourceCharacter) + 1u; // Random.Next max is exclusive, so add 1.
            return (uint) RANDOM.Next((int) min, (int) max);
        }
    }
}
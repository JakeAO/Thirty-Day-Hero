using System;
using System.Collections.Generic;
using Core.Etc;
using Core.StatMap;
using SadPumpkin.Util.CombatEngine.Actor;
using SadPumpkin.Util.CombatEngine.EffectCalculators;
using ICharacterActor = Core.Actors.ICharacterActor;

namespace Core.Effects
{
    public class StaminaEffect : IEffectCalc
    {
        private static readonly Random RANDOM = new Random();

        public Func<ICharacterActor, uint> MinCalculation { get; }
        public Func<ICharacterActor, uint> MaxCalculation { get; }

        public string Description { get; }

        public StaminaEffect(Func<ICharacterActor, uint> calculation, string description)
        {
            MinCalculation = MaxCalculation = calculation;
            Description = description;
        }

        public StaminaEffect(Func<ICharacterActor, uint> minCalculation, Func<ICharacterActor, uint> maxCalculation, string description)
        {
            MinCalculation = minCalculation;
            MaxCalculation = maxCalculation;
            Description = description;
        }

        public void Apply(IInitiativeActor sourceEntity, IReadOnlyCollection<ITargetableActor> targetActors)
        {
            if (sourceEntity is ICharacterActor sourceCharacter)
            {
                int stamina = (int) -GetRawAmount(sourceCharacter);
                foreach (ITargetableActor targetActor in targetActors)
                {
                    if (targetActor is ICharacterActor targetCharacter)
                    {
                        targetCharacter.Stats.ModifyStat(StatType.STA, stamina);
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
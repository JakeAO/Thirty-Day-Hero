using Core.Etc;
using SadPumpkin.Util.CombatEngine;
using SadPumpkin.Util.CombatEngine.Actor;
using SadPumpkin.Util.CombatEngine.CostCalculators;

using ICharacterActor = Core.Actors.ICharacterActor;

namespace Core.Costs
{
    public class StatCost : ICostCalc
    {
        public StatType Type { get; set; }
        public uint Amount { get; set; }

        public StatCost()
            : this(StatType.Invalid, 0u)
        {
            
        }

        public StatCost(StatType type, uint amount)
        {
            Type = type;
            Amount = amount;
        }

        public bool CanAfford(IInitiativeActor entity, IIdTracked actionSource)
        {
            return entity is ICharacterActor character && character.Stats.GetStat(Type) >= Amount;
        }

        public void Pay(IInitiativeActor entity, IIdTracked actionSource)
        {
            if (entity is ICharacterActor character)
                character.Stats.ModifyStat(Type, (int) -Amount);
        }

        public string Description()
        {
            return $"{Amount} {Type}";
        }
    }
}
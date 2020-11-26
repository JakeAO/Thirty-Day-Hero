using SadPumpkin.Util.CombatEngine.Abilities;
using SadPumpkin.Util.CombatEngine.CostCalculators;
using SadPumpkin.Util.CombatEngine.EffectCalculators;
using SadPumpkin.Util.CombatEngine.RequirementCalculators;
using SadPumpkin.Util.CombatEngine.TargetCalculators;

namespace Core.Abilities
{
    public class Ability : IAbility
    {
        public uint Id { get; }
        public string Name { get; }
        public string Desc { get; }
        public uint Speed { get; }
        public IRequirementCalc Requirements { get; }
        public ICostCalc Cost { get; }
        public ITargetCalc Target { get; }
        public IEffectCalc Effect { get; }

        public Ability(
            uint id,
            string name, string desc,
            uint speed,
            IRequirementCalc requirements,
            ICostCalc cost,
            ITargetCalc target,
            IEffectCalc effect)
        {
            Id = id;
            Name = name;
            Desc = desc;
            Speed = speed;
            Requirements = requirements;
            Cost = cost;
            Target = target;
            Effect = effect;
        }
    }
}
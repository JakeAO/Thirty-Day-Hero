using SadPumpkin.Util.CombatEngine.CostCalculators;
using SadPumpkin.Util.CombatEngine.EffectCalculators;
using SadPumpkin.Util.CombatEngine.RequirementCalculators;
using SadPumpkin.Util.CombatEngine.TargetCalculators;

namespace Core.Abilities
{
    public class Ability : IAbility
    {
        public uint Id { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public uint Speed { get; set; }
        public IRequirementCalc Requirements { get; set; }
        public ICostCalc Cost { get; set; }
        public ITargetCalc Target { get; set; }
        public IEffectCalc Effect { get; set; }

        public Ability()
            : this(
                0u,
                string.Empty,
                string.Empty,
                0u,
                NoRequirements.Instance,
                NoCost.Instance,
                SelfTargetCalculator.Instance,
                NoEffect.Instance)
        {

        }

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
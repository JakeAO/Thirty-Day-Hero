using Core.Etc;
using SadPumpkin.Util.CombatEngine;
using SadPumpkin.Util.CombatEngine.CostCalculators;
using SadPumpkin.Util.CombatEngine.EffectCalculators;
using SadPumpkin.Util.CombatEngine.RequirementCalculators;
using SadPumpkin.Util.CombatEngine.TargetCalculators;

namespace Core.Abilities
{
    public interface IAbility : IIdTracked, INamed
    {
        uint Speed { get; }
        IRequirementCalc Requirements { get; }
        ICostCalc Cost { get; }
        ITargetCalc Target { get; }
        IEffectCalc Effect { get; }
    }
}
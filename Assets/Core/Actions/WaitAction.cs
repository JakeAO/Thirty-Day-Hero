using System.Collections.Generic;
using Core.Etc;
using SadPumpkin.Util.CombatEngine;
using SadPumpkin.Util.CombatEngine.Action;
using SadPumpkin.Util.CombatEngine.Actor;
using SadPumpkin.Util.CombatEngine.CostCalculators;
using SadPumpkin.Util.CombatEngine.EffectCalculators;

namespace Core.Actions
{
    public class WaitAction : IAction, INamed, IIdTracked
    {
        private static readonly IReadOnlyCollection<ITargetableActor> NoTargets = new ITargetableActor[0];

        public uint Id { get; } = Constants.ACTION_WAIT;
        public string Name { get; } = "Wait";
        public string Desc { get; } = "Take no action.";
        public bool Available { get; } = true;
        public IInitiativeActor Source { get; }
        public IReadOnlyCollection<ITargetableActor> Targets { get; } = NoTargets;
        
        public uint Speed { get; } = 50u;
        public ICostCalc Cost { get; } = NoCost.Instance;
        public IEffectCalc Effect { get; } = NoEffect.Instance;

        public IIdTracked ActionSource { get; } = null;
        public IIdTracked ActionProvider { get; } = null;

        public WaitAction(IInitiativeActor actor)
        {
            Source = actor;
            ActionSource = this;
        }
    }
}
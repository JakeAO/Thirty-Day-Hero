using System.Collections.Generic;
using Core.Abilities;
using SadPumpkin.Util.CombatEngine;
using SadPumpkin.Util.CombatEngine.Action;
using SadPumpkin.Util.CombatEngine.Actor;
using SadPumpkin.Util.CombatEngine.CostCalculators;
using SadPumpkin.Util.CombatEngine.EffectCalculators;

namespace Core.Actions
{
    public class Action : IAction
    {
        public uint Id { get; set; }
        public bool Available { get; set; }
        public IInitiativeActor Source { get; set; }
        public IReadOnlyCollection<ITargetableActor> Targets { get; set; }
        
        public uint Speed { get; set; }
        public ICostCalc Cost { get; set; }
        public IEffectCalc Effect { get; set; }
        public IIdTracked ActionSource { get; set; }
        public IIdTracked ActionProvider { get; set; }

        public Action(
            uint id,
            bool available,
            IAbility ability,
            IInitiativeActor source,
            IReadOnlyCollection<ITargetableActor> targets,
            IIdTracked actionSource,
            IIdTracked actionProvider)
        {
            Id = id;
            Available = available;
            Source = source;
            Targets = targets;

            Speed = ability.Speed;
            Cost = ability.Cost;
            Effect = ability.Effect;
            ActionSource = actionSource;
            ActionProvider = actionProvider;
        }
    }
}
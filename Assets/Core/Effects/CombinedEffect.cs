using System.Collections.Generic;
using System.Linq;
using SadPumpkin.Util.CombatEngine.Actor;
using SadPumpkin.Util.CombatEngine.EffectCalculators;

namespace Core.Effects
{
    public class CombinedEffect : IEffectCalc
    {
        public IReadOnlyList<IEffectCalc> ChildEffects { get; }
        
        public string Description { get; }

        public CombinedEffect(params IEffectCalc[] childEffects)
        {
            ChildEffects = childEffects;
            Description = string.Join(", ", childEffects.Select(x => x.Description));
        }

        public void Apply(IInitiativeActor sourceEntity, IReadOnlyCollection<ITargetableActor> targetActors)
        {
            foreach (IEffectCalc childEffect in ChildEffects)
            {
                childEffect.Apply(sourceEntity, targetActors);
            }
        }
    }
}
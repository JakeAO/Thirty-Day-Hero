using System.Collections.Generic;
using System.Linq;
using SadPumpkin.Util.CombatEngine.Actor;
using SadPumpkin.Util.CombatEngine.EffectCalculators;

namespace Core.Effects
{
    public class CombinedEffect : IEffectCalc
    {
        public List<IEffectCalc> ChildEffects { get; set; }

        public string Description => string.Join(", ", ChildEffects.Select(x => x.Description));

        public CombinedEffect()
        {
            ChildEffects = new List<IEffectCalc>();
        }

        public CombinedEffect(params IEffectCalc[] childEffects)
        {
            ChildEffects = new List<IEffectCalc>(childEffects);
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
using System.Collections.Generic;
using System.Linq;
using Core.Targeting.Conditions;
using SadPumpkin.Util.CombatEngine.Actor;
using SadPumpkin.Util.CombatEngine.TargetCalculators;

namespace Core.Targeting
{
    public class ConditionalTargetCalc : ITargetCalc
    {
        public enum TargetCount
        {
            Self,
            One,
            All
        }

        public TargetCount Count { get; set; } = TargetCount.One;
        public List<ICondition> Conditions { get; set; } = new List<ICondition>();

        public string Description => $"{Count} {string.Join(", ", Conditions.Select(x => x.Description))}";

        public bool CanTarget(IInitiativeActor sourceActor, ITargetableActor targetActor)
        {
            foreach (ICondition condition in Conditions)
            {
                if (!condition.DoesSatisfyCondition(sourceActor, targetActor))
                    return false;
            }

            return true;
        }

        public IReadOnlyCollection<IReadOnlyCollection<ITargetableActor>> GetTargetOptions(IInitiativeActor sourceActor, IReadOnlyCollection<ITargetableActor> possibleTargets)
        {
            List<IReadOnlyCollection<ITargetableActor>> results = new List<IReadOnlyCollection<ITargetableActor>>(possibleTargets.Count);
            switch (Count)
            {
                case TargetCount.Self:
                {
                    if (sourceActor is ITargetableActor sourceTargetable)
                    {
                        if (CanTarget(sourceActor, sourceTargetable))
                        {
                            results.Add(new[] {sourceTargetable});
                        }
                    }

                    break;
                }
                case TargetCount.One:
                {
                    foreach (ITargetableActor possibleTarget in possibleTargets)
                    {
                        if (CanTarget(sourceActor, possibleTarget))
                        {
                            results.Add(new[] {possibleTarget});
                        }
                    }

                    break;
                }
                case TargetCount.All:
                {
                    List<ITargetableActor> group = new List<ITargetableActor>();
                    foreach (ITargetableActor possibleTarget in possibleTargets)
                    {
                        if (CanTarget(sourceActor, possibleTarget))
                        {
                            group.Add(possibleTarget);
                        }
                    }

                    results.Add(group);
                    break;
                }
            }

            return results;
        }
    }
}
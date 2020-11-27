using System.Collections.Generic;
using Core.Abilities;
using Core.Etc;
using SadPumpkin.Util.CombatEngine;
using SadPumpkin.Util.CombatEngine.Action;
using SadPumpkin.Util.CombatEngine.Actor;
using SadPumpkin.Util.TrackableIds;

namespace Core.Actions
{
    public static class ActionUtil
    {
        private static readonly UintGenerator IdTracker = new UintGenerator(Constants.ACTION, 9999, true);

        public static IReadOnlyCollection<IAction> GetActionsFromAbility(
            IInitiativeActor sourceActor,
            IIdTracked actionSource,
            IAbility ability,
            IReadOnlyCollection<ITargetableActor> possibleTargets)
        {
            List<IAction> actions = new List<IAction>();
            if (sourceActor != null &&
                actionSource != null &&
                ability != null &&
                possibleTargets != null)
            {
                if (ability.Requirements.MeetsRequirement(sourceActor) &&
                    ability.Cost.CanAfford(sourceActor, actionSource) &&
                    ability.Target.GetTargetOptions(sourceActor, possibleTargets) is var targetGroups &&
                    targetGroups.Count > 0)
                {
                    foreach (IReadOnlyCollection<ITargetableActor> targetGroup in targetGroups)
                    {
                        actions.Add(new Action(
                            IdTracker.GetNext(),
                            true,
                            ability,
                            sourceActor,
                            targetGroup,
                            actionSource));
                    }
                }
                else
                {
                    actions.Add(new Action(
                        IdTracker.GetNext(),
                        false,
                        ability,
                        sourceActor,
                        null,
                        actionSource));
                }
            }

            return actions;
        }
    }
}
using System.Collections.Generic;
using SadPumpkin.Util.CombatEngine.Action;
using SadPumpkin.Util.CombatEngine.Actor;

namespace Core.Actions
{
    public class StandardActionGenerator : IStandardActionGenerator
    {
        public IEnumerable<IAction> GetActions(IInitiativeActor actor)
        {
            yield return new WaitAction(actor);
        }
    }
}
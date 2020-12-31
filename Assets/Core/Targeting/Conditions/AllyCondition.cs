using SadPumpkin.Util.CombatEngine.Actor;

namespace Core.Targeting.Conditions
{
    public class AllyCondition : ICondition
    {
        public string Description => "Ally";
        
        public bool DoesSatisfyCondition(IInitiativeActor sourceActor, ITargetableActor targetActor)
        {
            return sourceActor.Party == targetActor.Party;
        }
    }
}
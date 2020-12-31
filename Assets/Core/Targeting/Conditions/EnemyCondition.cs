using SadPumpkin.Util.CombatEngine.Actor;

namespace Core.Targeting.Conditions
{
    public class EnemyCondition : ICondition
    {
        public string Description => "Enemy";

        public bool DoesSatisfyCondition(IInitiativeActor sourceActor, ITargetableActor targetActor)
        {
            return sourceActor.Party != targetActor.Party;
        }
    }
}
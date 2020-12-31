using SadPumpkin.Util.CombatEngine.Actor;

namespace Core.Targeting.Conditions
{
    public interface ICondition
    {
        string Description { get; }

        bool DoesSatisfyCondition(IInitiativeActor sourceActor, ITargetableActor targetActor);
    }
}
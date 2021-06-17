using Core.Actors;

namespace Unity.Scenes.Shared.Status
{
    public interface IStatusView
    {
        IHealthView HealthView { get; }
        IStaminaView StaminaView { get; }
        IStatusEffectView StatusEffectView { get; }

        void UpdateModel(ICharacterActor actorData);
    }
}
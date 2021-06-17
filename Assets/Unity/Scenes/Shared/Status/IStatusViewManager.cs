using Core.Actors;

namespace Unity.Scenes.Shared.Status
{
    public interface IStatusViewManager
    {
        IStatusView CreateStatusView(ICharacterActor actorData);
        IHealthView CreateHealthView(ICharacterActor actorData);
        IStaminaView CreateStaminaView(ICharacterActor actorData);
        IStatusEffectView CreateStatusEffectView(ICharacterActor actorData);

        IStatusView GetStatusView(uint actorId);
        IHealthView GetHealthView(uint actorId);
        IStaminaView GetStaminaView(uint actorId);
        IStatusEffectView GetStatusEffectView(uint actorId);

        void UpdateStatusView(ICharacterActor actorData);
        void UpdateHealthView(ICharacterActor actorData);
        void UpdateStaminaView(ICharacterActor actorData);
        void UpdateStatusEffectView(ICharacterActor actorData);

        void DeleteStatusView(uint actorId);
        void DeleteHealthView(uint actorId);
        void DeleteStaminaView(uint actorId);
        void DeleteStatusEffectView(uint actorId);
    }
}
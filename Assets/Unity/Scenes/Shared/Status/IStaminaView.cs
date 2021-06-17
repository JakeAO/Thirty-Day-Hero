using Core.Actors;

namespace Unity.Scenes.Shared.Status
{
    public interface IStaminaView
    {
        void UpdateModel(ICharacterActor actorData);
    }
}
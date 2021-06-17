using Core.Actors;

namespace Unity.Scenes.Shared.Status
{
    public interface IHealthView
    {
        void UpdateModel(ICharacterActor actorData);
    }
}
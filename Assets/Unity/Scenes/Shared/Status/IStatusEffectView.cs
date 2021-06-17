using Core.Actors;

namespace Unity.Scenes.Shared.Status
{
    public interface IStatusEffectView
    {
        void UpdateModel(ICharacterActor actorData);
    }
}
using Core.Actors;

namespace Unity.Scenes.Shared.Status
{
    public interface ILevelView
    {
        void UpdateModel(ICharacterActor actorData);
    }
}
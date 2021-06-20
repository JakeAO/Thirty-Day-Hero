using Core.Actors;

namespace Unity.Scenes.Shared.Status
{
    public interface IClassLevelView
    {
        void UpdateModel(ICharacterActor actorData);
    }
}